using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using NetMQ;
using NetMQ.Sockets;
using MsgPack.Serialization;
using Utils;

namespace Communication
{
    public class NetHandler : SingletonBase<NetHandler>
    {
        private DealerSocket dealer;
        private NetMQPoller poller;
        private SemaphoreSlim semaphore;

        protected override void Awake()
        {
            AsyncIO.ForceDotNet.Force();
            semaphore = new SemaphoreSlim(1, 1);
            dealer = new DealerSocket("tcp://localhost:49155");
            dealer.Options.Identity = Encoding.Unicode.GetBytes("ClientId");
            dealer.ReceiveReady += ReceiveReady;
            poller = new NetMQPoller();
            poller.Add(dealer);
            poller.RunAsync();
        }

        public class RemoteCall
        {
            public string FunctionName { get; set; }
            public Dictionary<string, object> Params { get; set; }
            // Result of function call. This will be populated on the backend.
            public string Result { get; set; }
            // Success indicates whether the function call resulted in success or not.
            // This will be populated on the backend.
            public bool Success { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="params"></param>
        /// <returns>Success</returns>
        public async Task Call(string function, Dictionary<string, object> param)
        {
            RemoteCall rpc = new RemoteCall
            {
                FunctionName = function,
                Params = param,
                Result = "",
                Success = true
            };

            // Serialize RemoteCall to MessagePack
            MessagePackSerializer serializer = MessagePackSerializer.Get(rpc.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.Pack(stream, rpc);

            // If there is some socket on another thread, wait for it to finish and release
            await semaphore.WaitAsync();
            try
            {
                dealer.SendFrame(stream.ToArray());
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to call \"" + function + "\" with params: " + string.Join(", ", param) + ". Exception: " + e.ToString());
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void ReceiveReady(object sender, NetMQSocketEventArgs ev)
        {
            byte[] resp;
            try
            {
                resp = ev.Socket.ReceiveFrameBytes();
            }
            catch (Exception e)
            {
                Debug.Log("Failed to receive frames from server. Exception: " + e.ToString());
                return;
            }
            var toUnpack = new MemoryStream(resp);
            MessagePackSerializer serializer = MessagePackSerializer.Get(typeof(RemoteCall));
            RemoteCall unpacked;
            try
            {
                unpacked = (RemoteCall)serializer.Unpack(toUnpack);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to unpack message. Exception: " + e.ToString());
                return;
            }
            if (unpacked.Success != true)
            {
                Debug.LogError("Server returned an error for \"" + unpacked.FunctionName + "\" with params: "
                    + string.Join(", ", unpacked.Params) + ". Result: " + unpacked.Result);
                return;
            }
            API_in.CallLocal(unpacked.FunctionName, unpacked.Result);
        }

        public void Terminate()
        {
            poller.Stop();
            NetMQConfig.Cleanup(false);
        }
    }
}
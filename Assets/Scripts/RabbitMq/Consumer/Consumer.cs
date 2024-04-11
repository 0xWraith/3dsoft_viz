// using System;
// using System.Collections.Concurrent;
// using System.Text;
// using System.Threading.Tasks;
// using RabbitMQ.Clin;
// using RabbitMQ.Client.Events;
// using UnityEngine;

// // namespace RabbitMq.Consumer;

// public class Consumer : RabbitConnection
// {
//     private BlockingCollection<object> queue = new BlockingCollection<object>();

//     private Consumer(string hostName, string exchangeChannel) : base(hostName, exchangeChannel)
//     {
//     }

//     /// <summary>
//     /// Starts consumer asynchronously 
//     /// </summary>
//     public static Consumer StartInstance(string hostName, string exchangeChannel)
//     {
//         var producer = new Consumer(hostName, exchangeChannel);
//         producer.Start();
//         return producer;
//     }

//     /// <summary>
//     /// Terminates consuming of messages
//     /// </summary>
//     public void End()
//     {
//         queue.Add(this);
//     }

//     public void Start()
//     {
//         Task.Factory.StartNew(() =>
//         {
//             var factory = new ConnectionFactory { HostName = "localhost"};
//             using var connection = factory.CreateConnection();
//             using var channel = connection.CreateModel();

//             channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

//             // declare a server-named queue
//             var queueName = channel.QueueDeclare().QueueName;
//             channel.QueueBind(queue: queueName,
//                 exchange: "logs",
//                 routingKey: string.Empty);


//             Debug.Log(" [*] Waiting for logs.");

//             var consumer = new EventingBasicConsumer(channel);
//             consumer.Received += (model, ea) =>
//             {
//                 byte[] body = ea.Body.ToArray();
//                 var message = Encoding.UTF8.GetString(body);
//                 Debug.Log($"{DateTime.Now.ToString("HH:mm:ss.ffffff")} Received: {message}");
//             };
//             channel.BasicConsume(queue: queueName,
//                 autoAck: true,
//                 consumer: consumer);

//             //wait until end message
//             queue.Take();

//         }, TaskCreationOptions.LongRunning);
//     }
// }

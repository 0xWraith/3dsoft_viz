using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using UnityEngine;

// namespace RabbitMq.Producer;
public class Producer : RabbitConnection
{
    private readonly BlockingCollection<string> queue = new BlockingCollection<string>();

    private Producer(string hostName, string exchangeChannel) : base(hostName, exchangeChannel)
    {
    }

    /// <summary>
    /// Starts producer asynchronously 
    /// </summary>
    public static Producer StartInstance(string hostName, string exchangeChannel)
    {
        var producer = new Producer(hostName, exchangeChannel);
        producer.Start();
        return producer;
    }


    /// <summary>
    /// Adds <paramref name="item"/> to processing queue.
    /// Added messages will be sent in first in-first out (FIFO) order.
    /// </summary>
    public void AddToQueue(string item)
    {
        queue.Add(item);
    }

    private void Start()
    {
        Task.Factory.StartNew(() =>
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
            while (true)
            {
                var item = queue.Take();
                var message = item;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                    routingKey: string.Empty,
                    basicProperties: null,
                    body: body);
                //Console.WriteLine($" [x] Sent {message}");
                Debug.Log($"{DateTime.Now.ToString("HH:mm:ss.ffffff")} Sent: {message}");
            }

        }, TaskCreationOptions.LongRunning);
    }
}

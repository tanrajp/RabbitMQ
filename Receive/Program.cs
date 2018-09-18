using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    var factory = new ConnectionFactory() { HostName = "localhost" };
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        channel.QueueDeclare("task_queue", true, false, false, null);

        //        var consumer = new EventingBasicConsumer(channel);

        //        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        //        Console.WriteLine(" [*] Waiting for messages.");

        //        consumer.Received += (model, ea) =>
        //        {
        //            var body = ea.Body;

        //            var message = Encoding.UTF8.GetString(body);
        //            Console.WriteLine(" [x] Received {0}", message);

        //            int dots = message.Split('.').Length - 1;
        //            Thread.Sleep(dots * 1000);

        //            Console.WriteLine(" [x] Done");

        //            channel.BasicAck(ea.DeliveryTag, false); // ack is false
        //        };

        //        channel.BasicConsume(queue: "task_queue",
        //             autoAck: false, //set this to false if sending ack manually
        //             consumer: consumer);

        //        Console.WriteLine(" Press [enter] to exit.");
        //        Console.ReadLine();
        //    }
        //}
    }
}

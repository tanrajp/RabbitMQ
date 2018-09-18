using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCClient
{
    public class RpcClient
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "rpcExchange", type: "direct");

            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);
        }

        public string Call(string message)
        {
            var tcs = new TaskCompletionSource<string>();
            var resultTask = tcs.Task;

            var correlationId = Guid.NewGuid().ToString();

            IBasicProperties props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            EventHandler<BasicDeliverEventArgs> handler = null;
            handler = (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    consumer.Received -= handler;

                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body);

                    tcs.SetResult(response);
                }
            };
            consumer.Received += handler;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "rpcExchange",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);


            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return resultTask.Result;
        }

        public string Add(string message)
        {
            var tcs = new TaskCompletionSource<string>();
            var resultTask = tcs.Task;

            var correlationId = Guid.NewGuid().ToString();

            IBasicProperties props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            EventHandler<BasicDeliverEventArgs> handler = null;
            handler = (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    consumer.Received -= handler;

                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body);

                    tcs.SetResult(response);
                }
            };
            consumer.Received += handler;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "rpcExchange",
                routingKey: "rpc_queue_add",
                basicProperties: props,
                body: messageBytes);


            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return resultTask.Result;
        }

        public void Close()
        {
            connection.Close();
        }



    }
}

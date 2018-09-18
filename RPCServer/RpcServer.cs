using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCServer
{
    public class RpcServer
    {
        private IModel channel;
        //private EventingBasicConsumer consumer;

        public RpcServer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };



            using (var connection = factory.CreateConnection())
            using (channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "rpcExchange", type: "direct");
                
                channel.BasicQos(0, 1, false);

                //consumer = new EventingBasicConsumer(channel);

                Fibonacci();
                Add();

                Console.WriteLine(" [x] Awaiting RPC requests");



                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }

        public void Add()
        {
            var queue = channel.QueueDeclare("rpc_queue_add", false, false, false, null);
            channel.QueueBind(queue.QueueName, "rpcExchange", "rpc_queue_add", null);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("rpc_queue_add", false, consumer);

            consumer.Received += (model, ea) =>
            {
                string response = null;
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();

                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    int n = int.Parse(message);
                    Console.WriteLine(" [.] add({0})", message);
                    response = (n * 2).ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                    channel.BasicAck(ea.DeliveryTag, false);

                }
            };

        }

        public void Fibonacci()
        {
            var queue = channel.QueueDeclare("rpc_queue", false, false, false, null);
            channel.QueueBind(queue.QueueName, "rpcExchange", "rpc_queue", null);

            //channel.QueueDeclare("rpc_queue", false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume("rpc_queue", false, consumer);
            consumer.Received += (model, ea) =>
            {
                string response = null;
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();

                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    int n = int.Parse(message);
                    Console.WriteLine(" [.] fib({0})", message);
                    response = fib(n).ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                    channel.BasicAck(ea.DeliveryTag, false);

                }
            };
        }

        private int fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }
    }
}

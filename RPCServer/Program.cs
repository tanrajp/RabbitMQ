using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCServer
{
    public class Program
    {


        public static void Main(string[] args)
        {
            new RpcServer();

            Console.Read();
        }




    }
}

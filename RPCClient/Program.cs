using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var rpcClient = new RpcClient();

            Console.WriteLine(" [x] Requesting fib(30)");
            //var response = rpcClient.Call("30");
            var response = rpcClient.Add("2");

            Console.WriteLine(" [.] Got '{0}'", response);
            Console.Read();
            rpcClient.Close();

        }
    }
}

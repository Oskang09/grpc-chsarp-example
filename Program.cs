using System;
using System.IO;
using amantiq;

namespace company_service
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start();
            string signature = File.ReadAllText("./signature.txt");
            Console.WriteLine(signature);
            Console.WriteLine("|————————————————|——————————————————————————————————————————————————————————————————————————————————————————————|");
            Console.WriteLine("|  1. Jaeger     | {0}", amantiq.constant.Environment.JAEGER_HOST + ":" + amantiq.constant.Environment.JAEGER_PORT);
            Console.WriteLine("|  2. KubeMQ     | {0}", amantiq.constant.Environment.KUBEMQ_ADDR);
            Console.WriteLine("|  3. PostgreSQL | {0}", amantiq.constant.Environment.CONNECTION_STRING);
            Console.WriteLine("|————————————————|——————————————————————————————————————————————————————————————————————————————————————————————|");
            Console.Read();
        }
    }
}

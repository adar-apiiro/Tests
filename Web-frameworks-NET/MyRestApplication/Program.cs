using System;
using OpenRasta.Hosting.InMemory;
using OpenRasta.Configuration;

namespace MyRestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new InMemoryHost(new Configuration());
            Console.WriteLine("OpenRasta API Server is running. Press any key to stop.");
            Console.ReadKey();
        }
    }
}
using System;
using System.ServiceModel; 

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Services.Implementations.BlockusService)))
            {
                host.Open();
                Console.WriteLine("Server is running");
                Console.WriteLine("Press any key to exit ...");
                Console.ReadLine(); 
            }
        }
    }
}

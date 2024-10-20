using System;
using System.ServiceModel;

namespace Host
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Services.Implementations.ServiceImplementation)))
            {
                host.Open();
                Console.WriteLine("Server is running");
                Console.WriteLine("Press any key to exit ...");
                Console.ReadLine(); 
            }
        }
    }
}

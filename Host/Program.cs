using log4net;
using System;
using System.ServiceModel;

namespace Host
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

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

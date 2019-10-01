using System;

namespace Consul.AspNetCore.ServiceDiscovery.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var consulClient = new ConsulClient(c => c.Address = new Uri(@"http://consul:8500")))
            {
                if (consulClient.Catalog.Service("example").Result.Response.Length > 0)
                {
                    var serviceData = consulClient.Catalog.Service("example").Result.Response[0];
                    Console.WriteLine($"Your service url is {serviceData.ServiceAddress}:{serviceData.ServicePort}");
                }
            }                
        }
    }
}

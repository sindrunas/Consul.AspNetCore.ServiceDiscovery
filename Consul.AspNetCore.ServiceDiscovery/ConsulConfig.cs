namespace Consul.AspNetCore.ServiceDiscovery
{
    public class ConsulConfig
    {
        public string ServiceAddress { get; set; }
        public string ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string[] Tags { get; set; }        
    }
}
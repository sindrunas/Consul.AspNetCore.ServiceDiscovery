using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Consul.AspNetCore.ServiceDiscovery
{
    public class ServiceDiscovery
    {
        readonly string _host;

        public ServiceDiscovery(IConfiguration configuration)
        {
            _host = configuration["ServiceConfig:serviceDiscoveryAddress"];
        }

        public CatalogService GetServiceInfo(string serviceName)
        {
            CatalogService ret = null;
            using (var consulClient = new ConsulClient(c => c.Address = new Uri(_host)))
            {
                while (ret == null)
                {
                    ret = consulClient.Catalog.Service(serviceName).Result.Response.Length > 0 ? consulClient.Catalog.Service(serviceName).Result.Response[0] : null;
                }
            }

            return ret;
        }
    }
}

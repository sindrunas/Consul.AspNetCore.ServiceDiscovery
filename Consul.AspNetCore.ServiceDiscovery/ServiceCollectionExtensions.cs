﻿using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consul.AspNetCore.ServiceDiscovery
{
    public static class ServiceCollectionExtensions
    {
        private static string _host;

        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsulConfig>(configuration.GetSection("ServiceConfig"));
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var host = configuration["ServiceConfig:serviceDiscoveryAddress"];
                _host = host;
                consulConfig.Address = new Uri(host);
            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var consulConfig = app.ApplicationServices.GetRequiredService<IOptions<ConsulConfig>>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Consul.AspNetCore");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            
            var uri = new Uri(consulConfig.Value.ServiceAddress);
            var registration = new AgentServiceRegistration()
            {
                ID = $"{consulConfig.Value.ServiceId}-{uri.Port}",
                Name = consulConfig.Value.ServiceName,
                Address = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString(),
                Port = uri.Port,
                Tags = consulConfig.Value.Tags
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            });

            return app;
        }

        public static CatalogService GetServiceAddress(string serviceName)
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
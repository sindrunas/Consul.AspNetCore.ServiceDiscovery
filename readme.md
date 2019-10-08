# Consul.AspNetCore.ServiceDiscovery

![NuGet](https://buildstats.info/nuget/Consul.AspNetCore.ServiceDiscovery)

Consul.AspNetCore.ServiceDiscovery is an extension that helps you to implement Service Discovery with Consul very easy.

If you need to register a service in Consul you've got to add this lines in Startup.cs:

```c#
public void ConfigureServices(IServiceCollection services)
{
	...
	services.AddConsul(Configuration);
	services.AddMvc();
}
```

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
	...
	app.UseMvc();
	app.UseConsul();
}
```

and then add this in the appsettings.json

		"ServiceConfig": {
			"ServiceDiscoveryAddress": "http://consul:8500",
			"ServiceAddress": "http://example.api:80",
			"ServiceName": "example",
			"ServiceId": "example-v1"
		  }

And that's all. Now your service is being registered to Consul.


After that if you want to retrieve your service's url from your API Gateway or App you can do it like this:

```c#
var svcDiscovery = new ServiceDiscovery(configuration);
var yourService = svcDiscovery.GetServiceInfo("example");
```
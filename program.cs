IHost host = Host.CreateDefaultBuilder(args)
                 .ConfigureServices(services => {
	                                    services.AddHostedService<Worker>();
                                    })
                 .UseElasticApm()
                 .Build();

await host.RunAsync();

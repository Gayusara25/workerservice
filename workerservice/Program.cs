

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<BackgroundWorkerService>();
    })
    .Build();

await host.RunAsync();

using Letters.Data;
using Letters.Handler;
using Letters.Handler.Interfaces;
using Letters.Handler.Services;
using Letters.Handler.Settings;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReceptionContext>();
        optionsBuilder.UseNpgsql(hostContext.Configuration.GetConnectionString("ReceptionConnection"));

        services.Configure<ServiceSettings>(hostContext.Configuration.GetSection(nameof(ServiceSettings)));

        services.AddScoped(_ => new ReceptionContext(optionsBuilder.Options));
        services.AddSingleton<ILetterSenderService, LetterSenderService>();

        services.AddHostedService<Sender>();
    })
    .Build();

await host.RunAsync();

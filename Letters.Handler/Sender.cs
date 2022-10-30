using Letters.Data;
using Letters.Handler.Interfaces;

namespace Letters.Handler
{
    public class Sender : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceProviderFactory;
        private readonly ILetterSenderService _letterSenderService;

        public Sender(IServiceScopeFactory serviceScopeFactory, ILetterSenderService letterSenderService)
        {
            _serviceProviderFactory = serviceScopeFactory;
            _letterSenderService = letterSenderService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProviderFactory.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<ReceptionContext>();

                await _letterSenderService.Send(context);


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
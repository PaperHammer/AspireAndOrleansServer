namespace Email.Api.Utils
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly RabbitMQReceiverService _receiverService;

        public RabbitMQBackgroundService(RabbitMQReceiverService receiverService)
        {
            _receiverService = receiverService ?? throw new ArgumentNullException(nameof(receiverService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _receiverService.StartReceiving();
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}

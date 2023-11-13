using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WebApplication1.Messaging;

namespace WebApplication1
{
    public class Receiver : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private RabbitMqConfiguration _configuration;
        public Receiver(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
            _connection = RabbitMqMessenger.CreateConnection(_configuration);
            _channel = _connection.CreateModel();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                RabbitMqMessenger.ReceiveJson(_configuration.QueueName, _channel);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
            }

            if (_connection != null)
            {
                _connection.Close();
            }

            base.Dispose();
        }
    }
}

using WebApplication1.Messaging;

namespace WebApplication1
{
    public class Receiver : BackgroundService
    {
        IMessenger _messenger;
        public Receiver(IMessenger messenger)
        {
            _messenger = messenger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _messenger.ReceiveJson();
                if (message != null && (message is Message))
                {
                    var typedMessage = (Message)message;
                    Console.WriteLine($"{typedMessage.input_value} {typedMessage.computed_value} {typedMessage.previous_value}");
                }
                else
                {
                    //Console.WriteLine($"Received null object");
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace WebApplication1
{
    public class Receiver : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TestReceive();
                Console.WriteLine("test");
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void TestReceive()
        {
            var factory = new ConnectionFactory { 
                HostName = "172.19.0.3",
                //Port = 5672,
                Port = 5672,
                UserName = "guest", 
                Password = "guest", 
                VirtualHost="/"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var tmp = JsonSerializer.Deserialize<ComputationResponse>(message);
                if (tmp != null)
                {
                    Console.WriteLine($" [x] Received {tmp.input_value} {tmp.previous_value}, {tmp.computed_value}");
                }
                else
                {
                    Console.WriteLine("null received");
                }
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}

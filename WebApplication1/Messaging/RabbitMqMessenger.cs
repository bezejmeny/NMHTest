using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace WebApplication1.Messaging
{
    public class RabbitMqMessenger : IMessenger
    {
        private readonly RabbitMqConfiguration _configuration;
        public RabbitMqMessenger(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
        }

        public IConnection CreateChannel()
        {
            ConnectionFactory connection = new()
            {
                UserName = _configuration.Username,
                Password = _configuration.Password,
                HostName = _configuration.HostName,
                Port = 5672
            };
            Debug.WriteLine($"Username: {_configuration.Username}, Password: {_configuration.Password}, Hostname: {_configuration.HostName}");
            Console.WriteLine($"Username: {_configuration.Username}, Password: {_configuration.Password}, Hostname: {_configuration.HostName}");
            var channel = connection.CreateConnection();
            return channel;
        }

        public void SendAsJson(object data, string queue = "defaultQueue")
        {
            using (IConnection connection = CreateChannel())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false, false, false, null);

                    string serializedObject = JsonSerializer.Serialize(data);
                    var body = Encoding.UTF8.GetBytes(serializedObject);
                    channel.BasicPublish(string.Empty, queue, null, body);
                    Console.WriteLine($"Sent data");
                }
            }
        }

        public object? ReceiveJson(string queue = "defaultQueue")
        {
            object? receivedObject = null;
            using (IConnection connection = CreateChannel())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    Console.WriteLine("Waiting for messages...");
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            receivedObject = JsonSerializer.Deserialize<Message>(message);
                        }

                        Console.WriteLine("Received data");
                    };

                    channel.BasicConsume(queue: queue,
                        autoAck: true,
                        consumer: consumer);
                }
            }

            return receivedObject;
        }
    }
}

using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication1.Messaging
{
    public class RabbitMqMessenger : IMessenger, IDisposable
    {
        private readonly RabbitMqConfiguration _configuration;
        private IConnection _connection;

        public RabbitMqMessenger(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
            _connection = CreateConnection(_configuration);
        }

        public void SendAsJson(object data)
        {
            using (IModel channel = _connection.CreateModel())
            {
                channel.QueueDeclare(_configuration.QueueName, false, false, false, null);

                //TODO: Verify with PO, BL behavior for Infinite/NaN 
                string serializedObject = JsonSerializer.Serialize(data, new JsonSerializerOptions { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals });
                var body = Encoding.UTF8.GetBytes(serializedObject);
                channel.BasicPublish(string.Empty, _configuration.QueueName, null, body);
            }
        }

        public static IConnection CreateConnection(RabbitMqConfiguration _configuration)
        {
            ConnectionFactory connection = new()
            {
                UserName = _configuration.Username,
                Password = _configuration.Password,
                HostName = _configuration.HostName,
                Port = 5672
            };
            var channel = connection.CreateConnection();
            return channel;
        }

        public static void ReceiveJson(string queueName, IModel channel)
        {
            channel.QueueDeclare(queueName, false, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    //TODO: Verify with PO, BL behavior for Infinite/NaN 
                    var tmp = JsonSerializer.Deserialize<Message>(message, new JsonSerializerOptions { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals });
                    Console.WriteLine($"{tmp?.input_value} {tmp?.computed_value} {tmp?.previous_value}");
                }
            };
            channel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }
    }
}

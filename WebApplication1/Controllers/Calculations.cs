using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Calculations : ControllerBase
    {
        [HttpPost(Name = "Calculation")]
        public ComputationResponse Calculation(int key)
        {
            var tmp = HandleInput(key);
            TestSend(tmp);
            return tmp;
        }

        private ComputationResponse HandleInput(int input)
        {
            if (!GlobalStorage.Storage.ContainsKey(input))
            {
                var complexValue = new ComplexValue() { Value = 2, TimeStamp = DateTime.Now };
                GlobalStorage.Storage.Add(input, complexValue);
                return new ComputationResponse(input, null, complexValue.Value);
            }
            else if (GlobalStorage.Storage.ContainsKey(input) && GlobalStorage.Storage[input].TimeStamp > DateTime.Now.AddSeconds(-15))
            {
                var complexValue = new ComplexValue() { Value = 2, TimeStamp = DateTime.Now };
                GlobalStorage.Storage[input] = complexValue;
                return new ComputationResponse(input, null, complexValue.Value);
            }
            else
            {
                var originalValue = GlobalStorage.Storage[input].Value;
                decimal inputValue = input / originalValue;
                inputValue = (decimal)Math.Log((double)inputValue);
                inputValue = (decimal)Math.Cbrt((double)inputValue);
                var complexValue = new ComplexValue() { Value = inputValue, TimeStamp = DateTime.Now };
                GlobalStorage.Storage[input] = complexValue;
                return new ComputationResponse(input, originalValue, complexValue.Value);
            }
        }


        private void TestSend(ComputationResponse computationResponse)
        {
            var factory = new ConnectionFactory { 
                HostName = "172.19.0.3",
                //Port = 5672,
                Port = 5672,
                UserName = "guest", 
                Password = "guest", 
                VirtualHost = "/"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = JsonSerializer.Serialize(computationResponse);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");
        }
    }
}

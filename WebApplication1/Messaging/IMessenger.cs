using RabbitMQ.Client;

namespace WebApplication1.Messaging
{
    public interface IMessenger
    {
        void SendAsJson(object data);
    }
}

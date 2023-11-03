using RabbitMQ.Client;

namespace WebApplication1.Messaging
{
    public interface IMessenger
    {
        IConnection CreateChannel();

        void SendAsJson(object data, string queue = "defaultQueue");
        object? ReceiveJson(string queue = "defaultQueue");
    }
}

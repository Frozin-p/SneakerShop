using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AuthService.Services
{
    public class RabbitMqPublisher
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "UserCreatedQueue";
        private IConnection _connection;

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
        }

        public void Publish<T>(T message)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("Сообщение отправлено");
        }
    }
}

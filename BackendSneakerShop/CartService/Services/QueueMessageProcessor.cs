using CartService.Abstractions;
using CartService.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CartService.Services
{
    public class QueueMessageProcessor
    {
        private readonly string _queueName = "UserCreatedQueue";
        private readonly ICartService _cartService;
        private readonly ILogger<QueueMessageProcessor> _logger;
        private readonly IServiceProvider _serviceProvider;

        public QueueMessageProcessor(ICartService cartService, IServiceProvider serviceProvider, ILogger<QueueMessageProcessor> logger)
        {
            _cartService = cartService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void StartListening()
        {
            _logger.LogInformation("Start listening to the queue...");

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Подписка на очередь
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                _logger.LogInformation("Received message: " + message);

                if (userCreatedEvent != null)
                {
                    _logger.LogInformation("Processing user created event...");

                    try
                    {                        
                        await _cartService.CreateCartAsync(new CartDto
                        {
                            UserId = userCreatedEvent.UserId,
                        });

                        _logger.LogInformation("Cart created for user: " + userCreatedEvent.UserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error while processing user created event for user {userCreatedEvent.UserId}: {ex.Message}");
                    }
                }
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            _logger.LogInformation("Consuming messages...");
        }
    }


    public class UserCreatedEvent
    {
        public Guid UserId { get; set; }
    }
}

using Email.App.Contract.Data;
using Email.App.Contract.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Email.Api.Utils
{
    public class RabbitMQReceiverService : IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly string _queueName = "config_updates_queue";
        private EventingBasicConsumer? _consumer;
        //private readonly string _rabbitMqHost = "config_rabbitmq_host";

        public RabbitMQReceiverService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                ContinuationTimeout = new(int.MaxValue / 2), 
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _queueName, exchange: "config_updates", routingKey: "");
        }

        public void StartReceiving()
        {
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                EmailConfig emailConfig = JsonSerializer.Deserialize<EmailConfig>(message);
                EmailUtil.EmailConfig = new()
                {
                    ServeEmail = emailConfig.ServeEmail,
                    SMTPPort = emailConfig.SMTPPort,
                    SMTPServer = emailConfig.SMTPServer,
                    AuthorizationCode = emailConfig.AuthorizationCode,
                };
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: _consumer);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}

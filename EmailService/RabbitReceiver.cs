using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using EmailService.Services;
using EmailService.Models;

namespace EmailService
{
    public class RabbitReceiver
    {
        private readonly Func<EmailSender> _emailSenderFactory;

        public RabbitReceiver(Func<EmailSender> emailSenderFactory)
        {
            _emailSenderFactory = emailSenderFactory;
        }

        public void ReceiveMessages()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "emailQueue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");

                // Deserialize message to Email object
                var email = JsonConvert.DeserializeObject<Email>(message);
                if (email != null)
                {
                    // Create a new instance of EmailSender from the factory when needed
                    var emailSender = _emailSenderFactory();
                    emailSender.SendEmail(email.To, email.Subject, email.Body);
                }
            };

            channel.BasicConsume(queue: "emailQueue",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}

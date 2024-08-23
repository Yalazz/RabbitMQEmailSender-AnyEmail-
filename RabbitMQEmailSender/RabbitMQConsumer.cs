using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

public class RabbitMQConsumer
{
    private readonly string _hostName = "localhost";
    private readonly string _queueName = "emailQueue";

    public void StartConsumer()
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageBody = Encoding.UTF8.GetString(body);
                var messageParts = messageBody.Split('|');

                if (messageParts.Length < 5)
                {
                    Console.WriteLine("Geçersiz mesaj formatı: {0}", messageBody);
                    return;
                }

                var recipientEmail = messageParts[0];
                var subject = messageParts[1];
                var message = messageParts[2];
                var smtpEmail = messageParts[3];
                var smtpPassword = messageParts[4];

                Console.WriteLine("Mesaj alındı: {0}", messageBody);

                // E-posta gönder
                var emailService = new EmailService();
                await emailService.SendEmailAsync(recipientEmail, subject, message, smtpEmail, smtpPassword);

                // Mesajı manuel olarak onayla
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            // autoAck: false olarak ayarlandı
            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Consumer çalışıyor. Çıkmak için herhangi bir tuşa basın.");
            Console.ReadLine();
        }
    }
}

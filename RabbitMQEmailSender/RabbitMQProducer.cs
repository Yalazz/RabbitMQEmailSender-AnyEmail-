using System;
using RabbitMQ.Client;
using System.Text;

public class RabbitMQProducer
{
    private readonly string _hostName = "localhost";
    private readonly string _queueName = "emailQueue";

    public void SendEmail(string recipientEmail, string subject, string body, string smtpEmail, string smtpPassword)
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

            var message = $"{recipientEmail}|{subject}|{body}|{smtpEmail}|{smtpPassword}";
            var bodyEncoded = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: bodyEncoded
            );

            Console.WriteLine("Mesaj kuyruğa eklendi: {0}", message);
        }
    }
}

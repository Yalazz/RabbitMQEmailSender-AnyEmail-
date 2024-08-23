using System;

class Program
{
    static void Main(string[] args)
    {
        // Kullanıcıya bilgi vermek
        Console.WriteLine("RabbitMQ Email Sender");

        // E-posta gönderimi için RabbitMQProducer sınıfından yeni bir örnek oluştur
        var producer = new RabbitMQProducer();

        // E-posta gönderecek SMTP bilgilerini almak için kullanıcıdan bilgi al
        Console.WriteLine("Gönderici e-posta adresinizi girin:");
        var smtpEmail = Console.ReadLine();

        Console.WriteLine("E-posta şifrenizi girin:");
        var smtpPassword = Console.ReadLine();

        // E-posta gönderme bilgilerini almak için kullanıcıdan bilgi al
        Console.WriteLine("E-posta gönderilecek adresi girin:");
        var recipientEmail = Console.ReadLine();

        Console.WriteLine("E-posta başlığını girin:");
        var subject = Console.ReadLine();

        Console.WriteLine("E-posta içeriğini girin:");
        var body = Console.ReadLine();

        // E-posta bilgilerini RabbitMQProducer ile kuyruğa ekleyin
        producer.SendEmail(recipientEmail, subject, body, smtpEmail, smtpPassword);

        // RabbitMQConsumer sınıfından yeni bir örnek oluştur ve tüketiciyi başlat
        var consumer = new RabbitMQConsumer();
        consumer.StartConsumer();
    }
}

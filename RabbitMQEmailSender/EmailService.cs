using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    public async Task SendEmailAsync(string recipientEmail, string subject, string message, string smtpEmail, string smtpPassword)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Sender", smtpEmail));
        emailMessage.To.Add(new MailboxAddress("Recipient", recipientEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = message };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpEmail, smtpPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}

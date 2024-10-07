using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SnapTalk.BLL.Interfaces;

namespace SnapTalk.BLL.Services;

public class EmailService(EmailConfig emailConfig) : IEmailService
{
    public async Task SendEmailAsync(string receiver, string subject, string message)
    {
        var email = BuildEmailMessage(receiver, subject, message);
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(emailConfig.Host, emailConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailConfig.Email, emailConfig.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
    
    private MimeMessage BuildEmailMessage(string receiver, string subject, string message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(emailConfig.Email));
        email.To.Add(MailboxAddress.Parse(receiver));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
        return email;
    }
}
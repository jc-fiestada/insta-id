using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

using InstaId.Models.Entity;

namespace InstaId.Services.Service;

public class Tools
{
    public async Task SendIdViaGmail(IEntity entity, byte[] pdfBytes)
    {

        string entityType = entity is Student ? "STUDENT" : "EMPLOYEE";

        string gmailAccount = Environment.GetEnvironmentVariable("gmail_account") ?? throw new InvalidOperationException("Gmail Account is Missing");
        string gmailAppPassword = Environment.GetEnvironmentVariable("app_password") ?? throw new InvalidOperationException("Gmail App Password is Missing");

        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("Employee Management", gmailAccount));
        message.To.Add(MailboxAddress.Parse(entity.Gmail));
        message.Subject = $"{entityType} ID - Issuance Notice";

        TextPart textPart = new TextPart("plain")
        {
            Text = $"Hello {entity.Name}, Attached is you official ID."  
        };

        MimePart pdfAttachment = new MimePart("application", "pdf")
        {
            Content = new MimeContent(new MemoryStream(pdfBytes)),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = $"{entityType}-id.pdf"
        };

        Multipart multipart = new Multipart("mixed");
        multipart.Add(textPart);
        multipart.Add(pdfAttachment);

        message.Body = multipart;

        using SmtpClient smtp = new SmtpClient();
        smtp.Timeout = 20000;
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(gmailAccount, gmailAppPassword);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
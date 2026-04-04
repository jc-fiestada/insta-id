using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

using InstaId.Models.Entity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace InstaId.Services.Service;

public class Tools
{
    private readonly string _gmailAccount;
    private readonly string _gmailAppPassword;

    public Tools(string gmailAccount, string gmailAppPassword)
    {
        _gmailAccount = gmailAccount;
        _gmailAppPassword = gmailAppPassword;
    }

    public async Task SendIdViaGmail(IEntity entity, byte[] pdfBytes)
    {

        string entityType = entity is Student ? "STUDENT" : "EMPLOYEE";

        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("Insta Id", _gmailAccount));
        message.To.Add(MailboxAddress.Parse(entity.Gmail));
        message.Subject = $"{entityType} ID - Issuance Notice";

        TextPart textPart = new TextPart("plain")
        {
            Text = $"Hello {entity.Name}, Attached is your official ID."  
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
        await smtp.AuthenticateAsync(_gmailAccount, _gmailAppPassword);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }

    public async Task<byte[]> OptimizeImage(IFormFile image)
{
        using var stream = image.OpenReadStream();
        using var img = await Image.LoadAsync(stream);

        img.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(300, 300),
            Mode = ResizeMode.Crop
        }));

        using var output = new MemoryStream();

        await img.SaveAsJpegAsync(output, new JpegEncoder
        {
            Quality = 70
        });

        return output.ToArray();
    }
}
using System.Text;
using System.Text.Json;

using InstaId.Models.Entity;
using InstaId.Services.Service;

namespace InstaId.Services.Service;

public class PdfShark
{
    private static readonly HttpClient client = new HttpClient()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };
    public async Task<byte[]> GeneratePdf(IEntity entity, IFormFile img, Tools tool)
    {
        byte[] optimizedImage;
        try
        {
            optimizedImage = await tool.OptimizeImage(img);
        } catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
            throw new Exception("Image optimization failed");
        }

        string templateFilepath = Path.Combine("templates", $"id{entity.Id}.html");

        string template = await File.ReadAllTextAsync(templateFilepath);

        string html = template
            .Replace("{{name}}", entity.Name)
            .Replace("{{gmail}}", entity.Gmail)
            .Replace("{{institute}}", entity.Institute)
            .Replace("{{role}}", entity.Role)
            .Replace("{{img}}", $"data:image/jpeg;base64,{Convert.ToBase64String(optimizedImage)}");

        if (entity is Student)
        {
            html = html
                .Replace("{{instituteLabel}}", "School")
                .Replace("{{roleLabel}}", "Course")
                .Replace("{{typeLabel}}", "Student");
        }

        if (entity is Employee)
        {
            html = html
                .Replace("{{instituteLabel}}", "Company")
                .Replace("{{roleLabel}}", "Position")
                .Replace("{{typeLabel}}", "Employee");
        }

        byte[] pdfBytes;

        try
        {
            using StringContent content = new StringContent(
                JsonSerializer.Serialize(new { html }),
                Encoding.UTF8,
                "application/json"
            );

            string apiUrl = "https://pdfspark.dev/api/v1/pdf/from-html";

            using HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("");

            pdfBytes = await response.Content.ReadAsByteArrayAsync();
        } catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
            throw new Exception("Failed to generate PDF.");
        }

        return pdfBytes;
    }
}
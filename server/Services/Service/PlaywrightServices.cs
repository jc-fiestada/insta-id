using InstaId.Models.Entity;
using Microsoft.Playwright;

namespace InstaId.Services.Service;

public class PlaywrightServices
{

    private static IBrowser _browser = null!;
    private static readonly SemaphoreSlim _pdfSemaphore = new SemaphoreSlim(3);

    public static async Task InitializeBrowser()
    {
        IPlaywright playwright = await Playwright.CreateAsync();
        _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    public async Task<byte[]> GeneratePdfBytes(IEntity entity, IFormFile img)
    {
        byte[] imgBytes;
        using (Stream stream = img.OpenReadStream())
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                imgBytes = memoryStream.ToArray();
            }
        }

        string templateFilepath = Path.Combine("templates", $"id{entity.Id}.html");

        string template = await File.ReadAllTextAsync(templateFilepath);

        string html = template
            .Replace("{{name}}", entity.Name)
            .Replace("{{gmail}}", entity.Gmail)
            .Replace("{{institute}}", entity.Institute)
            .Replace("{{role}}", entity.Role)
            .Replace("{{img}}", $"data:image/jpeg;base64,{Convert.ToBase64String(imgBytes)}");

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
            await _pdfSemaphore.WaitAsync();
            IPage page = await _browser.NewPageAsync();

            await page.SetContentAsync(html);

            pdfBytes = await page.PdfAsync(new PagePdfOptions
            {
                PrintBackground = true,
                Width = "60mm",
                Height = "92.6mm",
                Margin = new Margin
                {
                    Top = "0mm",
                    Bottom = "0mm",
                    Left = "0mm",
                    Right = "0mm"
                }
            });
            await page.CloseAsync();
        } catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new InvalidOperationException($"----- Failed to generate PDF -----");
        } finally
        {
            _pdfSemaphore.Release();
        }

        return pdfBytes;
    }
}
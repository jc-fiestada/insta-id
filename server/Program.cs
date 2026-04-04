using InstaId.Services.Service;
using InstaId.ResponseHandler;

using System.Threading.RateLimiting;
using Azure.Identity;
using DotNetEnv;
using Microsoft.AspNetCore.RateLimiting;

string gmailAccount;
string gmailAppPassword;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production ? 
        Environments.Production : Environments.Development
});

if (builder.Environment.IsDevelopment())
{
    string envFilepath = Path.Combine("..", "..", "DummyGmail", "gmail.env");
    Env.Load(envFilepath);
    gmailAccount = Environment.GetEnvironmentVariable("GmailAccount") ?? throw new Exception("Gmail Account is Missing!");
    gmailAppPassword = Environment.GetEnvironmentVariable("GmailAppPassword") ?? throw new Exception("Gmail App Password is Missing!");
}
else
{
    Uri keyVaultUrl = new Uri("https://instaid-kv.vault.azure.net/");
    DefaultAzureCredential credential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, credential);

    gmailAccount = builder.Configuration["GmailAccount"] ?? throw new Exception("Gmail Account is Missing!");
    gmailAppPassword = builder.Configuration["GmailAppPassword"] ?? throw new Exception("Gmail App Password is Missing!");
}

string port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddRateLimiter(options =>
{
    options.AddTokenBucketLimiter("token", opt =>
    {
        opt.TokenLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 5;
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
        opt.TokensPerPeriod = 2;
    });
});

builder.Services.AddScoped<PdfShark>();
builder.Services.AddScoped(_ => new Tools(gmailAccount, gmailAppPassword));
builder.Services.AddScoped<Response>();

var app = builder.Build();

app.UseStaticFiles();

app.MapPost("/generate-id", async (HttpRequest request, PdfShark pdfShark, Response response, Tools tools) =>
{
    return await response.GenerateId(request, pdfShark, tools);
}).RequireRateLimiting("token");

app.Run();

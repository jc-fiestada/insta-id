using InstaId.Services.Service;
using InstaId.ResponseHandler;

using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production ? 
        Environments.Production : Environments.Development
});

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
builder.Services.AddScoped<Tools>();
builder.Services.AddScoped<Response>();

var app = builder.Build();

app.UseStaticFiles();

app.MapPost("/generate-id", async (HttpRequest request, PdfShark pdfShark, Response response, Tools tools) =>
{
    return await response.GenerateId(request, pdfShark, tools);
}).RequireRateLimiting("token");

app.Run();

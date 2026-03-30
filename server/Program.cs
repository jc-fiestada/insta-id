using DotNetEnv;
using InstaId.Models.Dto;
using InstaId.Models.Entity;
using InstaId.Services.ModelValidator;
using InstaId.Services.Service;
using InstaId.ResponseHandler;

using Org.BouncyCastle.Utilities;
using Microsoft.Playwright;

string envFilepath = Path.Combine("..", "..", "DummyGmail", "gmail.env");
Env.Load(envFilepath);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PlaywrightServices>();
builder.Services.AddScoped<ValidateEmployee>();
builder.Services.AddScoped<ValidateStudent>();
builder.Services.AddScoped<Response>();

var app = builder.Build();

await PlaywrightServices.InitializeBrowser();

app.UseStaticFiles();

app.MapPost("/student/pdf-id", () =>
{
    
});

app.MapPost("/employee/pdf-id", () =>
{
    
});



app.MapPost("/test/pdf", (HttpRequest request, ValidateStudent validator, PlaywrightServices playwright, Response response) =>
{
    return response.GenerateId(request, playwright);
});

app.Run();

using InstaId.Models.Dto;
using InstaId.Models.Entity;
using InstaId.Services.ModelValidator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ValidateEmployee>();
builder.Services.AddScoped<ValidateStudent>();

var app = builder.Build();

app.UseStaticFiles();

app.MapPost("/student/pdf-id", () =>
{
    
});

app.MapPost("/employee/pdf-id", () =>
{
    
});

app.MapPost("/test/employee", (EmployeeDto employee, ValidateEmployee e) =>
{
    return Results.Json(e.Validate(employee), statusCode: 200);
});


app.MapPost("/test/student", (StudentDto student, ValidateStudent s) =>
{
    return Results.Json(s.Validate(student), statusCode: 200);
});

app.Run();

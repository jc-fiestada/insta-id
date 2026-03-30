using System.Text.Json;


using InstaId.Services.ModelValidator;
using InstaId.Services.Service;
using InstaId.Models.Dto;
using InstaId.Models.Entity;

namespace InstaId.ResponseHandler;

public class Response
{
    public async Task<IResult> GenerateId(HttpRequest request, PlaywrightServices playwright)
    {
        IFormCollection form = await request.ReadFormAsync();

        if (!form.ContainsKey("entity")) return Results.BadRequest("Client data is missing from the request");
        if (form.Files["img"] == null) return Results.BadRequest("Client image is missing from the request");
        if (form.Files["img"]!.Length == 0) return Results.BadRequest("Client image is missing from the request");

        IEntity entity = null!;

        try
        {
            string rawUserData = form["entity"].ToString();
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            EntityDto entityDto = JsonSerializer.Deserialize<EntityDto>(rawUserData, options) ?? throw new Exception();

            if (string.IsNullOrWhiteSpace(entityDto.EntityType)) throw new FormatException("Invalid Entity Model Detected");
            if (entityDto.EntityType != "student" && entityDto.EntityType != "employee") throw new FormatException("Invalid Entity Model Detected");

            if (entityDto.EntityType == "student") entity = new ValidateStudent().Validate(entityDto);
            if (entityDto.EntityType == "employee") entity = new ValidateEmployee().Validate(entityDto);
        } catch (FormatException ex)
        {
            return Results.UnprocessableEntity(ex.Message);
        } catch (Exception)
        {
            Console.WriteLine("----- Student Deserialization Failed -----");
            return Results.InternalServerError("Failed to process user information");
        }

        IFormFile img = form.Files["img"]!;

        byte[] imgBytes = await playwright.GeneratePdfBytes(entity, img);

        return Results.File(imgBytes, "application/pdf", "id-card.pdf");
    }

    
}
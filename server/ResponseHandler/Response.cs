using System.Text.Json;

using InstaId.Services.ModelValidator;
using InstaId.Services.Service;
using InstaId.Models.Dto;
using InstaId.Models.Entity;

namespace InstaId.ResponseHandler;

public class Response
{
    public async Task<IResult> GenerateId(HttpRequest request, PlaywrightServices playwright, Tools tools)
    {
        Console.WriteLine("---- endpoint reached ----");
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
            Console.WriteLine($"ERROR: {ex}");
            return Results.UnprocessableEntity(ex.Message);
        } catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
            return Results.InternalServerError("Failed to process user information");
        }

        IFormFile img = form.Files["img"]!;

        byte[] pdfBytes;

        try
        {
            pdfBytes = await playwright.GeneratePdfBytes(entity, img);
        } catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
            return Results.InternalServerError($"Failed to generate PDF");
        }

        GeneratedIdResponse response = new GeneratedIdResponse()
        {
            PdfStatusCode = 200,
            PdfBase64 = Convert.ToBase64String(pdfBytes),
            GmailStatusCode = 200,
            Message = "Id has been successfully sent through Gmail"
        };

        try
        {
            await tools.SendIdViaGmail(entity, pdfBytes);
        } catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
            response.Message = "Failed to send Id through Gmail";
            response.GmailStatusCode = 500;
            return Results.Ok(response);
        }
        

        return Results.Ok(response);
    }

    
}
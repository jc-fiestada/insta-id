namespace InstaId.Models.Dto;

public class GeneratedIdResponse
{
    public required int PdfStatusCode {get; set;}
    public required string PdfBase64 {get; set;}
    public required int GmailStatusCode {get; set;}
    public required string Message {get; set;}
}
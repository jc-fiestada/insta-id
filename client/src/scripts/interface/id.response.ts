export interface IdResponse{
    pdf
}


public required int PdfStatusCode {get; set;}
    public required string PdfBase64 {get; set;}
    public required int GmailStatusCode {get; set;}
    public required string Message {get; set;}
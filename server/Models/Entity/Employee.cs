namespace InstaId.Models.Entity;

public class Employee : IEntity
{
    public required string Name {get; set;}
    public required string CompanyName {get; set;}
    public required string Gmail {get; set;}
    public required string Position {get; set;}
    public required int Id {get; set;}

    public string Institute => CompanyName;
    public string Role => Position;

}
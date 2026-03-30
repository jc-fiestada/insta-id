namespace InstaId.Models.Entity;

public class Student : IEntity
{
    public required string Name {get; set;}
    public required string SchoolName {get; set;}
    public required string Gmail {get; set;}
    public required string Course {get; set;}
    public required int Id {get; set;}

    public string Institute => SchoolName ;
    public string Role => Course;

}
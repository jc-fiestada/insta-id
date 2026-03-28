using InstaId.Models.Entity;
using InstaId.Models.Dto;
namespace InstaId.Services.ModelValidator;

public class ValidateStudent : ValidateBase <StudentDto, Student>
{
    public override Student Validate(StudentDto Entity)
    {
        ValidateName(Entity.Name);
        ValidateGmail(Entity.Gmail);
        ValidateInstitution(Entity.SchoolName, "Student", "School Name");
        ValidateRole(Entity.Course, "Student", "Course");

        return new Student()
        {
            Name = Entity.Name!,
            SchoolName = Entity.SchoolName!,
            Course = Entity.Course!,
            Gmail = Entity.Gmail!,
        };
    }
}
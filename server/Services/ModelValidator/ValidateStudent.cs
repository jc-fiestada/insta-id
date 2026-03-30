using InstaId.Models.Entity;
using InstaId.Models.Dto;
namespace InstaId.Services.ModelValidator;

public class ValidateStudent : ValidateBase <EntityDto, Student>
{
    public override Student Validate(EntityDto Entity)
    {
        ValidateName(Entity.Name);
        ValidateGmail(Entity.Gmail);
        ValidateInstitution(Entity.Institute, "Student", "School Name");
        ValidateRole(Entity.Role, "Student", "Course");
        ValidateSelectedTemplate(Entity.Id);

        return new Student()
        {
            Name = Entity.Name!,
            SchoolName = Entity.Institute!,
            Course = Entity.Role!,
            Gmail = Entity.Gmail!,
            Id = (int)Entity.Id!
        };
    }
}
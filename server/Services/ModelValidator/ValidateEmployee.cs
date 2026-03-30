using InstaId.Models.Entity;
using InstaId.Models.Dto;
namespace InstaId.Services.ModelValidator;

public class ValidateEmployee : ValidateBase <EntityDto, Employee>
{
    public override Employee Validate(EntityDto Entity)
    {
        ValidateName(Entity.Name);
        ValidateGmail(Entity.Gmail);
        ValidateInstitution(Entity.Institute, "Employee", "Company Name");
        ValidateRole(Entity.Role, "Employee", "Position");
        ValidateSelectedTemplate(Entity.Id);

        return new Employee()
        {
            Name = Entity.Name!,
            CompanyName = Entity.Institute!,
            Position = Entity.Role!,
            Gmail = Entity.Gmail!,
            Id = (int)Entity.Id!
        };
    }
}
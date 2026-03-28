using InstaId.Models.Entity;
using InstaId.Models.Dto;
namespace InstaId.Services.ModelValidator;

public class ValidateEmployee : ValidateBase <EmployeeDto, Employee>
{
    public override Employee Validate(EmployeeDto Entity)
    {
        ValidateName(Entity.Name);
        ValidateGmail(Entity.Gmail);
        ValidateInstitution(Entity.CompanyName, "Employee", "Company Name");
        ValidateRole(Entity.Position, "Employee", "Position");

        return new Employee()
        {
            Name = Entity.Name!,
            CompanyName = Entity.CompanyName!,
            Position = Entity.Position!,
            Gmail = Entity.Gmail!,
        };
    }
}
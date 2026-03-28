using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.RegularExpressions;

namespace InstaId.Services.ModelValidator;

public abstract class ValidateBase<TInput, TOutput> : IValidate<TInput, TOutput> {


    protected void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new FormatException($"Name must not be empty");
        if (name.Length > 20)
            throw new FormatException($"Name must not exceed more than 20 Characters");
        if (!Regex.IsMatch(name, @"^[A-Za-z ]+$"))
            throw new FormatException($"Name must only include letters and spaces");
    }

    protected void ValidateGmail(string? gmail)
    {
        if (string.IsNullOrWhiteSpace(gmail)) 
            throw new FormatException($"Gmail must not be empty");
        
    }

    protected void ValidateInstitution(string? institute, string entityType, string entityAttr)
    {
        if (string.IsNullOrWhiteSpace(institute)) 
            throw new FormatException($"{entityType} must not be empty");
        if (institute.Length > 20)
            throw new FormatException($"{entityType} {entityAttr} must not exceed more than 20 Characters");
        if (!Regex.IsMatch(institute, @"^[A-Za-z ]+$"))
            throw new FormatException($"{entityType} {entityAttr} must only include letters and spaces");
    }

    protected void ValidateRole(string? role, string entityType, string entityAttr)
    {
        if (string.IsNullOrWhiteSpace(role)) 
            throw new FormatException($"{entityType} must not be empty");
        if (role.Length > 20)
            throw new FormatException($"{entityType} {entityAttr} must not exceed more than 15 Characters");
        if (!Regex.IsMatch(role, @"^[A-Za-z ]+$"))
            throw new FormatException($"{entityType} {entityAttr} must only include letters and spaces");
    }

    public abstract TOutput Validate(TInput Entity);
}
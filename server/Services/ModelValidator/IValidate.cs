namespace InstaId.Services.ModelValidator;

public interface IValidate<TInput, TOutput>
{
    TOutput Validate(TInput input);
}
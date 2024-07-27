using Microsoft.AspNetCore.Mvc;

namespace ApiModules.Validation;

public class ExtendedValidationProblem : ProblemDetails
{
    public sealed record Error(string Code, string Message);
    
    public sealed record ValidationError(string Property, Error[] Errors);
    
    public ValidationError[] Errors { get; }
    
    public ExtendedValidationProblem(ValidationError[] errors)
    {
        Title = "One or more validation errors occurred.";
        Status = StatusCodes.Status400BadRequest;
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        Errors = errors;
    }
}
using System.Diagnostics;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;

namespace ApiModules.Validation;

public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationFilterFactory(
        EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {
        var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices).ToList();

        return validationDescriptors.Any()
            ? invocationContext => Validate(validationDescriptors, invocationContext, next)
            : next;
    }

    private static async ValueTask<object?> Validate(
        IEnumerable<ValidationDescriptor> validationDescriptors,
        EndpointFilterInvocationContext invocationContext,
        EndpointFilterDelegate next)
    {
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is null)
                continue;

            var result = await descriptor.Validator.ValidateAsync(new ValidationContext<object>(argument));
            if (result.IsValid)
                continue;

            var problem = BuildProblemDetails(invocationContext, result);

            return TypedResults.Problem(problem);
        }

        return await next.Invoke(invocationContext);
    }

    private static ExtendedValidationProblem BuildProblemDetails(
        EndpointFilterInvocationContext invocationContext,
        ValidationResult validationResult)
    {
        var problem = new ExtendedValidationProblem(validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .Select(x => new ExtendedValidationProblem.ValidationError(x.Key, x
                    .Select(y => new ExtendedValidationProblem.Error(y.ErrorCode, y.ErrorMessage))
                    .ToArray()))
            .ToArray());

        var requestId = Activity.Current?.Id ?? invocationContext.HttpContext.TraceIdentifier;
        problem.Extensions.Add("requestId", requestId);
        problem.Extensions.Add("traceId", invocationContext.HttpContext.TraceIdentifier);

        return problem;
    }

    private static IEnumerable<ValidationDescriptor> GetValidators(
        MethodBase methodBase,
        IServiceProvider serviceProvider)
    {
        var parameters = methodBase.GetParameters();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            if (parameter.GetCustomAttribute<ValidateAttribute>() is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

            if (serviceProvider.GetService(validatorType) is IValidator validator)
                yield return new ValidationDescriptor
                {
                    ArgumentIndex = i,
                    ArgumentType = parameter.ParameterType,
                    Validator = validator
                };
        }
    }

    private class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required Type ArgumentType { get; init; }
        public required IValidator Validator { get; init; }
    }
}
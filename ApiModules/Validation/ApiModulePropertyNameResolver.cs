using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation.Internal;

namespace ApiModules.Validation;

public static class ApiModulePropertyNameResolver
{
    public static string? ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
    {
        var chain = PropertyChain.FromExpression(expression);

        return chain switch
        {
            { Count: 1 } => ToCamelCase(chain.ToString()),
            { Count: > 1 } => chain
                .ToString()
                .Split('.')
                .Skip(1) // remove the first part of the chain, because the front-end doesn't need it
                .Select(ToCamelCase)
                .Aggregate((x, y) => x + "." + y),
            _ => null
        };
        
        string? ToCamelCase(string? text)
        {
            if (string.IsNullOrEmpty(text) || !char.IsUpper(text[0]))
                return text;

            var chars = text.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                    break;

                var hasNext = i + 1 < chars.Length;
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                    break;

                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }
    }
}
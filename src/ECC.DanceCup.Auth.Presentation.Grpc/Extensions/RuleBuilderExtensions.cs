using FluentValidation;

namespace ECC.DanceCup.Auth.Presentation.Grpc.Extensions;

internal static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> IsValidUsername<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать корректное имя пользователя");
    }
    
    public static IRuleBuilderOptions<T, string> IsValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Необходимо передать корректный пароль");
    }
}
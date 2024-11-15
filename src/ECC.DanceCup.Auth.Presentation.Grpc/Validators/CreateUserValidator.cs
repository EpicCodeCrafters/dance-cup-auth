using ECC.DanceCup.Auth.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Auth.Presentation.Grpc.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(request => request.Username).IsValidUsername();
        
        RuleFor(request => request.Password).IsValidPassword();
    }
}
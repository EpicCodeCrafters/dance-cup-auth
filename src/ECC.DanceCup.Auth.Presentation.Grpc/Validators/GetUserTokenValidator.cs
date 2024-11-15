using ECC.DanceCup.Auth.Presentation.Grpc.Extensions;
using FluentValidation;

namespace ECC.DanceCup.Auth.Presentation.Grpc.Validators;

public class GetUserTokenValidator : AbstractValidator<GetUserTokenRequest>
{
    public GetUserTokenValidator()
    {
        RuleFor(request => request.Username).IsValidUsername();
        
        RuleFor(request => request.Password).IsValidPassword();
    }
}
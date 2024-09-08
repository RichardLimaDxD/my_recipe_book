using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword
{
    public class ChangepasswordValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangepasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .SetValidator(new PasswordValidator<RequestChangePasswordJson>());
        }
    }
}

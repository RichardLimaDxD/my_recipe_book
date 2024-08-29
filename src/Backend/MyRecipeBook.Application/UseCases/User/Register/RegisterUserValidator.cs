using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesExeption.NAME_EMPTY);

            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesExeption.EMAIL_EMPTY);

            RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessagesExeption.PASSWORD_EMPTY);

            When(user => user.Email.NotEmpty(), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesExeption.EMAIL_INVALID);
            });
        }
    }
}

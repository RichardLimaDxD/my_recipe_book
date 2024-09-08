using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using MyRepiceBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncripter _passwordEncriter;

        public ChangePasswordUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository repository,
            IUnitOfWork unitOfWork,
            IPasswordEncripter passwordEncriter)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _passwordEncriter = passwordEncriter;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.User();

            Validate(request, loggedUser);

            var user = await _repository.GetById(loggedUser.Id);

            user.Password = _passwordEncriter.Encrypt(request.NewPassword);

            _repository.Update(user);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggeduser)
        {
            var result = new ChangePasswordValidator().Validate(request);

            var currentPasswordEncripted = _passwordEncriter.Encrypt(request.Password);

            if (currentPasswordEncripted.Equals(loggeduser.Password).IsFalse())
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExeption.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

            if (result.IsValid.IsFalse())
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}

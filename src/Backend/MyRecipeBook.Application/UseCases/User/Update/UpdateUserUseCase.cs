using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using MyRepiceBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.User.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserUpdateOnlyRepository _repository;

        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public UpdateUserUseCase(
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IUserUpdateOnlyRepository repository,
            IUserReadOnlyRepository userReadOnlyRepository)
        {
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task Execute(RequestUpdateUserJson request)
        {
            var loggedUser = await _loggedUser.User();

            await Validate(request, loggedUser.Email);

            var user = await _repository.GetById(loggedUser.Id);

            user.Name = request.Name;
            user.Email = request.Email;

            _repository.Update(user);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var validator = new UpdateUserValidator();

            var result = await validator.ValidateAsync(request);

            if (currentEmail.Equals(request.Email).IsFalse())
            {
                var userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

                if (userExist)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesExeption.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();

            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(error => error.ErrorMessages.Count == 1 &&
                error.ErrorMessages.Contains(ResourceMessagesExeption.NAME_EMPTY));

            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Email);

            Func<Task> act = async () => { await useCase.Execute(request); };


            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(error => error.ErrorMessages.Count == 1 &&
                error.ErrorMessages.Contains(ResourceMessagesExeption.EMAIL_ALREADY_REGISTERED));
        }

        private static UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();

            var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

            if (string.IsNullOrEmpty(email).IsFalse())
                userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email!);

            return new UpdateUserUseCase(loggedUser, unitOfWork, userUpdateRepository, userReadOnlyRepositoryBuilder.Build());
        }
    }
}

﻿using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(error => error.ErrorMessages.Count == 1 &&
                error.ErrorMessages.Contains(ResourceMessagesExeption.EMAIL_ALREADY_REGISTERED));
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            request.Name = string.Empty;

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(error => error.ErrorMessages.Count == 1 &&
                error.ErrorMessages.Contains(ResourceMessagesExeption.NAME_EMPTY));
        }

        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();

            if (email.NotEmpty())
                readRepositoryBuilder.ExistActiveUserWithEmail(email);

            return new RegisterUserUseCase(accessTokenGenerator, writeRepository, readRepositoryBuilder.Build(), passwordEncripter, unitOfWork, mapper);
        }
    }
}

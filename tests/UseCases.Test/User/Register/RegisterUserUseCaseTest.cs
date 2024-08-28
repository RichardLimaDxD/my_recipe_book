using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;

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
            result.Name.Should().Be(request.Name);
        }

        private RegisterUserUseCase CreateUseCase()
        {
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var readRepository = new UserReadOnlyRepositoryBuilder().Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();

            return new RegisterUserUseCase(writeRepository, readRepository, passwordEncripter, unitOfWork, mapper);
        }
    }
}

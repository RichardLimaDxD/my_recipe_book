using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterUserUseCase(
            IAccessTokenGenerator accessTokenGenerator,
            IUserWriteOnlyRepository writeOnlyRepository,
            IUserReadOnlyRepository readOnlyRepository,
            IPasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _accessTokenGenerator = accessTokenGenerator;
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponsesRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _passwordEncripter.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _writeOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponsesRegisteredUserJson
            {
                Name = user.Name,

                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            bool emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExeption.EMAIL_ALREADY_REGISTERED));

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

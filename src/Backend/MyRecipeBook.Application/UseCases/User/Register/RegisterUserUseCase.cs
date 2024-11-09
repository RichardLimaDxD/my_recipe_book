using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Token;
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
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;

        public RegisterUserUseCase(
            IAccessTokenGenerator accessTokenGenerator,
            IUserWriteOnlyRepository writeOnlyRepository,
            IUserReadOnlyRepository readOnlyRepository,
            IPasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRefreshTokenGenerator refreshTokenGenerator,
            ITokenRepository tokenRepository)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
        }

        public async Task<ResponsesRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _passwordEncripter.Encrypt(request.Password);

            await _writeOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            var refreshToken = await CreateAndSaveRefreshToken(user);

            return new ResponsesRegisteredUserJson
            {
                Name = user.Name,

                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
                    RefreshToken = refreshToken
                }
            };
        }

        private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
        {
            var refreshToken = _refreshTokenGenerator.Generate();

            await _tokenRepository.SaveNewRefreshToken(new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = user.Id,
            });

            await _unitOfWork.Commit();

            return refreshToken;
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = await validator.ValidateAsync(request);

            bool emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExeption.EMAIL_ALREADY_REGISTERED));

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

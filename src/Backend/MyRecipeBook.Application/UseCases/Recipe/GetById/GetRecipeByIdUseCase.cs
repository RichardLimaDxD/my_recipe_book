﻿using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using MyRepiceBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRepository _repository;
        private readonly IBlobStorageService _blobStorageService;

        public GetRecipeByIdUseCase(
            IMapper mapper,
            ILoggedUser loggedUser,
            IRecipeReadOnlyRepository repository,
            IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _loggedUser = loggedUser;
            _repository = repository;
            _blobStorageService = blobStorageService;
        }

        public async Task<ResponseRecipeJson> Execute(long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _repository.GetById(loggedUser, recipeId);

            if (recipe is null)
                throw new NotFoundException(ResourceMessagesExeption.RECIPE_NOT_FOUND);

            var response = _mapper.Map<ResponseRecipeJson>(recipe);

            if (recipe.ImageIdentifier.NotEmpty())
            {
                var url = await _blobStorageService.GetImageUrl(loggedUser, recipe.ImageIdentifier);

                response.ImageUrl = url;
            }

            return response;
        }
    }
}
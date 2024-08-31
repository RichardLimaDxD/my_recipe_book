using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Login.DtoLogin
{
    public interface IDoLoginUseCase
    {
        public Task<ResponsesRegisteredUserJson> Execute(RequestLoginJson request);
    }
}

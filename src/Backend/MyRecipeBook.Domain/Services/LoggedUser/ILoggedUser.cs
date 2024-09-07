using MyRecipeBook.Domain.Entities;

namespace MyRepiceBook.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}

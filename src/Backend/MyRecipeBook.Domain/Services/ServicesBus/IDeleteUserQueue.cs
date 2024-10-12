using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Services.ServicesBus
{
    public interface IDeleteUserQueue
    {
        public Task SendMessage(User user);
    }
}

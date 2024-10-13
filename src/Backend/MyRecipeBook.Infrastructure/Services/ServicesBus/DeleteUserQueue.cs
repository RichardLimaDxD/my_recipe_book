using Azure.Messaging.ServiceBus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.ServicesBus;

namespace MyRecipeBook.Infrastructure.Services.ServicesBus
{
    public class DeleteUserQueue : IDeleteUserQueue
    {
        private readonly ServiceBusSender _serviceBusSender;

        public DeleteUserQueue(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }

        public async Task SendMessage(User user)
        {
            await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(user.UserIdentifier.ToString()));
        }
    }
}

using Moq;
using MyRecipeBook.Domain.Services.ServicesBus;

namespace CommonTestUtilities.ServiceBus
{
    public class DeleteUserQueueBuilder
    {
        public static IDeleteUserQueue Build()
        {
            return new Mock<IDeleteUserQueue>().Object;
        }
    }
}

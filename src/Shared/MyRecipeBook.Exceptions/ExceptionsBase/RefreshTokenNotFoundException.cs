using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class RefreshTokenNotFoundException : MyRecipeBookException
    {
        public RefreshTokenNotFoundException() : base(ResourceMessagesExeption.EXPIRED_SESSION)
        {
        }
    }
}

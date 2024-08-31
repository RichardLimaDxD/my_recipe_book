namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : MyRecipeBookException
    {
        public InvalidLoginException() : base(ResourceMessagesExeption.EMAIL_OR_PASSWORD_INVALID)
        {
        }
    }
}

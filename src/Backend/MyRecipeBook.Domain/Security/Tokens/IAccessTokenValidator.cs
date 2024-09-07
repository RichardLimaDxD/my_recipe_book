namespace MyRecipeBook.Domain.Security.Tokens
{
    public interface IAccessTokenValidator
    {
        public Guid ValidatedAndGetUserIdentifier(string token);
    }
}

namespace MyRecipeBook.Infrastructure
{
    internal class APIAuthentication
    {
        private string? key;

        public APIAuthentication(string? key)
        {
            this.key = key;
        }
    }
}
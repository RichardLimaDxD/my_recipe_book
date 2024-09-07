using Microsoft.AspNetCore.Mvc;
using MyRepiceBook.API.Filters;

namespace MyRepiceBook.API.Attributes
{
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }
}

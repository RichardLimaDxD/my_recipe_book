using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Domain.Extensions;

namespace MyRepiceBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyRecipeBookBaseController : ControllerBase
    {
        protected static bool IsNotAuthencitacated(AuthenticateResult authenticate)
        {
            return authenticate.Succeeded.IsFalse() ||
                authenticate.Principal is null ||
                authenticate.Principal.Identities.Any(id => id.IsAuthenticated).IsFalse();
        }
    }
}

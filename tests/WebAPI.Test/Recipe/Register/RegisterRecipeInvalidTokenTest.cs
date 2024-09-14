using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using System.Net;

namespace WebAPI.Test.Recipe.Register
{
    public class RegisterRecipeInvalidTokenTest : MyRecipeBookClassFixture
    {

        private const string METHOD = "recipe";


        public RegisterRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var request = new RequestRegisterUserJson();

            var response = await DoPost(method: METHOD, request: request, token: "tokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var request = new RequestRegisterUserJson();

            var response = await DoPost(method: METHOD, request: request, token: string.Empty);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Toke_With_User_NotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var request = new RequestRegisterUserJson();

            var response = await DoPost(method: METHOD, request: request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
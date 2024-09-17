using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Recipe.Register.Filter
{
    public class FilterRecipeTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe/filter";

        private readonly Guid _userIdentifier;

        private string _recipeTitle;
        private MyRecipeBook.Domain.Enums.Difficulty _recipeDifficultyLevel;
        private MyRecipeBook.Domain.Enums.CookingTime _recipeCookingTime;
        private IList<MyRecipeBook.Domain.Enums.DishType> _recipeDishTypes;

        public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();

            _recipeTitle = factory.GetRecipeTitle();
            _recipeCookingTime = factory.GetRecipeCookingTime();
            _recipeDifficultyLevel = factory.GetRecipeDifficulty();
            _recipeDishTypes = factory.GetDishTypes();
        }

        [Fact]
        public async Task Success()
        {

        }

        [Fact]
        public async Task Success_NoContent()
        {

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_CookingTime_Invalid(string culture)
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesExeption.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
        }
    }
}

using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;

namespace Validators.Test.Recipe.Filter
{
    public class FilterRecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {

        }
    }
}

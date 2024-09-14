using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe
{
    public class RecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = (MyRecipeBook.Communication.Enums.CookingTime?)1000;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage
            .Equals(ResourceMessagesExeption.COOKING_TIME_NOT_SUPPORTED));
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Difficulty = (MyRecipeBook.Communication.Enums.Difficulty?)1000;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error
            .ErrorMessage.Equals(ResourceMessagesExeption.DIFFICULTY_LEVEL_NOT_SUPPORTED));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void Error_Empty_Title(string title)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = title;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.RECIPE_TITLE_EMPTY));
        }

        [Fact]
        public void Success_Cooking_Time_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Success_Difficulty_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Success_DishTypes_Empty()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Invalid_DishTypes()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.DISH_TYPE_NOT_SUPPORTED));
        }

        [Fact]
        public void Error_Empty_Ingredients()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.AT_LEAST_ONE_INGREDIENT));
        }

        [Fact]
        public void Error_Empty_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.AT_LEAST_ONE_INSTRUCTION));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void Error_Empty_Value_Ingredients(string ingrediets)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Add(ingrediets);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.INGREDIENT_EMPTY));
        }

        [Fact]
        public void Error_Same_Step_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
        }

        [Fact]
        public void Error_Negative_Step_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = -1;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.NON_NEGATIVE_INSTRUCTION_STEP));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Error_Empty_Value_Instructions(string instruction)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = instruction;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesExeption.INSTRUCTION_EMPTY));
        }

        [Fact]
        public void Error_Instructions_Too_long()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage
            .Equals(ResourceMessagesExeption.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
        }
    }
}

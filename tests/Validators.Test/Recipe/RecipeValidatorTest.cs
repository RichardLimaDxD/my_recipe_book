﻿using CommonTestUtilities.Requests;
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

            var request = RequestRegisterRecipeFormDataBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {
            var validator = new RecipeValidator();

            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.CookingTime = (MyRecipeBook.Communication.Enums.CookingTime?)1000;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage
            .Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            var validator = new RecipeValidator();

            var request = RequestRegisterRecipeFormDataBuilder.Build();

            request.Difficulty = (MyRecipeBook.Communication.Enums.Difficulty?)1000;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error
            .ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void Error_Empty_Title(string title)
        {
            var validator = new RecipeValidator();

            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Title = title;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
        }

        [Fact]
        public void Success_Cooking_Time_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.CookingTime = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Success_Difficulty_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Difficulty = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Success_DishTypes_Empty()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.DishTypes.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Invalid_DishTypes()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
        }

        [Fact]
        public void Error_Empty_Ingredients()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Ingredients.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT));
        }

        [Fact]
        public void Error_Empty_Instructions()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void Error_Empty_Value_Ingredients(string ingrediets)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Ingredients.Add(ingrediets);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
        }

        [Fact]
        public void Error_Same_Step_Instructions()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
        }

        [Fact]
        public void Error_Negative_Step_Instructions()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions.First().Step = -1;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Error_Empty_Value_Instructions(string instruction)
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions.First().Text = instruction;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
        }

        [Fact]
        public void Error_Instructions_Too_long()
        {
            var request = RequestRegisterRecipeFormDataBuilder.Build();
            request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage
            .Equals(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
        }
    }
}

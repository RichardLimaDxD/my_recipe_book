using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate
{
    public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
    {
        public GenerateRecipeValidator()
        {
            var maximum_number_ingredients = MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;

            RuleFor(request => request.Ingredients.Count)
                .InclusiveBetween(1, maximum_number_ingredients)
                .WithMessage(ResourceMessagesExeption.INVALID_NUMBER_INGREDIENTS);

            RuleFor(request => request.Ingredients)
                .Must(ingredients => ingredients.Count == ingredients
                .Select(c => c).Distinct().Count())
                .WithMessage(ResourceMessagesExeption.DUPLICATED_INGREDIENTS_IN_LIST);

            RuleFor(request => request.Ingredients).ForEach(rule =>
            {
                rule.Custom((value, context) =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        context.AddFailure("Ingredient", ResourceMessagesExeption.INGREDIENT_EMPTY);
                        return;
                    }

                    if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 3)
                    {
                        context.AddFailure("Ingredient", ResourceMessagesExeption.INGREDIENT_NOT_FOLLOWING_PATTERN);
                        return;
                    }
                });
            });
        }
    }
}

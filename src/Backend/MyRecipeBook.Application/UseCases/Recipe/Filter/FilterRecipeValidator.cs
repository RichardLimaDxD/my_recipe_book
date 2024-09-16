using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter
{
    public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
    {
        public FilterRecipeValidator()
        {
            RuleForEach(r => r.CookingTimes).IsInEnum().WithMessage(ResourceMessagesExeption.COOKING_TIME_NOT_SUPPORTED);
            RuleForEach(r => r.Difficulties).IsInEnum().WithMessage(ResourceMessagesExeption.DIFFICULTY_LEVEL_NOT_SUPPORTED);
            RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMessagesExeption.DISH_TYPE_NOT_SUPPORTED);
        }
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskFlow.Extensions;

namespace TaskFlow.Filters;

public class ValidationFilters<T> : IAsyncActionFilter where T : class 
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, 
        ActionExecutionDelegate next)
    {
        var dto = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

        if (dto is null)
        {
            throw new InvalidOperationException(
                $"DTO of type {typeof(T).Name} was not found.");
        }
        
        var validator = context.HttpContext.RequestServices
            .GetRequiredService <IValidator<T>>();
        
        var validationResult = await validator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            context.Result = validationResult.ToProblem();

            return;
        }

        await next();
    }
}
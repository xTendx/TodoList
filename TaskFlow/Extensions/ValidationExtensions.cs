using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Extensions;

public static class ValidationExtensions
{
    public static ActionResult ToProblem(this ValidationResult validationResult)
    {
        return new BadRequestObjectResult(new ValidationProblemDetails(validationResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest
        });
    }
}
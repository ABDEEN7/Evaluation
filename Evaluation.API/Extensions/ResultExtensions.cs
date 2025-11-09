using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToValueActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result);
        string message = GetErrorMessage(result);
        return new BadRequestObjectResult(new { Message = message });
    }
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result);
        var message = GetErrorMessage(result);
        return new BadRequestObjectResult(new { Message = message });
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result);
        string message = result.Errors.FirstOrDefault()?.Message ?? "An unknown error occurred.";
        return new BadRequestObjectResult(new { Message = message });
    }
    private static string GetErrorMessage<T>(Result<T> result)
        => result.Errors.FirstOrDefault()?.Message ?? "An unknown error occurred.";

}

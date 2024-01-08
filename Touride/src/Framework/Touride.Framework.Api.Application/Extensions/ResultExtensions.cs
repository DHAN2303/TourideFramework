using Microsoft.AspNetCore.Mvc;
using Touride.Framework.Abstractions.Application.Models;

namespace Touride.Framework.Api.Application.Extensions
{
    /// <summary>
    /// Result extensions for APIs.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Creates an ActionResult from a service Result
        /// </summary>
        /// <returns>The action result.</returns>
        /// <param name="result">Service Result.</param>
        /// <typeparam name="T">The data type of the Result.</typeparam>
        public static ActionResult FromResult<T>(this ControllerBase controller, Result<T> result)
        {
            switch (result.ResultType)
            {
                case ResultType.Ok:
                    if (result.Data == null)
                        return controller.NoContent();
                    else
                        return controller.Ok(result);
                case ResultType.NoContent:
                    return controller.Ok(result);
                case ResultType.InvalidModel:
                    return controller.BadRequest(result);
                case ResultType.BadRequest:
                    return controller.BadRequest();
                case ResultType.NotFound:
                    return controller.NotFound(result.Messages);
                case ResultType.Invalid:
                    return controller.BadRequest(result.Messages);
                case ResultType.Unexpected:
                    {
                        var message = "";
                        result.Messages?.ForEach(p => message += p);
                        throw new Exception(message);
                    }
                case ResultType.Unauthorized:
                    return controller.Forbid();
                default:
                    throw new Exception("An unhandled result has occurred as a result of a service call.");
            }
        }
    }
}

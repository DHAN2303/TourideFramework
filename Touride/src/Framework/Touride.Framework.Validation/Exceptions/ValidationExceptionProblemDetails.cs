using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Touride.Framework.Validation.Exceptions
{
    public class ValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public ValidationExceptionProblemDetails(ValidationException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status400BadRequest;
            this.Detail = JsonSerializer.Serialize(exception.Data);
            this.Type = "validation-error";
        }
    }
}

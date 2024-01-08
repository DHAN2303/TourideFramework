using System.Collections.Generic;

namespace Touride.Framework.Abstractions.Application.Models
{
    /// <summary>
    /// Invalid result.
    /// </summary>
    public class AccessDeniedResult<T> : Result<T>
    {
        public AccessDeniedResult(string error = null)
            : base()
        {
            Messages.Add(error ?? "User is unauthorized.");
        }
        public override ResultType ResultType => ResultType.Unauthorized;
        public override bool Success => false;
        public override List<string> Messages { get; set; }

        public override T Data => default(T);
    }
}

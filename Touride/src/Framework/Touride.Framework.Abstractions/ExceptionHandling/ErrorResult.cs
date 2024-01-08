using System;

namespace Touride.Framework.Abstractions.ExceptionHandling
{
    /// <summary>
    /// Herhangi bir exception durumunda apiden dönecek olan nesne
    /// </summary>

    public class ErrorResult
    {
        public ErrorResult()
        {
            ErrorId = Guid.NewGuid().ToString("N");
        }
        public string ErrorId { get; set; }
        public bool IsCustomException { get; set; }
        public int StatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public bool ShowErrorCode { get; set; }
    }
}

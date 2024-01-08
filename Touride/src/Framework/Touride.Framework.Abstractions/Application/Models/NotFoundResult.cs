namespace Touride.Framework.Abstractions.Application.Models
{
    /// <summary>
    /// Not found result.
    /// </summary>
    public class NotFoundResult<T> : Result<T>
    {
        public NotFoundResult(string error = null) : base()
        {
            Messages.Add(error ?? "The input was invalid.");
        }
        public override ResultType ResultType => ResultType.NotFound;
        public override bool Success => false;
        public override List<string> Messages { get; set; }

        public override T Data => default(T);
    }
}

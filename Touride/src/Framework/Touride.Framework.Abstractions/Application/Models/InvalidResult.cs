namespace Touride.Framework.Abstractions.Application.Models
{
    /// <summary>
    /// Invalid result.
    /// </summary>
    public class InvalidResult<T> : Result<T>
    {
        public InvalidResult(string error)
            : base()
        {
            Messages.Add(error ?? "The input was invalid.");
        }
        public override ResultType ResultType => ResultType.Invalid;

        public override bool Success => false;
        public override List<string> Messages { get; set; }

        public override T Data => default(T);
    }
}

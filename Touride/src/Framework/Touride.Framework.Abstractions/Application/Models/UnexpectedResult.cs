namespace Touride.Framework.Abstractions.Application.Models
{
    /// <summary>
    /// Unexpected result.
    /// </summary>
    public class UnexpectedResult<T> : Result<T>
    {
        public UnexpectedResult(string error = null) : base()
        {
            Messages.Add(error ?? "There was an unexpected problem");
        }
        public UnexpectedResult()
        {

        }
        public override ResultType ResultType => ResultType.Unexpected;
        public override bool Success => false;
        public override List<string> Messages { get; set; }

        public override T Data => default(T);
    }
}

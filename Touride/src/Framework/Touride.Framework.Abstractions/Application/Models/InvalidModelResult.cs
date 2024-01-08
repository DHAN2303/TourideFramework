namespace Touride.Framework.Abstractions.Application.Models
{
    /// <summary>
    /// Invalid result.
    /// </summary>
    public class InvalidModelResult<T> : Result<T>
    {
        public InvalidModelResult(string error)
            : base()
        {
            Messages.Add(error ?? "Model sent is wrong.");
        }
        public override ResultType ResultType => ResultType.InvalidModel;

        public override bool Success => false;
        public override List<string> Messages { get; set; }

        public override T Data => default(T);
    }
}

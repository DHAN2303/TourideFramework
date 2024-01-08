namespace Touride.Framework.Abstractions.Application.Models
{
    public class BadRequestResult<T> : Result<T>
    {
        public BadRequestResult(string error = null)
            : base()
        {
            Messages.Add(error ?? "The input was invalid.");
        }
        public override ResultType ResultType => ResultType.BadRequest;

        public override bool Success => false;
        public override List<string> Messages { get; set; }

        public override T Data => default(T);
    }
}
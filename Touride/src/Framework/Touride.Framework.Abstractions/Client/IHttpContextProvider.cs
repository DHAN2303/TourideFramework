namespace Touride.Framework.Abstractions.Client
{
    public interface IHttpContextProvider
    {
        string? CorrelationId { get; }
        string? CorrelationSeq { get; }
        string? Token { get; }
        string? UserId { get; }

    }
}
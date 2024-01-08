namespace Touride.Framework.Abstractions.Auth
{
    public interface IAuthorityAttribute
    {
        string[] Action { get; set; }
    }
}

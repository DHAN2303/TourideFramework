namespace Touride.Framework.Abstractions.Secrets
{
    public interface IVaultProvider
    {
        string GetValue(string mySecret);
    }
}

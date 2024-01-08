using System.ComponentModel;
using System.Threading.Tasks;

namespace Touride.Framework.Client.Abstractions
{
    public interface ITokenProvider
    {
        Task<string> GetToken(TokenType type);
    }

    public interface IIdentityFlowProvider : ITokenProvider
    {
        //Task<string> GetClientCredentialToken();
    }

    public enum TokenType
    {
        [Description("access_token")]
        AccessToken,
        [Description("id_token")]
        IdToken,
        [Description("refresh_token")]
        RefreshToken
    }
}

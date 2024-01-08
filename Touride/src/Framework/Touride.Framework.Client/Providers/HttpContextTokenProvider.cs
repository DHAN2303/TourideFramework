using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Touride.Framework.Client.Abstractions;
using Touride.Framework.Client.Extensions;

namespace Touride.Framework.Client.Providers
{
    public class HttpContextTokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GetToken(TokenType type)
        {


            if (_httpContextAccessor.HttpContext == null)
            {
                return null;
            }
            try
            {
                var openIdConnect = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectDefaults.AuthenticationScheme, type.ToDescription());
                return openIdConnect == null ? null : openIdConnect.ToString();
            }
            catch (Exception ex)
            {

                var token = await _httpContextAccessor.HttpContext.GetTokenAsync(type.ToDescription());

                return token == null ? ex.Message : token.ToString();

            }
        }
    }
}
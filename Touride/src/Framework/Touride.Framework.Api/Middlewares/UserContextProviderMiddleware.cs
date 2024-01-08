using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Touride.Framework.Abstractions.Client;

namespace Touride.Framework.Api.Middlewares
{
    public class UserContextProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationId = "Correlation-Id";
        private const string CorrelationSeq = "Correlation-Seq";
        // Dependency Injection
        public UserContextProviderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserContextProvider userContextProvider)
        {
            SetClientInfo(context, userContextProvider);
            await _next(context);
        }

        private void SetClientInfo(HttpContext context, IUserContextProvider userContextProvider)
        {
            string authHeader = context.Request.Headers["Authorization"];
            string correlationId = null;
            string correlationSeq = null;


            if (context.Request.Headers.TryGetValue(CorrelationId, out StringValues correlationIds))
                correlationId = correlationIds.FirstOrDefault();


            if (context.Request.Headers.TryGetValue(CorrelationSeq, out StringValues correlationSeqs))
                correlationSeq = correlationSeqs.FirstOrDefault();

            userContextProvider.CorrelationId = correlationId;
            userContextProvider.CorrelationSeq = correlationSeq;

            if (authHeader != null && authHeader.Split(" ")[0] != "Basic")
            {
                string headerTokenInfo = authHeader.Replace("Bearer ", "");

                if (headerTokenInfo == authHeader) headerTokenInfo = authHeader.Replace("bearer ", "");

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(headerTokenInfo) as JwtSecurityToken;
                if (token != null)
                {

                    userContextProvider.ClientId = token.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.PreferredUserName)?.Value;
                    userContextProvider.ClientName = token.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Name)?.Value;
                    userContextProvider.Email = token.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Email)?.Value;
                    userContextProvider.UserCode = token.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                    userContextProvider.UserId = token.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                    userContextProvider.ClientIp = token.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                    userContextProvider.FirstName = token.Claims.FirstOrDefault(claim => claim.Type == "firstname")?.Value;
                    userContextProvider.LastName = token.Claims.FirstOrDefault(claim => claim.Type == "lastname")?.Value;
                    userContextProvider.UserName = token.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;
                    userContextProvider.DepartmentNo = token.Claims.FirstOrDefault(claim => claim.Type == "departmentNo")?.Value;
                    userContextProvider.ErpUserId = token.Claims.FirstOrDefault(claim => claim.Type == "userid")?.Value;
                    userContextProvider.CountryCode = token.Claims.FirstOrDefault(claim => claim.Type == "countryCode")?.Value;


                }
            }

            userContextProvider.ClientIp = context.Connection.RemoteIpAddress?.ToString();
            var emptyGuid = Guid.Empty;
            userContextProvider.ClientId = userContextProvider.ClientId == null ? emptyGuid.ToString() : userContextProvider.ClientId;
            //Pass to the next middleware
        }
    }
}

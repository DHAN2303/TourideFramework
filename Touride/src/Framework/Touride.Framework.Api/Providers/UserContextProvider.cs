using Touride.Framework.Abstractions.Client;

namespace Touride.Framework.Api.Providers
{
    /// <summary>
    /// Backend(Api) tarafında token içindeki oturum bilgilerine erişim için kullanılacak
    /// </summary>
    public class ClientInfoProvider : IUserContextProvider
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientIp { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserCode { get; set; }
        public string CityCode { get; set; }
        public string DistrictCode { get; set; }
        public string CorrelationId { get; set; }
        public string CorrelationSeq { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ErpUserId { get; set; }
        public string DepartmentNo { get; set; }
        public string CountryCode { get; set; }
    }
}

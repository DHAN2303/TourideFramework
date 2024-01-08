namespace Touride.Framework.Abstractions.Client
{
    /// <summary>
    /// Backend(Api) ve Frontend(Mvc) tarafında token içindeki oturum bilgilerine erişim için kullanılacak
    /// </summary>
    public interface IUserContextProvider
    {
        /// <summary>
        /// Client Id bilgisi
        /// </summary>
        string ClientId { get; set; }
        /// <summary>
        /// Client Name bilgisi
        /// </summary>
        string ClientName { get; set; }
        /// <summary>
        /// Client Ip bilgisi
        /// </summary>
        /// 
        string ClientIp { get; set; }
        /// <summary>
        /// User Id Bilgisi
        /// </summary>
        string UserId { get; set; }
        /// <summary>
        /// User Code bilgisi
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// User Code bilgisi
        /// </summary>
        string UserCode { get; set; }

        /// <summary>
        /// İl Kodu
        /// </summary>
        string CityCode { get; set; }

        /// <summary>
        /// İlçe Kodu
        /// </summary>
        string DistrictCode { get; set; }

        string CorrelationId { get; set; }

        string CorrelationSeq { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string UserName { get; set; }
        string ErpUserId { get; set; }
        string DepartmentNo { get; set; }
        string CountryCode { get; set; }

    }
}

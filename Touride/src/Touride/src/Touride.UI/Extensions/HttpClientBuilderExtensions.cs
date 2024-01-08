namespace Touride.UI.Extensions
{
    public static class HttpClientBuilderExtensions
    {

        public static IHttpClientBuilder AddPanelProperties(this IHttpClientBuilder builder)
        {
            builder.ConfigureHttpClient(c =>
            {
                c.DefaultRequestHeaders.Add("request-source", "admin-panel");
            });

            return builder;
        }
    }
}
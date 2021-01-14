namespace Zaggoware.Games.Web.Extensions
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {

        public static bool EnableSwagger(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("Swagger:Enable");
        }

        public static bool RequireHttps(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("Security:RequireHttps");
        }

        public static string SpaUrl(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("Spa:Url");
        }
    }
}
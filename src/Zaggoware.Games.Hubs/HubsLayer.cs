namespace Zaggoware.Games.Hubs
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Zaggoware.Games.Common;

    [ServiceConfiguration]
    public static class HubsLayer
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
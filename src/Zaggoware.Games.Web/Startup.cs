namespace Zaggoware.Games.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using Zaggoware.Games.Common;
    using Zaggoware.Games.CrazyEights;
    using Zaggoware.Games.Hubs;
    using Zaggoware.Games.Web.Extensions;
    using Zaggoware.Games.Web.Hubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration.EnableSwagger())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zaggoware.Games.Web v1"));
            }

            if (Configuration.RequireHttps())
            {
                app.UseHttpsRedirection();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<CrazyEightsGameHub>("/hubs/games/crazy-eights");
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });

            app.UseSpa(builder =>
            {
                builder.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    builder.UseProxyToSpaDevelopmentServer(Configuration.SpaUrl());
                }
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "ClientApp/dist";
            });
            services.AddMemoryCache();
            services.AddControllers()
                .AddJsonOptions(SetupJson);
            services.AddSignalR();

            if (Configuration.EnableSwagger())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zaggoware.Games.Web", Version = "v1" });
                });
            }

            GameTypeRegistrar.Register<CrazyEightsGame>();

            HubsLayer.ConfigureServices(services, Configuration);
        }

        private void SetupJson(JsonOptions options)
        {
        }
    }
}
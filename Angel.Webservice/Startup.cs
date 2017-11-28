namespace Angel.Webservice
{
    using Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using NLog.Extensions.Logging;
    using NLog.Web;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string key = Configuration.GetSection("Token").GetValue<string>("Key");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = "Angel",
                        ValidateIssuer = true,
                        ValidAudience = "Angel",
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = JwtSecurityKey.Create(key)
                    };
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}

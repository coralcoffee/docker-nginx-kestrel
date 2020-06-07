using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://ubuntu-dev.southcentralus.cloudapp.azure.com/ident/";
                    //options.RequireHttpsMetadata = false;
                    //options.CallbackPath = new PathString("/sample/signin-oidc");
                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    options.SaveTokens = true;

                    options.Scope.Add("api1");
                    options.Scope.Add("offline_access");
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.Map("/sample", builder =>
            {
                builder.UseForwardedHeaders(fordwardedHeaderOptions);
                if (env.IsDevelopment())
                {
                    builder.UseDeveloperExceptionPage();
                }
                else
                {
                    builder.UseExceptionHandler("/Home/Error");
                }

                builder.UseStaticFiles();


                builder.UseAuthentication();
                builder.UseRouting();
                builder.UseAuthorization();


                builder.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                    .RequireAuthorization();
            });
            });
        }
    }
}

using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Infrastructure.API;
using ExpenseTracker.Infrastructure.API.Authorization.Handlers;
using ExpenseTracker.Infrastructure.API.Extensions;
using ExpenseTracker.Infrastructure.IdentityServer.Extensions;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using System.Collections.Generic;
using System.Net.Http;

namespace ExpenseTracker.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ExpenseTrackerDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("ExpenseTrackerConnection"));
                options.UseLazyLoadingProxies();
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001/";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "app.expensetracker.api";
                });
            services.AddAuthorization(options =>
            {
                options.AddAuthorizationPolicies();
            });

            services.AddScoped(typeof(IEFRepository<>), typeof(EFRepository<>));
            services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
            services.AddSingleton<IDiscoveryCache>(s =>
            {
                var factory = s.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(Configuration["ApiResourceBaseUrls:AuthServer"], () => factory.CreateClient());
            });

            services.AddControllers(options =>
            {
                options.AddAuthorizationFilters();
            });

            services.AddOpenApiDocument(options =>
            {
                options.DocumentName = "v1";
                options.Title = "Expense Tracker API";
                options.Version = "v1";

                options.AddSecurity("oauth2", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = "https://localhost:5001/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                {"app.expensetracker.api.full", "expense tracker web api - full access"},
                                {"app.expensetracker.api.write", "expense tracker web api - only write access"},
                                {"app.expensetracker.api.read", "expense tracker web api - only read access"},
                            }
                        }
                    }
                });

                options.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3(options =>
                {
                    options.OAuth2Client = new OAuth2ClientSettings
                    {
                        ClientId = Configuration["Swagger:Id"],
                        ClientSecret = Configuration["Swagger:Secret"],
                        AppName = "Expense Tracker Web Api",
                        UsePkceWithAuthorizationCodeGrant = true,
                    };
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Infrastructure.Repository.API;
using ExpenseTracker.Infrastructure.Repository.API.Authorization.Handlers;
using ExpenseTracker.Infrastructure.Repository.API.Extensions;
using ExpenseTracker.Infrastructure.Repository.IdentityServer.Extensions;
using ExpenseTracker.Infrastructure.Repository.Shared;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using ExpenseTracker.Core.Application;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Application.QueryableBuilders;
using ExpenseTracker.Core.Application.QueryHandlers.Expenses;
using ExpenseTracker.Core.Domain.AutoMapperProfiles;
using ExpenseTracker.Infrastructure.Repository.Shared.SignalR;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

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

            services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)))
                //.AddScoped<IEmailSettings, EmailSettings>();
                .AddScoped<IEmailSettings>(sp =>
                sp.GetRequiredService<IOptionsMonitor<EmailSettings>>().CurrentValue);

            services.AddHttpContextAccessor();
            services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(ExpenseProfile).GetTypeInfo().Assembly,
                typeof(Infrastructure.API.AutoMapperProfiles.ExpenseProfile).GetTypeInfo().Assembly);
            services.AddScoped<ISendingManager, SendingManager>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ExpensesBuilder>();
            services.AddSingleton<ITagReplacer, MailTagReplacer>();
            services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            //services.AddSingleton<TelemetryClient>();
            services.AddSingleton<IDiscoveryCache>(s =>
            {
                var factory = s.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(Configuration["ApiResourceBaseUrls:AuthServer"], () => factory.CreateClient());
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSignalR();
            
            services.AddMediatR(typeof(GetUserExpensesQueryHandler),typeof(GetUserExpensesQuery));

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

            //app.AddExceptionHandlerMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChartHub>("/chartHub");
            });
        }
    }
}

using ExpenseTracker.Core.Domain.Auth;
using ExpenseTracker.Core.Domain.Schemas;
using ExpenseTracker.Infrastructure.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Validation;
using System.Reflection;

namespace ExpenseTracker.Web.IdentityServer
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
            services.AddDbContext<IDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("ExpenseTrackerConnection"));
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<IDbContext>()
                .AddDefaultTokenProviders();

            var migrationsAssembly = typeof(IDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(builder =>
                {
                    builder.ConfigureDbContext = options => options.UseSqlServer(
                        Configuration.GetConnectionString("ExpenseTrackerConnection"),
                        opt => opt.MigrationsAssembly(migrationsAssembly)
                        );
                    builder.DefaultSchema = IdentitySchemas.IdentityServer;
                })
                .AddOperationalStore(builder =>
                {
                    builder.ConfigureDbContext = options => options.UseSqlServer(
                        Configuration.GetConnectionString("ExpenseTrackerConnection"),
                        opt => opt.MigrationsAssembly(migrationsAssembly)
                        );
                    builder.DefaultSchema = IdentitySchemas.IdentityServer;
                })
                .AddProfileService<ProfileService>()
                .AddResourceOwnerValidator<ExpenseTrackerResourceOwnerPasswordValidator>();

            //Our API server
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://localhost:5002")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("default");
            app.UseRouting();

            app.UseIdentityServer();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Hangfire;
using LeadwaycanteenApi.util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LeadwaycanteenApi
{
    /// <summary>
    /// Class for configuring the application
    /// </summary>
    public static class ExtentionClass
    {
        /// <summary>
        /// Configures JWT authentication for the application by setting up the JWT bearer authentication scheme
        /// with appropriate token validation parameters using settings from the configuration.
        /// </summary>
        /// <param name="service">The <see cref="IServiceCollection"/> to configure services.</param>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to access the application configuration.</param>
        public static void ConfigureJWTAuth(this IServiceCollection service, WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetSection("jwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? string.Empty);

            service.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        /// <summary>
        /// Configures Hangfire services for background job processing using SQL Server for storage.
        /// </summary>
        /// <param name="service">The <see cref="IServiceCollection"/> to configure services.</param>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to access the application configuration.</param>
        public static void ConfigureHangFire(this IServiceCollection service, WebApplicationBuilder builder)
        {
            service.AddHangfire(opt =>
            {
                opt.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new Hangfire.SqlServer.SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                })
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();
            });

            service.AddHangfireServer(opt =>
            {
                opt.WorkerCount = 30;
                opt.ServerName = "Leadwaycanteenapp";
            });
        }

        /// <summary>
        /// Registers services required for HTTP client communication and validation utility.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to configure services.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with validation services added.</returns>
        public static IServiceCollection AddValidationUtility(this IServiceCollection services)
        {
            services.AddHttpClient("LeadwayAuth", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddScoped<ValidationUtility>();
            return services;
        }

        /// <summary>
        /// Configures Cross-Origin Resource Sharing (CORS) policy to allow any origin, method, and header.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to configure services.</param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAll", builder =>
                {
                    builder
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true);
                });
            });
        }
    }

}

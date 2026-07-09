using crudSignalR.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MontessoriSystem.Core.Domain.Settings;
using MontessoriSystem.Infrastructure.Identity.Contexts;
using MontessoriSystem.Infrastructure.Identity.Entities;
using MontessoriSystem.Infrastructure.Identity.Services;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;


namespace MontessoriSystem.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.EnableSensitiveDataLogging();

                string connectionString = Environment.GetEnvironmentVariable("IdentityConnection")?.Trim() ?? "";
                

                // Si la variable de entorno no está definida, usa la de appsettings.json
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration.GetConnectionString("IdentityConnection");
                }

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), 
                    mysqlOptions =>
                    {
                        mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                        mysqlOptions.MaxBatchSize(1);
                    }
                );

                Console.WriteLine($"Connection String: {connectionString}"); // Para depuración

            });

            #region Idenity

            services.AddIdentity<ApplicationUser,IdentityRole> ()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            services.AddTransient<IAccountService, AccountService>();

            #endregion
        }

        public static void AddIdentityInfrastructureWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context DB

            services.AddDbContext<IdentityContext>(options =>
            {
                //options.EnableSensitiveDataLogging();                

                string connectionString = Environment.GetEnvironmentVariable("IdentityConnection")?.Trim() ?? "";

                // Si la variable de entorno no está definida, usa la de appsettings.json
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }
                try
                {
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), 
                        mysqlOptions => 
                        {
                            mysqlOptions.MaxBatchSize(1);
                        });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                    throw;
                }

            });

            #endregion

            #region Identity

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                #region Security

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,  
                    ValidateAudience = true, 
                    ValidateLifetime = true,  
                    ValidateIssuerSigningKey = true,
                    
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],  
                    ValidAudience = configuration["JWTSettings:Audience"],  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"])) 
                };

                #endregion

                #region Configuración para manejar eventos

                option.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new { HasError = true, Error = "Usted no está autorizado para usar esta app, inicie sesión y coloque su token" });
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new { HasError = true, Error = "Usted no está autorizado para usar este recurso de la api" });
                        return c.Response.WriteAsync(result);
                    }
                };

                #endregion

            });

            services.AddTransient<IAccountService, AccountService>();
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            #endregion

        }
    }
}

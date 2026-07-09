using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MontessoriSystem.Infrastructure.Persistence.Contexts;

namespace MontessoriSystem.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructureSegurity(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context

            services.AddDbContext<SecurityContext>(options =>
            {
                string connectionString = Environment.GetEnvironmentVariable("SecurityConnection")?.Trim() ?? "";

                // Si la variable de entorno no está definida, usa la de appsettings.json
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }
                try
                {
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                    throw;
                }

            });

            #endregion

            #region Dependency inyeccion


            #endregion
        }
    }

}

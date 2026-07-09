using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SmartCampus.Extentions
{
    public static class SwaggerServiceExtensions
    {

        #region Configuración de Swagger para la API, documentación, Metodo de Autorizacion
        public static void AddSuaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MontessorieSystem API",
                    Version = "v1",
                    Description = "Esta es mi API para trabajar el sistema de MontessorieSystem",
                    Contact = new OpenApiContact
                    {
                        Name = "Alexander Polanco Moreno",
                        Email = "alexanderrpolanco11@gmail.com",
                        Url = new Uri("https://my-portfolio-app-pink.vercel.app/")
                    }
                });
                
                c.EnableAnnotations();
                c.DescribeAllParametersInCamelCase();

                #region Seguridad By Bearer Token

                // Definición de esquema de seguridad
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    In = ParameterLocation.Header, // Dónde se encuentra el token
                    Description = "Por favor, ingrese el token de autenticación en formato 'Bearer {token}'",
                    Name = "Authorization", // Nombre del encabezado
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",

                });

                // Añadir el esquema de seguridad a las operaciones
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" // El ID debe coincidir con el esquema definido
                            },
                            Scheme="Bearer",
                            Name="Bearer",
                            In = ParameterLocation.Header,
                        },
                        new string[] {}
                    }
                });

                #endregion


            });
        }

        #endregion

        #region Swagger Versioning
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader(); // Asegúrate de que esto esté configurado

            });
        }
        #endregion

        #region Swagger Configuration
        public static void UseSwaggerDocumentation(this IApplicationBuilder app)
        {
           
            app.UseSwagger(); // Habilita el middleware de Swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1"); 
                c.DefaultModelRendering(ModelRendering.Model); // Establece la representación del modelo
            });
            
        }

        #endregion

    }
}

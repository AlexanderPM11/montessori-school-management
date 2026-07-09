using Microsoft.AspNetCore.Mvc.Filters;
using montessoriSystem.Controllers;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using RealtyApp.Presentation.WebApp.Middlewares;

namespace montessoriSystem.Middlewares
{

    public class ValidateInitDataCompletedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IInitDataService _initDataService;

        public ValidateInitDataCompletedMiddleware(RequestDelegate next, IInitDataService initDataService)
        {
            _next = next;
            _initDataService = initDataService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var initData = await _initDataService.GetAllViewModel();
            var unreadyScreens = initData.FirstOrDefault(x => x.Ready == false);

            if (unreadyScreens != null)
            {
                // Redirige según la pantalla que falte completar
                string redirectUrl = unreadyScreens.Description switch
                {
                    "EducationalCenter" => "/YourController/FillEducationalCenter",
                    "Teachers" => "/YourController/FillTeachers",
                    "StudentsAndFather" => "/YourController/FillStudentsAndFather",
                    "Room" => "/YourController/FillRoom",
                    _ => "/YourController/AllScreensReady"
                };

                context.Response.Redirect(redirectUrl);
                return; // Detén la ejecución del middleware
            }

            // Continúa con el siguiente middleware si todas las pantallas están listas
            await _next(context);
        }
    }

}

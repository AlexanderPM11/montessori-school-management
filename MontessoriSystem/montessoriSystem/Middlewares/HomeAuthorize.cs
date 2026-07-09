using Microsoft.AspNetCore.Mvc.Filters;
using MontessoriSystem.Core.Application.Enums;
using montessoriSystem.Controllers;

namespace RealtyApp.Presentation.WebApp.Middlewares
{
    public class HomeAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;

        public HomeAuthorize(ValidateUserSession userSession)
        {
            _userSession = userSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = _userSession.HasUser();
            if (user != null)
            {
                var controller = (StudentController)context.Controller;
                if (user.Roles.Any(n => n == Roles.Admin.ToString()))
                {
                    context.Result = controller.RedirectToAction("index", "Student");
                }
                if (user.Roles.Any(n => n == Roles.SuperAdmin.ToString()))
                {
                    context.Result = controller.RedirectToAction("index", "Student");
                }
                if (user.Roles.Any(n => n == Roles.Basico.ToString()))
                {
                    context.Result = controller.RedirectToAction("index", "Student");
                }
            }
            else
            {
                await next();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using montessoriSystem.Controllers;
using MontessoriSystem.Core.Application.Enums;

namespace RealtyApp.Presentation.WebApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginAuthorize(ValidateUserSession userSession, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = _userSession.HasUser();
            if (user != null)
            {
                var controller = (StudentController)context.Controller;
                if (user.Roles.Count == 1)
                {
                    if (user.Roles.Any(n => n == Roles.Profesor.ToString()))
                    {
                        context.Result = controller.RedirectToAction("Index", "Student");
                    }
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

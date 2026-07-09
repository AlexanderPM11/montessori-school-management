using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Services;
using RealtyApp.Core.Application.Helpers;

namespace RealtyApp.Presentation.WebApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public LoginResponse? HasUser()
        {
            LoginResponse userViewModel = _httpContextAccessor.HttpContext.Session.GetString<LoginResponse>("user");

            if (userViewModel == null)
            {
                return null;
            }
            return userViewModel;
        }

    }
}

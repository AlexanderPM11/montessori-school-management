using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Helpers
{
    public class UserFilter
    {
        public static void SetCookie(HttpContext httpContext, string key, string value)
        {
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(50) 
            };

            httpContext.Response.Cookies.Append(key, value, option);
        }

        public static string GetCookie(HttpContext httpContext, string key)
        {
            return httpContext.Request.Cookies[key];
        }

        public static void RemoveCookie(HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }


    }
}

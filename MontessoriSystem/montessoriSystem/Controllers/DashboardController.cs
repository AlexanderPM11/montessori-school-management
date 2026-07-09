using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using RealtyApp.Core.Application.Helpers;

namespace montessoriSystem.Controllers
{

    public class DashboardController : Controller
    {
        private readonly IStudentServices _studentServices;
        private readonly IUserService _userService;
        private readonly IInitDataHelper _initDataHelper;


        public DashboardController(IStudentServices studentServices, IUserService userService, IInitDataHelper initDataHelper)
        {
            _studentServices = studentServices;
            _userService = userService;
            _initDataHelper = initDataHelper;
        }

        [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
        public  async Task<IActionResult> Index()
       {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return RedirectToAction(action, controller);
            }

            HttpContext.Session.SetString<string>("currentPage", "Dashboard");
            var userName = User.Identity.Name;

            var user = await _userService.GetUserByName(userName);
            if (user.Roles.Count == 1)
            {
                if (user.Roles.Any(n => n == Roles.Profesor.ToString()))
                {
                    return RedirectToAction("Index", "Room");
                }
            }
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
        [HttpGet]
        public async Task<JsonResult> StudentCharts()
        {
            // Obtener todos los estudiantes desde el servicio
            var allStudents = await _studentServices.GetAllViewModel();

            var studentChartData = allStudents.Select(student => new
            {
                Nombre = student.Name,
                Sexo = student.Sexo , 
                Edad = student.Age < 0 ? 0: student.Age
            }).ToList();

            // Enviar el modelo a la vista
            return Json(studentChartData);
        }
    }

}

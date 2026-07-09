using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.User;
using Newtonsoft.Json;
using RealtyApp.Core.Application.Helpers;
using System.Net;

namespace montessoriSystem.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ISelectionService _selectionService;
        private readonly IUserService _userService;
        private readonly IEducationalInstitutionService _educationalInstitutionServices;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IInitDataHelper _initDataHelper;


        public UserController(IUserService userService, IUserManagementService userManagementService,
            IEducationalInstitutionService educationalInstitutionServices,
            IValidateInfoDataBase validateInfoDataBase,ISelectionService selectionService, IInitDataHelper initDataHelper)
        {
            _userService = userService;
            _educationalInstitutionServices = educationalInstitutionServices;
            _validateInfoDataBase = validateInfoDataBase;
            _userManagementService = userManagementService;
            _selectionService = selectionService;
            _initDataHelper = initDataHelper;
        }

        public async Task<IActionResult> Login()
        {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return RedirectToAction(action, controller);
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            GeneralResponse<LoginResponse> userVm = await _userService.AuthenticateAsync(vm);

            if (userVm != null && userVm.result)
            {
                HttpContext.Session.SetString<LoginResponse>("user", userVm.Data);
                return RedirectToRoute( new { controller = "Dashboard", action = "Index" });
            }
            else
            {
                vm.HasError = true;
                vm.Error = userVm.messages.FirstOrDefault();
                return View(vm);
            }
        }
        
        [Authorize(Roles = "SuperAdmin,Admin")]
        public  async Task<IActionResult> Index()
        {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return RedirectToAction(action, controller);
            }
            HttpContext.Session.SetString<string>("currentPage", "Usuarios");
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<PartialViewResult> PartialViewUsers(int? idInstitu, int? institutionIdPrincipal)
        {
            //if(idInstitu == null || idInstitu <= 0)
            //{
            //    return PartialView();
            //}

            //var userName = User.Identity.Name;
            //var user = await _userService.GetUserByName(userName);

            ////Validar idInstitu antes de guardar
            //bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(idInstitu ?? 0, user);

            //if (!isAuthorized)
            //{
            //    Response.StatusCode = (int)HttpStatusCode.Forbidden;
            //    return PartialView("");
            //}

            ViewBag.IdInstitu = idInstitu;
            ViewBag.InstitutionIdPrincipal = institutionIdPrincipal;

            return PartialView("Index");
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<JsonResult> GetDatosUserJson()
        {
            try
            {              
                var userName = User.Identity.Name;
                var user = await _userService.GetUserByName(userName);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { message = "Usuarios no encontrados" });
                }              
                
                var filterUser =  UserFilter.GetCookie(HttpContext, "filters-users");  
                List<string> selectedOptions = JsonConvert.DeserializeObject<List<string>>(filterUser ?? "[]");

                var response = await _userManagementService.GetUsersAsync(selectedOptions);

                return Json(response.data);
            }

            catch (Exception ex)
            {
                return Json("");
            }
        }
       
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<PartialViewResult> Filtro()
        {
            try
            {
                var userName = User.Identity.Name;
                var user = await _userService.GetUserByName(userName);
                var filterUser = UserFilter.GetCookie(HttpContext, "filters-users");
                if(filterUser == null || filterUser == "[]")
                {
                    UserFilter.SetCookie(HttpContext, "filters-users", "[\"Todos\"]");
                }

                var roles = await _userService.GetAllRoles(user.Roles);
                ViewBag.Roles =  roles;
                ViewBag.SelectedOptions = filterUser ?? "[\"Todos\"]";

                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView("");
            }

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<JsonResult> SetFiltro( string filters)
        {
            try
            {
                var userName = User.Identity.Name;
                var user = await _userService.GetUserByName(userName);
                UserFilter.SetCookie(HttpContext, "filters-users", filters);

                var roles = await _userService.GetAllRoles(user.Roles);
                ViewBag.Roles =  roles;

                return Json("");
            }
            catch (Exception ex)
            {
                return Json("");
            }

        }
        
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult ListUser()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<PartialViewResult> CreateOrUpdate(string? idUser)
        {
            var userName = User.Identity.Name;
            var user = await _userService.GetUserByName(userName);

            if(!user.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView("");
            }           

            var roles = await _userService.GetAllRoles(user.Roles);

            var titlesTask = _selectionService.GetTitlesAchieved();
            var specializationsTask = _selectionService.GetSpecializations();
            var educationalLevelsTask = _selectionService.GetEducationalLevel();
            var professionsTask = _selectionService.GetProfessions();
            var maritalStatusTask = _selectionService.GetCivilStatus();
            var nationalityTask = _selectionService.GetNationality();
            var relationshipTask = _selectionService.GetRelationship();

            // Ejecutamos todas las tareas en paralelo
            await Task.WhenAll(titlesTask, specializationsTask, educationalLevelsTask, professionsTask, maritalStatusTask, nationalityTask, relationshipTask);

            // Asignamos los resultados cuando todas las tareas han finalizado
            var targettitlesAchieved = await titlesTask;
            var targetSpecializations = await specializationsTask;
            var targetEducationalLevels = await educationalLevelsTask;
            var targetProfessions = await professionsTask;
            var targetMaritalStatus = await maritalStatusTask;
            var targetNationalityViewModels = await nationalityTask;
            var targetRelationshipPersonViewModels = await relationshipTask;

            ViewBag.TitleAchieved = targettitlesAchieved;
            ViewBag.Specializations = targetSpecializations;
            ViewBag.EducatLevel = targetEducationalLevels;
            ViewBag.Professions = targetProfessions;
            ViewBag.MaritalStatus = targetMaritalStatus;
            ViewBag.Nacionaties = targetNationalityViewModels;
            ViewBag.RelationshipPerson = targetRelationshipPersonViewModels;
            ViewBag.Roles = roles;

            if (idUser != null || idUser == "")
            {
                var studentSaveViewModel = await _userService.GetUserById(idUser);
                //studentSaveViewModel.Roles = roles;
                return PartialView(studentSaveViewModel);
            }

            return PartialView(new SaveUserViewModel { DateBirth = DateTime.Now });
        }
        
        public async Task<PartialViewResult> UpdateProfile(string? idUser)
        {
            var userName = User.Identity.Name;
            var user = await _userService.GetUserByName(userName);
            var roles = await _userService.GetAllRoles(user.Roles);
            ViewBag.Roles =  roles;

            if (idUser != null || idUser =="")
            {
                var studentSaveViewModel =await _userService.GetUserById(idUser);
                studentSaveViewModel.FromProfile = true;
                //studentSaveViewModel.Roles = roles;
                return PartialView(studentSaveViewModel);
            }
            
            return PartialView(new SaveUserViewModel { DateBirth=DateTime.Now , FromProfile=true });
        }

        [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
        [HttpPost]
        public async Task<JsonResult> CreateOrUpdate(SaveUserViewModel vm, int? idInstitu, int? institutionIdPrincipal)
        {
            
            if (!ModelState.IsValid)
            {
                return Json(new { data = "Datos sin valdiar", result = false });
            }

            var userName = User.Identity.Name;
            var origin = Request.Headers["origin"];

            var response = await _userManagementService.CreateOrUpdateUserAsync(vm, idInstitu, institutionIdPrincipal, origin);
           
            return Json(new { data = response.message, result = response.result });
        }
        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Login" });
        }
        
        [HttpPost]
        public async Task<JsonResult> Delete(string idUser)
        {
            var response = await _userService.DeleteUser(idUser);
            return Json(new { data = response.messages.FirstOrDefault(), result = response.Data });
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var response = await _userService.ConfirmEmailAsync(userId, token);

            return View("ConfirmEmail", response.Data);
        }
        public async Task<IActionResult> ForgotPassword()
        {           
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ForgotPassWordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(model);
            }
            var origin = Request.Headers["origin"];
            var response = await _userService.ForgotPasswordAync(model, origin);

            return Json(response);
        }
        public async Task<IActionResult> ResetPassword(string token)
        {           
            return View( new ResetPasswordViewModel { token = token});
        }
        
        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(model);
            }
            var origin = Request.Headers["origin"];
            var response = await _userService.ResetPasswordAsyn(model, origin);

            return Json(response);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}

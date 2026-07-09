using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers;
using MontessoriSystem.Core.Application.Helpers.ImportAndExport;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.Specialization;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Application.ViewModels.MaritalStatus;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Application.ViewModels.Professions;
using MontessoriSystem.Core.Application.ViewModels.Province;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Application.ViewModels.TitlesAchieved;
using MontessoriSystem.Core.Application.ViewModels.User;
using Newtonsoft.Json;
using System.Net;

namespace montessoriSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TeacherController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEducationalLevelService _educationalLevelService;
        private readonly ITitlesAchievedsService _titlesAchievedsService;
        private readonly IProfessionsService _professionsService;
        private readonly ISpecializationService _specializationService;
        private readonly IMaterialStatusService _materialStatusService;
        private readonly INacionalityService _nacionalityService;
        private readonly IFileServices _fileServices;
        private readonly string _wwwrootPath;
        private readonly IEducationalInstitutionService _educationalInstitutionServices;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IExcelManager _excelManager;
        private readonly IInitDataHelper _initDataHelper;


        public TeacherController(IUserService userService, IFileServices fileServices,
            IWebHostEnvironment webHostEnvironment,
            IEducationalInstitutionService educationalInstitutionServices,
            IValidateInfoDataBase validateInfoDataBase, IEducationalLevelService educationalLevelService, 
            IProfessionsService professionsService, 
            ISpecializationService specializationService, ITitlesAchievedsService titlesAchievedsService, 
            IMaterialStatusService materialStatusService, INacionalityService nacionalityService, IExcelManager excelManager, IInitDataHelper initDataHelper)
        {
            _userService = userService;
            _fileServices = fileServices;
            _wwwrootPath = webHostEnvironment.WebRootPath;
            _educationalInstitutionServices = educationalInstitutionServices;
            _validateInfoDataBase = validateInfoDataBase;
            _educationalLevelService = educationalLevelService;
            _professionsService = professionsService;
            _specializationService = specializationService;
            _titlesAchievedsService = titlesAchievedsService;
            _materialStatusService = materialStatusService;
            _nacionalityService = nacionalityService;
            _excelManager = excelManager;
            _initDataHelper = initDataHelper;
        }

        public async Task<IActionResult> Index()
        {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return RedirectToAction(action, controller);
            }
            return View();
        }
        public async Task<PartialViewResult> PartialViewUsers(int? idInstitu)
        {
            if (idInstitu == null || idInstitu <= 0)
            {
                return PartialView();
            }
            var userName = User.Identity.Name;
            var user = await _userService.GetUserByName(userName);

            if (!user.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView("");
            }

            ViewBag.IdInstitu = idInstitu;

            return PartialView("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetDatosUserJson(int? idInstitu)
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

                var filterUser = UserFilter.GetCookie(HttpContext, "filters-users");
                List<string> selectedOptions = JsonConvert.DeserializeObject<List<string>>(filterUser ?? "[]");

                var DataFilterViewModel = _userService.GetAllUser().Result.Data;
                DataFilterViewModel = DataFilterViewModel.Where(user => user.Roles.Contains(Roles.Profesor.ToString())).ToList();

                var titlesAchievedViewModels = await _titlesAchievedsService.GetAllViewModel();
                var targettitlesAchieved = GetTitlesAchieved(titlesAchievedViewModels);

                var specializationViewModels = await _specializationService.GetAllViewModel();
                var targetSpecializations = GetSpecializations(specializationViewModels);

                var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
                var targetMaritalStatus = GetCivilStatus(maritalStatusViewModels);

                var nationalityViewModels = await _nacionalityService.GetAllViewModel();
                var targetNationalityViewModels = GetNationality(nationalityViewModels);

                foreach (var item in DataFilterViewModel)
                {
                    item.Sex = item.Gender == 1 ? "Masculino" : "Femenino";
                    if (!string.IsNullOrEmpty(item.CivilStatus))
                    {
                        item.CivilStatusDesc = maritalStatusViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.CivilStatus))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.Nationality))
                    {
                        item.NationalityDesc = nationalityViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.Nationality ?? "0"))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.TitleAchieved))
                    {
                        item.TitleAchievedDesc = titlesAchievedViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.TitleAchieved ?? "0"))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.StudiesCurrentlyPursuing))
                    {
                        item.StudiesCurrentlyPursuingDesc = specializationViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.StudiesCurrentlyPursuing ?? "0"))?.Name;
                    }
                    if (item.AreaSpecialization != null)
                    {
                        item.AreaSpecializationDesc = specializationViewModels.FirstOrDefault(stu => stu.Id == item.AreaSpecialization)?.Name;
                    }

                }
                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json("");
            }

        }
        public async Task<PartialViewResult> CreateOrUpdate(string? idUser)
        {
            var userName = User.Identity.Name;
            var user = await _userService.GetUserByName(userName);

            if (!user.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView("");
            }

            var titlesAchievedViewModels = await _titlesAchievedsService.GetAllViewModel();
            var targettitlesAchieved = GetTitlesAchieved(titlesAchievedViewModels);

            var specializationViewModels = await _specializationService.GetAllViewModel();
            var targetSpecializations = GetSpecializations(specializationViewModels);

            var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
            var targetMaritalStatus = GetCivilStatus(maritalStatusViewModels);

            var nationalityViewModels = await _nacionalityService.GetAllViewModel();
            var targetNationalityViewModels = GetNationality(nationalityViewModels);

            ViewBag.TitleAchieved = targettitlesAchieved;
            ViewBag.Specializations = targetSpecializations;
            ViewBag.MaritalStatus = targetMaritalStatus;
            ViewBag.Nacionaties = targetNationalityViewModels;

            if (idUser != null || idUser == "")
            {
                var studentSaveViewModel = await _userService.GetUserById(idUser);
                //studentSaveViewModel.Roles = roles;
                return PartialView(studentSaveViewModel);
            }

            return PartialView(new SaveUserViewModel { DateBirth = DateTime.Now });
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrUpdate(SaveUserViewModel vm, int? idInstitu, int? institutionIdPrincipal)
        {

            if (!ModelState.IsValid)
            {
                return Json(new { data = "Datos sin valdiar", result = false });
            }

            var userName = User.Identity.Name;
            var user = await _userService.GetUserByName(userName);

            //Validar idInstitu antes de guardar
            if (!user.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json("");
            }


            var origin = Request.Headers["origin"];

            vm.IdUserCreator = user.Id;
            bool deleteImg = false;

            if (vm.File != null && vm.File.Length > 0)
            {
                var pathNewFolder = $"{_wwwrootPath}/FileUser/User";
                GeneralResponse<string> reponse = await _fileServices.CreateOrUpdateFile(vm.File, pathNewFolder);

                vm.UrlImage = reponse.Data;
                deleteImg = true;
            }
            vm.InstitutionId = idInstitu;
            vm.InstitutionIdPrincipal = institutionIdPrincipal;

            vm.Roles.Add(Roles.Profesor.ToString());

            if (vm.Id != null)
            {
                var exitRegister = await _userService.GetUserById(vm.Id);
                if (deleteImg && exitRegister != null)
                {
                    string rutaArchivo = $"FileUser/User/{exitRegister.UrlImage}";
                    var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                    await _fileServices.DeleteFile(filePath);
                }
                else
                {
                    vm.UrlImage = exitRegister.UrlImage;
                }
                vm.FromProfile = true;
                var response = await _userService.UpdateUserAsync(vm, vm.Roles, vm.Id);
                return Json(new { data = response.messages.FirstOrDefault(), result = response.result });
            }
            else
            {
                var response = await _userService.RegisterUsserAsync(vm, vm.Roles, origin);
                return Json(new { data = response.messages.FirstOrDefault(), result = response.result });
            }

        }

        #region Teacher Room
        public async Task<PartialViewResult> TeacherRoom(int IdRoom)
        {
            ViewBag.IdRoom = IdRoom;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetDatosTeacherRoom(int? idRoom)
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

                // Obtener todos los usuarios
                var dataFilterViewModel = _userService.GetAllUser().Result.Data;

                // Filtrar solo profesores y aquellos que tienen asignado el IdRoom
                dataFilterViewModel = dataFilterViewModel
                    .Where(u => u.Roles.Contains(Roles.Profesor.ToString())
                                && !string.IsNullOrEmpty(u.IdsRoom)
                                && u.IdsRoom.Split(',').Contains(idRoom.ToString()))
                    .ToList();

                // Obtener las descripciones adicionales
                var titlesAchievedViewModels = await _titlesAchievedsService.GetAllViewModel();
                var specializationViewModels = await _specializationService.GetAllViewModel();
                var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
                var nationalityViewModels = await _nacionalityService.GetAllViewModel();

                foreach (var item in dataFilterViewModel)
                {
                    item.Sex = item.Gender == 1 ? "Masculino" : "Femenino";

                    if (!string.IsNullOrEmpty(item.CivilStatus))
                    {
                        item.CivilStatusDesc = maritalStatusViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.CivilStatus))?.Name;
                    }

                    if (!string.IsNullOrEmpty(item.Nationality))
                    {
                        item.NationalityDesc = nationalityViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.Nationality ?? "0"))?.Name;
                    }

                    if (!string.IsNullOrEmpty(item.TitleAchieved))
                    {
                        item.TitleAchievedDesc = titlesAchievedViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.TitleAchieved ?? "0"))?.Name;
                    }

                    if (!string.IsNullOrEmpty(item.StudiesCurrentlyPursuing))
                    {
                        item.StudiesCurrentlyPursuingDesc = specializationViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.StudiesCurrentlyPursuing ?? "0"))?.Name;
                    }

                    if (item.AreaSpecialization != null)
                    {
                        item.AreaSpecializationDesc = specializationViewModels
                            .FirstOrDefault(stu => stu.Id == item.AreaSpecialization)?.Name;
                    }
                }

                return Json(dataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error en la operación", error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<PartialViewResult> QuitTeacherRoom(List<string> idsTeacher, int IdRoom)
        {
            var userName = User.Identity.Name;
            var currentUser = await _userService.GetUserByName(userName);

            if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView("");
            }
            foreach (var item in idsTeacher)
            {
                await _userService.QuitStudentTeacher(item, IdRoom);
            }


            return PartialView("Index");

        }

        [HttpPost]
        public async Task<JsonResult> AddTeacherRoom(List<string> idsTeacher, int IdRoom)
        {
            var userName = User.Identity.Name;
            var currentUser = await _userService.GetUserByName(userName);

            if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { });
            }
            foreach (var item in idsTeacher)
            {
                await _userService.AddStudentTeacher(item, IdRoom);
            }

            return Json(new { });
        }
        [HttpGet]
        public async Task<PartialViewResult> GetDatosTeacherToAddRoom(int IdRoom)
        {
            return PartialView("AddTeacherRoom");
        }
        
        [HttpGet]
        public async Task<JsonResult> GetTeacherToAddRoom(int? IdRoom)
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

                // Obtener todos los usuarios
                var dataFilterViewModel = _userService.GetAllUser().Result.Data;

                // Filtrar solo profesores y aquellos que tienen el valor de IdsRoom vacío o no contienen el IdRoom
                dataFilterViewModel = dataFilterViewModel.Where( u => u.Roles.Contains(Roles.Profesor.ToString() ) 
                && ( string.IsNullOrEmpty(u.IdsRoom) || !u.IdsRoom.Split(',').Contains( IdRoom.ToString() ) ) ).ToList();


                // Obtener las descripciones adicionales
                var titlesAchievedViewModels = await _titlesAchievedsService.GetAllViewModel();
                var specializationViewModels = await _specializationService.GetAllViewModel();
                var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
                var nationalityViewModels = await _nacionalityService.GetAllViewModel();

                foreach (var item in dataFilterViewModel)
                {
                    item.Sex = item.Gender == 1 ? "Masculino" : "Femenino";

                    if (!string.IsNullOrEmpty(item.CivilStatus))
                    {
                        item.CivilStatusDesc = maritalStatusViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.CivilStatus))?.Name;
                    }

                    if (!string.IsNullOrEmpty(item.Nationality))
                    {
                        item.NationalityDesc = nationalityViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.Nationality ?? "0"))?.Name;
                    }

                    if (!string.IsNullOrEmpty(item.TitleAchieved))
                    {
                        item.TitleAchievedDesc = titlesAchievedViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.TitleAchieved ?? "0"))?.Name;
                    }

                    if (!string.IsNullOrEmpty(item.StudiesCurrentlyPursuing))
                    {
                        item.StudiesCurrentlyPursuingDesc = specializationViewModels
                            .FirstOrDefault(stu => stu.Id == int.Parse(item.StudiesCurrentlyPursuing ?? "0"))?.Name;
                    }

                    if (item.AreaSpecialization != null)
                    {
                        item.AreaSpecializationDesc = specializationViewModels
                            .FirstOrDefault(stu => stu.Id == item.AreaSpecialization)?.Name;
                    }
                }

                return Json(dataFilterViewModel);

            }
            catch (Exception ex)
            {
                return Json(new { message = "Error en la operación", error = ex.Message });
            }
        }



        #endregion


        #region Manejo Teacher By Excel

        [HttpGet]
        public IActionResult DownloadTemplateExcel()
        {
            var filePath = Path.Combine(_wwwrootPath, "Templates", "PlantillaTeacher.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("La plantilla de Excel no se encontró.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PlantillaTeacher.xlsx");
        }

        [HttpGet]
        public async Task<PartialViewResult> ViewExcel()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<JsonResult> CreateTeacherExcel(IFormFile excelFile, int IdInstitu)
        {
            var origin = Request.Headers["origin"];

            var response = await _excelManager.TeacherImport(excelFile, origin, IdInstitu);

            return Json(new { data = response.messages.FirstOrDefault(), result = response.result });

        }

        #endregion

        #region Private Methods
        private List<ClassSelected<int>> GetTitlesAchieved(List<TitlesAchievedViewModel>? titlesAchievedViewModels)
        {
            List<ClassSelected<int>> titleAchieved = new List<ClassSelected<int>>();

            foreach (var item in titlesAchievedViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                titleAchieved.Add(classSelected);
            }
            return titleAchieved;
        }
        private List<ClassSelected<int>> GetSpecializations(List<SpecializationViewModel>? specializationViewModels)
        {
            List<ClassSelected<int>> specializations = new List<ClassSelected<int>>();

            foreach (var item in specializationViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                specializations.Add(classSelected);
            }
            return specializations;
        } 
        private List<ClassSelected<int>> GetCivilStatus(List<MaritalStatusViewModel>? maritalStatusViewModels)
        {
            List<ClassSelected<int>> civilStatus = new List<ClassSelected<int>>();

            foreach (var item in maritalStatusViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                civilStatus.Add(classSelected);
            }
            return civilStatus;
        }
        private List<ClassSelected<int>> GetNationality(List<NationalityViewModel>? nationalityViewModels)
        {
            List<ClassSelected<int>> nacionality = new List<ClassSelected<int>>();

            foreach (var item in nationalityViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                nacionality.Add(classSelected);
            }
            return nacionality;
        }
        #endregion
    }
}

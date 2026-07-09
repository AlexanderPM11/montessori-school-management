using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Application.ViewModels.MaritalStatus;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Application.ViewModels.Professions;
using MontessoriSystem.Core.Application.ViewModels.Province;
using MontessoriSystem.Core.Application.ViewModels.RelationshipPerson;
using MontessoriSystem.Core.Application.ViewModels.User;
using Newtonsoft.Json;
using System.Net;

namespace montessoriSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ParentsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEducationalLevelService _educationalLevelService;
        private readonly IProfessionsService _professionsService;
        private readonly IFileServices _fileServices;
        private readonly string _wwwrootPath;
        private readonly IEducationalInstitutionService _educationalInstitutionServices;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IMaterialStatusService _materialStatusService;
        private readonly INacionalityService _nacionalityService;
        private readonly IRelationshipPersonService _relationshipPersonService;
        private readonly IInitDataHelper _initDataHelper;

        public ParentsController(IUserService userService, IFileServices fileServices,
            IWebHostEnvironment webHostEnvironment,
            IEducationalInstitutionService educationalInstitutionServices,
            IValidateInfoDataBase validateInfoDataBase, IEducationalLevelService educationalLevelService,
            IProfessionsService professionsService, IMaterialStatusService materialStatusService, INacionalityService nacionality, 
            IRelationshipPersonService relationshipPersonService, IInitDataHelper initDataHelper)
        {
            _userService = userService;
            _fileServices = fileServices;
            _wwwrootPath = webHostEnvironment.WebRootPath;
            _educationalInstitutionServices = educationalInstitutionServices;
            _validateInfoDataBase = validateInfoDataBase;
            _educationalLevelService = educationalLevelService;
            _professionsService = professionsService;
            _materialStatusService = materialStatusService;
            _nacionalityService = nacionality;
            _relationshipPersonService = relationshipPersonService;
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

            //Validar idInstitu antes de guardar
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
                DataFilterViewModel = DataFilterViewModel.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

                var professionsViewModels = await _professionsService.GetAllViewModel();
                var educationalLevels = await _educationalLevelService.GetAllViewModel();
                var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
                var nationalityViewModels = await _nacionalityService.GetAllViewModel();
                var Relationships = await _relationshipPersonService.GetAllViewModel();

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
                    if (!string.IsNullOrEmpty(item.IdNivelEducativo))
                    {
                        item.EducationLevel = educationalLevels.FirstOrDefault(stu => stu.Id == int.Parse(item.IdNivelEducativo))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.Profession))
                    {
                        item.ProfessionDesc = professionsViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.Profession))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.Job))
                    {
                        item.JobDesc = professionsViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.Job))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.RelationshipId))
                    {
                        item.Relationship = Relationships.FirstOrDefault(stu => stu.Id == int.Parse(item.RelationshipId))?.Name;
                    }
                }
                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json("");
            }

        }
        public IActionResult ListUser()
        {
            return View();
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

            var educationalLevels = await _educationalLevelService.GetAllViewModel();
            var targetEducationalLevels = GetEducationalLevel(educationalLevels);

            var professionsViewModels = await _professionsService.GetAllViewModel();
            var targetProfessions = GetProfessions(professionsViewModels);

            var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
            var targetMaritalStatus = GetCivilStatus(maritalStatusViewModels);

            var nationalityViewModels = await _nacionalityService.GetAllViewModel();
            var targetNationalityViewModels = GetNationality(nationalityViewModels);

            var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();
            var targetRelationshipPersonViewModels = GetRelationship(relationshipPersonViewModels);

            ViewBag.EducatLevel = targetEducationalLevels;
            ViewBag.Professions = targetProfessions;
            ViewBag.MaritalStatus = targetMaritalStatus;
            ViewBag.Nacionaties = targetNationalityViewModels;
            ViewBag.RelationshipPerson = targetRelationshipPersonViewModels;
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
            vm.FromProfile = true;

            vm.Roles.Add(Roles.Padre_Tutor.ToString());

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

                var response = await _userService.UpdateUserAsync(vm, vm.Roles, vm.Id);
                return Json(new { data = response.messages.FirstOrDefault(), result = response.result });
            }
            else
            {
                var response = await _userService.RegisterUsserAsync(vm, vm.Roles, origin);
                return Json(new { data = response.messages.FirstOrDefault(), result = response.result });
            }

        }


        #region Private Methods
        private List<ClassSelected<int>> GetEducationalLevel(List<EducationalLevelViewModel>? educationalLevelViewModels)
        {
            List<ClassSelected<int>> educationalLevel = new List<ClassSelected<int>>();

            foreach (var item in educationalLevelViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                educationalLevel.Add(classSelected);
            }
            return educationalLevel;
        } 
        private List<ClassSelected<int>> GetProfessions(List<ProfessionsViewModel>? professionsViewModels)
        {
            List<ClassSelected<int>> professions = new List<ClassSelected<int>>();

            foreach (var item in professionsViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                professions.Add(classSelected);
            }
            return professions;
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
        private List<ClassSelected<int>> GetRelationship(List<RelationshipPersonViewModel>? relationshipPersonViewModels)
        {

            List<ClassSelected<int>> relationship = new List<ClassSelected<int>>();

            foreach (var item in relationshipPersonViewModels)
            {
                if(item.Name != "Tutor(a)")
                {
                    ClassSelected<int> classSelected = new ClassSelected<int>
                    {
                        Id = item.Id,
                        text = item.Name,
                    };

                    relationship.Add(classSelected);
                }
               
            }
            return relationship;
        }
        #endregion
    }
}

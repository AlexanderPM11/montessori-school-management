
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using AutoMapper;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.User;
using System.Net;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.ViewModels.Province;
using MontessoriSystem.Core.Application.ViewModels.Department;
using MontessoriSystem.Core.Application.ViewModels.InstitutionalCenterUsers;
using RealtyApp.Core.Application.Helpers;
using MontessoriSystem.Core.Application.Helpers.InitData;


namespace montessoriSystem.Controllers
{
    //[ServiceFilter(typeof(LoginAuthorize))]
    public class EducationalInstitutionController : Controller
    {
        private readonly IEducationalInstitutionService _educationalInstitutionServices;
        private readonly IFileServices _fileServices;
        private readonly string _wwwrootPath;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IProvinceDomService _provinceDomService;
        private readonly IDepartmentService _departmentService;
        private readonly IInstitutionalCenterUsersService _institutionalCenterUsersService;
        private readonly IInitDataHelper _initDataHelper;


        public EducationalInstitutionController(IEducationalInstitutionService educationalInstitutionServices, 
            IFileServices fileServices, IWebHostEnvironment webHostEnvironment,
            IUserService userService, IMapper mapper, IValidateInfoDataBase validateInfoDataBase, 
            IProvinceDomService provinceDomService, IDepartmentService departmentService, 
            IInstitutionalCenterUsersService institutionalCenterUsersService, IInitDataHelper initDataHelper)
        {
            _educationalInstitutionServices = educationalInstitutionServices;
            _fileServices = fileServices;
            _wwwrootPath = webHostEnvironment.WebRootPath;
            _userService = userService;
            _mapper = mapper;
            _validateInfoDataBase = validateInfoDataBase;
            _provinceDomService = provinceDomService;
            _departmentService = departmentService;
            _institutionalCenterUsersService = institutionalCenterUsersService;
            _initDataHelper = initDataHelper;
        }

        #region  Principal
        [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return PartialView(action, controller);
            }

            HttpContext.Session.SetString<string>("currentPage", "Centro Educativo");

            var userName = User.Identity.Name;          

            var user = await _userService.GetUserByName(userName);
            if (user.Roles.Count == 1)
            {
                if (user.Roles.Any(n => n == Roles.Profesor.ToString()))
                {
                    return RedirectToAction("Index", "Student");
                }
            }


            var educationalInstitutionViewModel = new List<EducationalInstitutionViewModel>();

            if (user.Roles.Contains(Roles.Admin.ToString()))
            {
                educationalInstitutionViewModel = await _educationalInstitutionServices.GetAllViewModel();
            }
            var provinces = await _provinceDomService.GetAllViewModel();
           

            ViewBag.Provinces = GetProvinces( provinces );
           

            if (educationalInstitutionViewModel != null && educationalInstitutionViewModel.Count > 0)
            {
                var educationModel = educationalInstitutionViewModel.FirstOrDefault();

                var appliciationUserDTO = _userService.GetAllUser().Result.Data;
                var educationalInsti = educationalInstitutionViewModel.FirstOrDefault();

                var departmentViewModels = new List<DepartmentViewModel>();

                if (user.Roles.Contains(Roles.SuperAdmin.ToString()))
                {
                    departmentViewModels = await _departmentService.GetBy(department =>
                        department.IdUserCreator == user.Id);
                }
                else
                {
                    departmentViewModels = await _departmentService.GetBy(department =>
                        department.IdUserCreator == user.IdUserCreator);
                }
                var userViewModels = _mapper.Map<List<UserViewModel>>(appliciationUserDTO);

                ViewBag.Rector = GetUserByRole(userViewModels, Roles.Rector, educationalInsti.Id, educationalInsti.Id);
                ViewBag.Cordinator =  GetUserByRole(userViewModels, Roles.Cordinador, educationalInsti.Id, educationalInsti.Id);
                ViewBag.Secretary =  GetUserByRole(userViewModels, Roles.Secretario, educationalInsti.Id, educationalInsti.Id);
                ViewBag.Departments = GetDepartment(departmentViewModels);

                var model = _mapper.Map<SaveEducationalInstitutionViewModel>(educationModel);          

                return View(model);
            }
            else
            {
                return View(new SaveEducationalInstitutionViewModel());
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? idInstitu, int institutionIdPrincipal)
        {            
            var userName = User.Identity.Name;

            var user = await _userService.GetUserByName(userName);

            var provinces = await _provinceDomService.GetAllViewModel();
            ViewBag.Provinces = GetProvinces(provinces);

            if (idInstitu == null || idInstitu <= 0)
            {
                return PartialView(new SaveEducationalInstitutionViewModel());
            }

            bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(idInstitu  ?? 0, user);

            if (!isAuthorized)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView();
            }



            var educationalInstitutionViewModel = await _educationalInstitutionServices.GetBy(headquar => headquar.Id == idInstitu
                && headquar.IdUser == user.Id && headquar.IsMainSchool == false);


            var userViewModels = await _userService.GetAllUsersByIdIinstitutionPrincipal( (int)institutionIdPrincipal! );



            #region UsersLoded

                ViewBag.Rector = GetUserByRole(userViewModels, Roles.Rector, (int)idInstitu, institutionIdPrincipal);
                ViewBag.Cordinator = GetUserByRole(userViewModels, Roles.Cordinador, (int)idInstitu, institutionIdPrincipal);
                ViewBag.Secretary = GetUserByRole(userViewModels, Roles.Secretario, (int)idInstitu, institutionIdPrincipal);
                ViewBag.AdminUssers = GetUserByRole(userViewModels, Roles.Admin, (int)idInstitu, institutionIdPrincipal);

            #endregion


            if (educationalInstitutionViewModel != null && educationalInstitutionViewModel.Count > 0)
            {
                var model = _mapper.Map<SaveEducationalInstitutionViewModel>(educationalInstitutionViewModel.FirstOrDefault());               

                return PartialView(model);
            }
            else
            {
                return PartialView(new SaveEducationalInstitutionViewModel());

            }

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<JsonResult> CreateUpdate(SaveEducationalInstitutionViewModel model)
        {
            try
            {
                var userName = User.Identity.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Json(new { data = "Ocurrió un error al obtener el usuario!", result = false });
                }

                var user = await _userService.GetUserByName(userName);

                if (user == null || !user.Roles.Contains(Roles.Admin.ToString()))
                {
                    return Json(new { data = "Usuario sin permiso para realizar esta acción", result = false });
                }

                if (ModelState.IsValid)
                {
                    bool deleteImg = false;
                    var exitRegister = new List<EducationalInstitutionViewModel>();

                    exitRegister = await _educationalInstitutionServices.GetAllViewModel();

                    if (model.File != null && model.File.Length > 0)
                    {
                        var pathNewFolder = $"{_wwwrootPath}/FileUser/EducantionalInstitution";
                        GeneralResponse<string> reponse = await _fileServices.CreateOrUpdateFile(model.File, pathNewFolder);

                        model.UrlLogo = reponse.Data;
                        deleteImg = true;
                    }

                    if (exitRegister != null && exitRegister.Count > 0)
                    {
                        var DBModel = exitRegister.FirstOrDefault();

                        if (!string.IsNullOrEmpty(DBModel.UrlLogo) && deleteImg)
                        {
                            await DeleteDoc(DBModel!.UrlLogo);

                        }
                        if (!deleteImg)
                        {
                            model.UrlLogo = DBModel.UrlLogo;
                        }

                        model.LastModifiedBy = user.UserName;

                        model.IsMainSchool = DBModel!.IsMainSchool;
                        model.IdUser = DBModel!.IdUser;
                        model.UserAssignmentId = DBModel!.UserAssignmentId;

                        await _educationalInstitutionServices.Update(model, DBModel.Id);

                        return Json(new { data = "Información actualizada correctamente!", result = true });
                    }

                    else
                    {
                        model.IdUser = user.Id;

                        model.CreatedBy = user.UserName;
                        model.IsMainSchool = true;

                        var centerAdded= await _educationalInstitutionServices.Add(model);
                        InstitutionalCenterUsersSaveViewModel InsCenterUsersSaveViewModel  =  new InstitutionalCenterUsersSaveViewModel { UserId = user.Id! , CenterId= centerAdded.Id};
                        
                        await _institutionalCenterUsersService.Add(InsCenterUsersSaveViewModel);

                        return Json(new { data = "Información actualizada correctamente!", result = true });
                    }

                }
                else
                {
                    return Json(new { data = "Datos sin validar!", result = false });
                }
            }
            catch (Exception ex)
            {

                return Json(new { data = "Ocurrio un error!.. " + ex.Message, result = false });
            }

        }

        #endregion

        #region  Headquarters

        [HttpPost]
        public async Task<JsonResult> CreateUpdateHeadquarters(SaveEducationalInstitutionViewModel model)
        {
            try
            {
                var userName = User.Identity.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Json(new { data = "Ocurrió un error al obtener el usuario!", result = false });
                }

                var user = await _userService.GetUserByName(userName);

                if (user == null || !user.Roles.Contains(Roles.SuperAdmin.ToString()))
                {
                    return Json(new { data = "Usuario sin permiso para realizar esta acción", result = false });
                }

                if (ModelState.IsValid)
                {
                    bool deleteImg = false;

                    if (model.File != null && model.File.Length > 0)
                    {
                        var pathNewFolder = $"{_wwwrootPath}/FileUser/EducantionalInstitution";
                        GeneralResponse<string> reponse = await _fileServices.CreateOrUpdateFile(model.File, pathNewFolder);

                        model.UrlLogo = reponse.Data;
                        deleteImg = true;
                    }

                    if (model.Id > 0)
                    {
                        var exitRegister = await _educationalInstitutionServices.GetBy(educatio => educatio.IdUser == user.Id &&
                        educatio.Id  == model.Id);


                        var DBModel = exitRegister.FirstOrDefault();

                        if (!string.IsNullOrEmpty(DBModel.UrlLogo) && deleteImg)
                        {
                            await DeleteDoc(DBModel!.UrlLogo);
                        }
                        if (!deleteImg)
                        {
                            model.UrlLogo = DBModel.UrlLogo;
                        }

                        model.LastModifiedBy = user.UserName;

                        model.Id = DBModel!.Id;
                        model.IdUser = user.Id;

                        await _educationalInstitutionServices.Update(model, DBModel.Id);

                        return Json(new { data = "Información actualizada correctamente!", result = true });
                    }

                    else
                    {
                        model.IdUser = user.Id;
                        model.CreatedBy = user.UserName;

                        await _educationalInstitutionServices.Add(model);

                        return Json(new { data = "Información actualizada correctamente!", result = true });
                    }

                }
                else
                {
                    return Json(new { data = "Datos sin validar!", result = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = "Ocurrio un error!.. " + ex.Message, result = false });
            }

        }

        public async Task<PartialViewResult> Headquarters(int? idInstitu)
        {
            ViewBag.IdInstitu = idInstitu;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetHeadquartersJson(int? idInstitu, int institutionIdPrincipal)
        {
            try
            {
                var userName = User.Identity.Name;
                var user = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(idInstitu ?? 0, user);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("") ;
                }

                var DataFilterViewModel = await _educationalInstitutionServices.GetBy(headquar =>  headquar.IsMainSchool == false 
                && headquar.IdUser == user.Id);

                List<UserViewModel> allUserInCenter = new List<UserViewModel>();

                if (idInstitu != null && idInstitu > 0)
                {
                    allUserInCenter = await _userService.GetAllUsersByIdIinstitutionPrincipal( (int) idInstitu );
                }

                List<ClassSelected<string>> cordinator = new List<ClassSelected<string>>();
                List<ClassSelected<string>> secretary = new List<ClassSelected<string>>();
                List<ClassSelected<string>> adminUssers = new List<ClassSelected<string>>();
                List<ClassSelected<string>> retors = new List<ClassSelected<string>>();

                if(DataFilterViewModel.Count > 0)
                {
                    
                }

                bool enterIteration = true;
                foreach (var educational in DataFilterViewModel)
                {
                    if (educational != null)
                    {
                        if (idInstitu != null && idInstitu > 0)
                        {

                            if (enterIteration)
                            {
                                cordinator = GetUserByRole(allUserInCenter, Roles.Cordinador, (int)educational.Id, institutionIdPrincipal);
                                secretary = GetUserByRole(allUserInCenter, Roles.Secretario, (int)educational.Id, institutionIdPrincipal);
                                adminUssers = GetUserByRole(allUserInCenter, Roles.Admin, (int)educational.Id, institutionIdPrincipal);
                                retors = GetUserByRole(allUserInCenter, Roles.Rector, (int)educational.Id, institutionIdPrincipal);
                            }

                            educational.NameRector = retors.FirstOrDefault(userR => userR.Id == educational.IdRector)?.text;
                            educational.NameCordinator = cordinator.FirstOrDefault(userC => userC.Id == educational.IdCordinator)?.text;
                            educational.NameSecretary = secretary.FirstOrDefault(userS => userS.Id == educational.IdSecretary)?.text;
                            educational.NameAdmin = adminUssers.FirstOrDefault(userA => userA.Id == educational.UserAssignmentId)?.text;
                        }
                          
                    }
                    enterIteration =false;

                }

                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json("");
            }

        }

        #endregion

        #region Private

        private async Task DeleteDoc(string pathDoc)
        {
            string rutaArchivo = $"FileUser/EducantionalInstitution/{pathDoc}";
            var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
            await _fileServices.DeleteFile(filePath);
        }        
        private List<ClassSelected<string>> GetUserByRole(List<UserViewModel> userViewModels, Roles role , int idInstitu, int institutionIdPrincipal)
        {


            userViewModels = userViewModels.Where(user =>
                user.Roles.Contains(role.ToString()) ).ToList();

            List<ClassSelected<string>> users = new List<ClassSelected<string>>();

            foreach (var adminUser in userViewModels)
            {
                ClassSelected<string> classSelected = new ClassSelected<string>
                {
                    Id = adminUser.Id,
                    text = $"{adminUser.FirstName} {adminUser.LastName}",
                };

                users.Add(classSelected);
            }
            return users;
        }

        private List<ClassSelected<int>> GetProvinces(List<ProvinceDomViewModel>? provinces)
        {
            List <ClassSelected<int>> provincestarget = new List<ClassSelected<int>>();

            foreach (var province in provinces)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = province.Id,
                    text = province.Name,
                };

                provincestarget.Add(classSelected);
            }
            return provincestarget;
        }
        private List<ClassSelected<int>> GetDepartment(List<DepartmentViewModel> departments)
        {
            List <ClassSelected<int>> departmentTarget = new List<ClassSelected<int>>();

            foreach (var item in departments)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                departmentTarget.Add(classSelected);
            }
            return departmentTarget;
        }



        #endregion





    }
}

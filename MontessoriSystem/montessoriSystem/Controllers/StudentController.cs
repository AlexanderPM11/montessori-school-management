using crudSignalR.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.ViewModels.User;
using MontessoriSystem.Core.Application.ViewModels.RelationshipPerson;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Application.ViewModels.TypeRegister;
using AutoMapper;
using Azure;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using Microsoft.AspNetCore.Http;

namespace montessoriSystem.Controllers
{
    //[ServiceFilter (typeof (LoginAuthorize))]
    [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
    public class StudentController : Controller
    {
        private readonly IStudentServices _studentServices;
        private readonly IExcelManager _excelManager;
        private readonly ISectionServices _sectionServices;
        private readonly IEmailService _emailService;
        private readonly IAdjuntoServices _adjuntoServices;
        private readonly IFileServices _fileServices;
        private readonly string _wwwrootPath;
        private readonly IUserService _userService;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IRelationshipPersonService _relationshipPersonService;
        private readonly INacionalityService _nacionalityService;
        private readonly IRoomService _roomService;
        private readonly ITypeRegisterService _typeRegisterService;
        private readonly IMapper _mapper;
        private readonly IInitDataHelper _initDataHelper;
        private readonly IGradeService _gradeService;


        public StudentController(IStudentServices studentServices, ISectionServices 
            sectionServices, IEmailService emailService, IAdjuntoServices adjuntoServices,
            IFileServices fileServices, IWebHostEnvironment webHostEnvironment, IUserService userService,
            IValidateInfoDataBase validateInfoDataBase, IRelationshipPersonService relationshipPersonService, 
            INacionalityService nacionalityService, IRoomService roomService, ITypeRegisterService typeRegisterService, 
            IMapper mapper, IExcelManager excelManager, IInitDataHelper initDataHelper, IGradeService gradeService)
        {
            _studentServices = studentServices;
            _sectionServices = sectionServices;
            _emailService = emailService;
            _adjuntoServices = adjuntoServices;
            _fileServices = fileServices;
            _wwwrootPath = webHostEnvironment.WebRootPath;
            _userService = userService;
            _validateInfoDataBase = validateInfoDataBase;
            _relationshipPersonService = relationshipPersonService;
            _nacionalityService = nacionalityService;
            _roomService = roomService;
            _typeRegisterService = typeRegisterService;
            _mapper = mapper;
            _excelManager = excelManager;
            _initDataHelper = initDataHelper;
            _gradeService = gradeService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return RedirectToAction(action, controller);
            }
            var userName = User.Identity.Name;
            var currentUser = await _userService.GetUserByName(userName);

            if (currentUser.Roles.Contains(Roles.Admin.ToString()))
            {
                return RedirectToAction("Index", "EducationalInstitution");
            }
                ViewBag.IdInstitu = currentUser.InstitutionId;
            //var DataFilterViewModel = await _studentServices.GetAllWithIncludeViewModel(new List<string> {"Section"});
            return View();
        }
        [HttpGet]
        public async Task<PartialViewResult> Filtro()
        {
            ViewBag.ListSection = await _sectionServices.GetAllViewModel();
            //EmailRequest request = new EmailRequest { To= "alexanderrpolanco11@gmail.com" ,Body ="Hola", Subject="New Registered"};
            //await _emailService.SendAsync(request);
            //var DataFilterViewModel = await _studentServices.GetAllWithIncludeViewModel(new List<string> {"Section"});
            return PartialView();
        }

        public async Task<PartialViewResult> GetStudent(int IdInstitu)
        {
            ViewBag.IdInstitu = IdInstitu;
            //var DataFilterViewModel = await _studentServices.GetAllWithIncludeViewModel(new List<string> {"Section"});
            return PartialView("Index");
        }
        [HttpGet]
        public async Task<JsonResult> GetDatosEstudianteJson(int IdInstitu)
        {
            try
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                //if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
                //{
                //    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                //    return Json("");
                //}
               
                var users = await _userService.GetAllUser();
                var parentsViewModel = users.Data.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

                var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();
                var DataFilterViewModel = await _studentServices.GetAllViewModel();
                var nationalityViewModels = await _nacionalityService.GetAllViewModel();
                var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();
                var grades = await _gradeService.GetAllViewModel();

                foreach (var item in DataFilterViewModel)
                {
                    if (!string.IsNullOrEmpty(item.Sexo))
                    {
                        item.SexDes = int.Parse(item.Sexo) == 1 ? "Masculino" : "Femenino";
                    }
                    if (!string.IsNullOrEmpty(item.RelationPersonLiveWith))
                    {
                        item.RelationPersonLiveWithDesc = relationshipPersonViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.RelationPersonLiveWith))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.IdFather))
                    {
                        var father = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdFather);
                        item.IdFatherDesc = $"{father?.FirstName} {father?.LastName}";
                    } 
                    if (!string.IsNullOrEmpty(item.IdMother))
                    {
                        var mother = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdMother);
                        item.IdMotherDesc = $"{mother?.FirstName} {mother?.LastName}";
                    } 
                    if (item.IdTypeRegister  != null && item.IdTypeRegister > 0)
                    {
                        var levelSelect = typeRegisterViewModels.FirstOrDefault(stu => stu.Id == item.IdTypeRegister);
                        item.Level = levelSelect.Name;
                    }
                    item.Nacionality = nationalityViewModels.FirstOrDefault(stu => stu.Id == item.IdNacionality )?.Name;
                    if(!string.IsNullOrEmpty(item.AgesSiblings))
                    {
                        string[] ages = item.AgesSiblings.Split('&');

                        item.AgesSiblings = string.Join(",", ages); 
                    }
                    if(item.IdGrade != null && item.IdGrade > 0)
                    {
                        var gradeSelect = grades.FirstOrDefault(stu => stu.Id == item.IdGrade);
                        item.GradeDes = gradeSelect.Name;
                    }
                    

                }

                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json("");
            }

        } 
        [HttpGet]
        public async Task<JsonResult> GetDatosEstudianteByParents(int IdInstitu, string IdParent)
        {
            try
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("");
                }

                var parent = await _userService.GetUserById(IdParent);
                var DataFilterViewModel = new List<StudentViewModel>();

                if (parent.Gender == 1) {

                    DataFilterViewModel = await _studentServices.GetBy(student => student.IdFather == IdParent);
                }
                else
                {
                    DataFilterViewModel = await _studentServices.GetBy(student => student.IdMother == IdParent);
                }

                var users = await _userService.GetAllUser();
                var parentsViewModel = users.Data.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

                var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();
                var nationalityViewModels = await _nacionalityService.GetAllViewModel();
                var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();
                var grades = await _gradeService.GetAllViewModel();

                foreach (var item in DataFilterViewModel)
                {
                    if (!string.IsNullOrEmpty(item.Sexo))
                    {
                        item.SexDes = int.Parse(item.Sexo) == 1 ? "Masculino" : "Femenino";
                    }
                    if (!string.IsNullOrEmpty(item.RelationPersonLiveWith))
                    {
                        item.RelationPersonLiveWithDesc = relationshipPersonViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.RelationPersonLiveWith))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.IdFather))
                    {
                        var father = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdFather);
                        item.IdFatherDesc = $"{father?.FirstName} {father?.LastName}";
                    } 
                    if (!string.IsNullOrEmpty(item.IdMother))
                    {
                        var mother = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdMother);
                        item.IdMotherDesc = $"{mother?.FirstName} {mother?.LastName}";
                    } 
                    if (item.IdTypeRegister  != null && item.IdTypeRegister > 0)
                    {
                        var levelSelect = typeRegisterViewModels.FirstOrDefault(stu => stu.Id == item.IdTypeRegister);
                        item.Level = levelSelect.Name;
                    }
                    item.Nacionality = nationalityViewModels.FirstOrDefault(stu => stu.Id == item.IdNacionality )?.Name;
                    if(!string.IsNullOrEmpty(item.AgesSiblings))
                    {
                        string[] ages = item.AgesSiblings.Split('&');

                        item.AgesSiblings = string.Join(",", ages); ;
                    }
                    if (item.IdGrade != null && item.IdGrade > 0)
                    {
                        var gradeSelect = grades.FirstOrDefault(stu => stu.Id == item.IdGrade);
                        item.GradeDes = gradeSelect.Name;
                    }

                }

                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json("");
            }

        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? idStudent, int IdInstitu, bool isPrincipalMain = true)
        {
            ViewBag.ListSection = await _sectionServices.GetAllViewModel();
            ViewBag.IsPrincipalMain = isPrincipalMain;

            var parentsViewModel = _userService.GetAllUser().Result.Data;
            parentsViewModel = parentsViewModel.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

            var parentsModel = parentsViewModel.Where(pare => pare.Gender == 1).ToList();
            var MothersModel = parentsViewModel.Where(pare => pare.Gender != 1).ToList();

            var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();
            var targetRelationshipPersonViewModels = GetRelationship(relationshipPersonViewModels);

            var nationalityViewModels = await _nacionalityService.GetAllViewModel();
            var targetNationalityViewModels = GetNationality(nationalityViewModels);
            
            var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();
            var targetLevels = GetLevels(typeRegisterViewModels);

            var parents = _mapper.Map<List<UserViewModel>>(parentsViewModel);
            var mothers = _mapper.Map<List<UserViewModel>>(MothersModel);

            var grades = await _gradeService.GetAllViewModel();

            ViewBag.Grades = GetGrades(grades);
            ViewBag.Fathers = GetParents(parents);
            ViewBag.Mothers = GetParents(mothers);
            ViewBag.RelationshipPerson = targetRelationshipPersonViewModels;
            ViewBag.Nacionaties = targetNationalityViewModels;
            ViewBag.Levels = targetLevels;

            if (idStudent!= null && idStudent!= 0)
            {
                var studentSaveViewModel = await _studentServices.GetByIdSaveViewModel((int)idStudent);
               
                if(!string.IsNullOrEmpty(studentSaveViewModel.BornDate))
                {
                    DateTime dateTime = DateTime.Parse(studentSaveViewModel.BornDate);
                    studentSaveViewModel.BornDate = dateTime.ToString();
                }
               

                return PartialView(studentSaveViewModel);
            }
            return PartialView(new StudentSaveViewModel());   
           
        }

        [HttpPost]
        public async Task<PartialViewResult> CreateUpdateStudent(StudentSaveViewModel model, int IdInstitu)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return PartialView("");
                }
                model.IdInstitu = IdInstitu;
                model.IdRoom = (model.IdRoom == 0 ? null : model.IdRoom);

                model.Age = CalculateAge(model.BornDate);

                bool deleteImg = false;
                if (model.File != null && model.File.Length > 0)
                {
                    var pathNewFolder = $"{_wwwrootPath}/FileUser/Students";
                    GeneralResponse<string> responseFile = await _fileServices.CreateOrUpdateFile(model.File, pathNewFolder);

                    model.UrlImg = responseFile.Data;
                    deleteImg = true;
                }

                if (model.Id != 0)
                {
                    var student = await _studentServices.GetByIdSaveViewModel(model.Id);
                    if (deleteImg )
                    {
                        string rutaArchivo = $"FileUser/Students/{student.UrlImg}";
                        var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                        await _fileServices.DeleteFile(filePath);
                    }
                    else
                    {
                        model.UrlImg = student.UrlImg;
                    }
                    await _studentServices.Update(model, model.Id);
                }
                else
                {
                    var studentSaveViewModel = await _studentServices.Add(model);
                }               
                
            }
            else
            {
                ViewBag.ListSection = await _sectionServices.GetAllViewModel();
                return PartialView("", model);
            }
            
            return PartialView("Index");

        }

        #region Manejo Student By Excel

        [HttpGet]
        public IActionResult DownloadTemplateExcel()
        {
            var filePath = Path.Combine(_wwwrootPath, "Templates", "PlantillaAlumnoRepresentantes.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("La plantilla de Excel no se encontró.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PlantillaAlumnoRepresentantes.xlsx");
        }

        [HttpGet]
        public async Task<PartialViewResult> ViewExcel()
        {           
            return PartialView();
        }
        [HttpPost]
        public async Task<JsonResult> CreateStudentExcel(IFormFile excelFile, int IdInstitu)
        {
            var userName = User.Identity.Name;
            var currentUser = await _userService.GetUserByName(userName);
            if(IdInstitu == 0)
            {
                IdInstitu = currentUser.InstitutionId ?? 0;
            }
            var origin = Request.Headers["origin"];

            var response =  await _excelManager.StudentAndParentsImport(excelFile);

            return Json(new { data = response.messages.FirstOrDefault(), result = response.result });

        }

        #endregion

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<PartialViewResult> ActiveInactiveStudent(int idStudent)
        {
            await _studentServices.ActiveInactiveStudent(idStudent);
            return PartialView("Index");

        }

        #region Students Room
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<PartialViewResult> QuitStudentRoom(List<int> idsStudent)
        {
            var userName = User.Identity.Name;
            var currentUser = await _userService.GetUserByName(userName);

            //var student = await _studentServices.GetByIdSaveViewModel(idStudent);
            //var room = await _roomService.GetByIdSaveViewModel(student.IdRoom ?? 0);

            if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView("");
            }
            foreach (var item in idsStudent)
            {
                await _studentServices.QuitStudentRoom(item);
            }
            

            return PartialView("Index");

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<JsonResult> AddStudentRoom(List<int> idsStudent, int IdRoom)
        {
            var userName = User.Identity.Name;
            var currentUser = await _userService.GetUserByName(userName);

            var room = await _roomService.GetByIdSaveViewModel(IdRoom);

            if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { }); 
            }
            foreach (var item in idsStudent)
            {
                await _studentServices.AddStudentRoom(item, IdRoom);
            }
           
            return Json(new {  });
        }

        [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
        public async Task<PartialViewResult> StudentRoom(int IdInstitu, int IdRoom)
        {
            ViewBag.IdInstitu = IdInstitu;
            ViewBag.IdRoomStudent = IdRoom;
            //var DataFilterViewModel = await _studentServices.GetAllWithIncludeViewModel(new List<string> {"Section"});
            return PartialView("StudentRoom");
        }
        
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<JsonResult> GetDatosEstudianteJsonToAddRoom(int IdInstitu, int IdTypeRegister, int IdRoom)
        {
            try
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { });
                }

                var users = await _userService.GetAllUser();
                var parentsViewModel = users.Data.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

                List<StudentViewModel> DataFilterViewModel =  new();

                var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();

                var roomModel = await _roomService.GetByIdSaveViewModel(IdRoom);

                if (roomModel != null)
                {
                    var idSelects = roomModel.IdTypeRegisters?.Split(",");

                    if(idSelects != null)
                    {
                        foreach (var idSelect in idSelects)
                        {
                           var modelStudent = await _studentServices.GetBy(student => student.IdRoom
                           == null && student.IdTypeRegister == int.Parse(idSelect));

                            if(modelStudent != null)
                            {
                                DataFilterViewModel.AddRange(modelStudent);
                            }
                            
                        }
                    }
                    
                }

                var nationalityViewModels = await _nacionalityService.GetAllViewModel();
                var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();
                var grades = await _gradeService.GetAllViewModel();
                foreach (var item in DataFilterViewModel)
                {
                    if (!string.IsNullOrEmpty(item.Sexo))
                    {
                        item.SexDes = int.Parse(item.Sexo) == 1 ? "Masculino" : "Femenino";
                    }
                    if (!string.IsNullOrEmpty(item.RelationPersonLiveWith))
                    {
                        item.RelationPersonLiveWithDesc = relationshipPersonViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.RelationPersonLiveWith))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.IdFather))
                    {
                        var father = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdFather);
                        item.IdFatherDesc = $"{father?.FirstName} {father?.LastName}";
                    }
                    if (!string.IsNullOrEmpty(item.IdMother))
                    {
                        var mother = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdMother);
                        item.IdMotherDesc = $"{mother?.FirstName} {mother?.LastName}";
                    }
                    item.Nacionality = nationalityViewModels.FirstOrDefault(stu => stu.Id == item.IdNacionality)?.Name;
                    if (!string.IsNullOrEmpty(item.AgesSiblings))
                    {
                        string[] ages = item.AgesSiblings.Split('&');

                        item.AgesSiblings = string.Join(",", ages); ;
                    }
                    if (item.IdTypeRegister != null && item.IdTypeRegister > 0)
                    {
                        var levelSelect = typeRegisterViewModels.FirstOrDefault(stu => stu.Id == item.IdTypeRegister);
                        item.Level = levelSelect.Name;
                    }
                    if (item.IdGrade != null && item.IdGrade > 0)
                    {
                        var gradeSelect = grades.FirstOrDefault(stu => stu.Id == item.IdGrade);
                        item.GradeDes = gradeSelect.Name;
                    }

                }

                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json("");
            }

        }

        [Authorize(Roles = "SuperAdmin,Admin,Teacher")]
        [HttpGet]
        public async Task<JsonResult> GetDatosEstudianteRoomJson(int IdInstitu, int IdRoom)
        {
            try
            {
                var userName = User.Identity.Name;
                var currentUser = await _userService.GetUserByName(userName);

                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { });
                }

                var users = await _userService.GetAllUser();
                var parentsViewModel = users.Data.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

                var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();
                
                List<StudentViewModel> DataFilterViewModel = new();

                var roomModel = await _roomService.GetByIdSaveViewModel(IdRoom);

                if (roomModel != null)
                {
                    var idSelects = roomModel.IdTypeRegisters?.Split(",");

                    if (idSelects != null)
                    {
                        foreach (var idSelect in idSelects)
                        {
                            var modelStudent = await  _studentServices.GetBy(student => student.IdRoom == IdRoom
                            && student.IdTypeRegister == int.Parse(idSelect));

                            if (modelStudent != null)
                            {
                                DataFilterViewModel.AddRange(modelStudent);
                            }
                        }
                    }
                }


                DataFilterViewModel = DataFilterViewModel.OrderBy(student => student.Lastname).ToList();

                var nationalityViewModels = await _nacionalityService.GetAllViewModel();
                var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();
                var grades = await _gradeService.GetAllViewModel();

                int number = 1;
                foreach (var item in DataFilterViewModel)
                {
                    item.NumberList = number;
                    if (!string.IsNullOrEmpty(item.Sexo))
                    {
                        item.SexDes = int.Parse(item.Sexo) == 1 ? "Masculino" : "Femenino";
                    }
                    if (!string.IsNullOrEmpty(item.RelationPersonLiveWith))
                    {
                        item.RelationPersonLiveWithDesc = relationshipPersonViewModels.FirstOrDefault(stu => stu.Id == int.Parse(item.RelationPersonLiveWith))?.Name;
                    }
                    if (!string.IsNullOrEmpty(item.IdFather))
                    {
                        var father = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdFather);
                        item.IdFatherDesc = $"{father?.FirstName} {father?.LastName}";
                    }
                    if (!string.IsNullOrEmpty(item.IdMother))
                    {
                        var mother = parentsViewModel.FirstOrDefault(stu => stu.Id == item.IdMother);
                        item.IdMotherDesc = $"{mother?.FirstName} {mother?.LastName}";
                    }
                    item.Nacionality = nationalityViewModels.FirstOrDefault(stu => stu.Id == item.IdNacionality)?.Name;
                    if (!string.IsNullOrEmpty(item.AgesSiblings))
                    {
                        string[] ages = item.AgesSiblings.Split('&');

                        item.AgesSiblings = string.Join(",", ages); ;
                    }
                    if (item.IdTypeRegister != null && item.IdTypeRegister > 0)
                    {
                        var levelSelect = typeRegisterViewModels.FirstOrDefault(stu => stu.Id == item.IdTypeRegister);
                        item.Level = levelSelect.Name;
                    }
                    if (item.IdGrade != null && item.IdGrade > 0)
                    {
                        var gradeSelect = grades.FirstOrDefault(stu => stu.Id == item.IdGrade);
                        item.GradeDes = gradeSelect.Name;
                    }
                    number++;

                }

                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {
                return Json("");
            }

        }
        
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<PartialViewResult> CreateStudentRoom(int IdInstitu, int IdRoom)
        {
            var userName = User.Identity.Name;
            var currentUser = await _userService.GetUserByName(userName);

            var room = await _roomService.GetByIdSaveViewModel(IdRoom);

            if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView("");
            }
            var DataFilterViewModel = await _studentServices.GetBy(student => student.IdInstitu == IdInstitu 
            && student.IdRoom == null);
           
            return PartialView("AddStudentRoom", DataFilterViewModel);

        }

        [HttpGet]
        public async Task<JsonResult> FilterStudentRoom(int IdInstitu, string nameLastNameStudent)
        {
            var data = await _studentServices.GetBy(student =>
                student.IdInstitu == IdInstitu &&
                student.IdRoom == null
            );
            List<StudentViewModel>? DataFilterViewModel = new List<StudentViewModel>();
            if (string.IsNullOrEmpty(nameLastNameStudent))
            {
                DataFilterViewModel = data;
            }
            else
            {
                DataFilterViewModel = data.Where(student =>
               student.Name.Contains(nameLastNameStudent)
               || student.Lastname.Contains(nameLastNameStudent)).ToList();
            }


            return Json(new { data = DataFilterViewModel});
        }



        #endregion      

        #region  Private Methos
        static int ObtenerNumeroMes(string nombreMes)
        {
            switch (nombreMes.ToLower())
            {
                case "enero":
                    return 1;
                case "febrero":
                    return 2;
                case "marzo":
                    return 3;
                case "abril":
                    return 4;
                case "mayo":
                    return 5;
                case "junio":
                    return 6;
                case "julio":
                    return 7;
                case "agosto":
                    return 8;
                case "septiembre":
                    return 9;
                case "octubre":
                    return 10;
                case "noviembre":
                    return 11;
                case "diciembre":
                    return 12;
                default:
                    throw new ArgumentException($"Nombre de mes no válido: {nombreMes}");
            }
        }
        private int CalculateAge(string  fechaString)
        {
            // Separar la cadena por espacios
            string[] partesFecha = fechaString.Split(' ');
            // Obtener el día
            int dia = int.Parse(partesFecha[0]);
            // Obtener el mes
            int mes = ObtenerNumeroMes(partesFecha[1]);
            // Obtener el año
            int año = int.Parse(partesFecha[2]);
            // Construir el objeto DateTime
            DateTime fechaDeNacimiento = new DateTime(año, mes, dia);
            // Fecha actual
            DateTime fechaActual = DateTime.Today;


            int edad = fechaActual.Year - fechaDeNacimiento.Year;

            // Restar un año si todavía no ha pasado el cumpleaños este año
            if (fechaActual.Month < fechaDeNacimiento.Month ||
                (fechaActual.Month == fechaDeNacimiento.Month && fechaActual.Day < fechaDeNacimiento.Day))
            {
                edad--;
            }

            return edad;
        }
        private List<ClassSelected<int>> GetGrades(List<GradeViewModel> userViewModels)
        {
            List<ClassSelected<int>> users = new List<ClassSelected<int>>();

            foreach (var item in userViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = $"{item.Name}",
                };

                users.Add(classSelected);
            }
            return users;
        }
        private List<ClassSelected<string>> GetParents(List<UserViewModel> userViewModels)
        {
            List<ClassSelected<string>> users = new List<ClassSelected<string>>();

            foreach (var item in userViewModels)
            {
                ClassSelected<string> classSelected = new ClassSelected<string>
                {
                    Id = item.Id,
                    text = $"{item.FirstName} {item.LastName}",
                };

                users.Add(classSelected);
            }
            return users;
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
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                relationship.Add(classSelected);

            }
            return relationship;
        }
        private List<ClassSelected<int>> GetLevels(List<TypeRegisterViewModel>? typeRegisters)
        {

            List<ClassSelected<int>> levels = new List<ClassSelected<int>>();

            foreach (var item in typeRegisters)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name,
                };

                levels.Add(classSelected);

            }
            return levels;
        }
        #endregion
    }
}

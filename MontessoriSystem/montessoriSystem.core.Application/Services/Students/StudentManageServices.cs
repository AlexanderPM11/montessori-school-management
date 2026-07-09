using AutoMapper;
using crudSignalR.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.DTOS.Student;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Student;
using System.Data;
using System.Security.Claims;

namespace MontessoriSystem.Core.Application.Services.Students
{
    public class StudentManageServices: IStudentManagementService
    {
        private readonly IStudentServices _studentServices;
        private readonly IFileServices _fileServices;
        private readonly IUserService _userService;
        private readonly IRelationshipPersonService _relationshipPersonService;
        private readonly INacionalityService _nacionalityService;
        private readonly IRoomService _roomService;
        private readonly ITypeRegisterService _typeRegisterService;
        private readonly IGradeService _gradeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string userName;


        public StudentManageServices(IStudentServices studentServices,
            IFileServices fileServices, IUserService userService, IRelationshipPersonService relationshipPersonService,
            INacionalityService nacionalityService, IRoomService roomService, ITypeRegisterService typeRegisterService,
            IGradeService gradeService, IHttpContextAccessor httpContextAccessor)
        {
            _studentServices = studentServices;
            _fileServices = fileServices;
            _userService = userService;
            _relationshipPersonService = relationshipPersonService;
            _nacionalityService = nacionalityService;
            _roomService = roomService;
            _typeRegisterService = typeRegisterService;
            _gradeService = gradeService;
            _httpContextAccessor = httpContextAccessor;
            userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        #region Students
        public async Task<GeneralResponse<List<StudentViewModel>>> GetAllStudents()
        {
            GeneralResponse<List<StudentViewModel>> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

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
                    if (item.IdTypeRegister != null && item.IdTypeRegister > 0)
                    {
                        var levelSelect = typeRegisterViewModels.FirstOrDefault(stu => stu.Id == item.IdTypeRegister);
                        item.Level = levelSelect.Name;
                    }
                    item.Nacionality = nationalityViewModels.FirstOrDefault(stu => stu.Id == item.IdNacionality)?.Name;
                    if (!string.IsNullOrEmpty(item.AgesSiblings))
                    {
                        string[] ages = item.AgesSiblings.Split('&');

                        item.AgesSiblings = string.Join(",", ages);
                    }
                    if (item.IdGrade != null && item.IdGrade > 0)
                    {
                        var gradeSelect = grades.FirstOrDefault(stu => stu.Id == item.IdGrade);
                        item.GradeDes = gradeSelect.Name;
                    }
                    if (item.UrlImg != null)
                    {
                        var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/Students/{item.UrlImg}");
                        item.UrlImg = responseBase64.Data;
                    }

                }

                response.Data = DataFilterViewModel.ToList();

                return response;

            }

            catch (Exception ex)
            {
                return new GeneralResponse<List<StudentViewModel>>();
            }
        }        
        public async Task<GeneralResponse<List<StudentViewModel>>> GetDatosEstudianteByParents(string IdParent)
        {
            GeneralResponse<List<StudentViewModel>> response = new();

            try
            {

                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                var parent = await _userService.GetUserById(IdParent);
                var DataFilterViewModel = new List<StudentViewModel>();

                if (parent.Gender == 1)
                {

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
                    if (item.IdTypeRegister != null && item.IdTypeRegister > 0)
                    {
                        var levelSelect = typeRegisterViewModels.FirstOrDefault(stu => stu.Id == item.IdTypeRegister);
                        item.Level = levelSelect.Name;
                    }
                    item.Nacionality = nationalityViewModels.FirstOrDefault(stu => stu.Id == item.IdNacionality)?.Name;
                    if (!string.IsNullOrEmpty(item.AgesSiblings))
                    {
                        string[] ages = item.AgesSiblings.Split('&');

                        item.AgesSiblings = string.Join(",", ages); ;
                    }
                    if (item.IdGrade != null && item.IdGrade > 0)
                    {
                        var gradeSelect = grades.FirstOrDefault(stu => stu.Id == item.IdGrade);
                        item.GradeDes = gradeSelect.Name;
                    }
                    if (item.UrlImg != null)
                    {
                        var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/Students/{item.UrlImg}");
                        item.UrlImg = responseBase64.Data;
                    }

                }

                response.Data = DataFilterViewModel.ToList();
                response.messages.Add("Consulta exitosa");

                return response;

            }

            catch (Exception ex)
            {
                return new GeneralResponse<List<StudentViewModel>>();
            }
        }
        public async Task<GeneralResponse<bool>> CreateUpdateStudent(StudentSaveViewModel model)
        {
            GeneralResponse<bool> response = new();

            try
            {

                var currentUser = await _userService.GetUserByName(userName);

                if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                model.IdRoom = (model.IdRoom == 0 ? null : model.IdRoom);

                model.Age = CalculateAge(model.BornDate);

                bool deleteImg = false;
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                if (model.File != null && model.File.Length > 0)
                {
                    var pathNewFolder = $"FileUser/Students";
                    var filePath = Path.Combine(uploadsPath, pathNewFolder);

                    GeneralResponse<string> responseFile = await _fileServices.CreateOrUpdateFile(model.File, filePath);
                    model.UrlImg = responseFile.Data;

                    deleteImg = true;
                }

                if (model.Id != 0)
                {
                    var student = await _studentServices.GetByIdSaveViewModel(model.Id);
                    if (deleteImg)
                    {
                        string rutaArchivo = $"FileUser/Students/{student.UrlImg}";
                        var filePath = Path.Combine(uploadsPath, rutaArchivo);

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

                return response;


            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add("Error al guardar el estudiante" +  ex.Message);

                return response;
            }

        }

        #endregion

        #region Students Room
        public async Task<GeneralResponse<List<StudentViewModel>>> GetStudentsRoom(int IdRoom)
        {
            GeneralResponse<List<StudentViewModel>> response = new();
            try
            {
                var currentUser = await _userService.GetUserByName(userName);
                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
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
                            var modelStudent = await _studentServices.GetBy(student => student.IdRoom == IdRoom
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
                    if (item.UrlImg != null)
                    {
                        var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/Students/{item.UrlImg}");
                        item.UrlImg = responseBase64.Data;
                    }
                    number++;
                }
                response.Data = DataFilterViewModel;
                response.messages.Add("Consulta éxitosa");


                return response;

            }

            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                response.result = false;

                return response;
            }
        }
        public async Task<GeneralResponse<List<StudentViewModel>>> GetStudentsToAddRoom(int IdRoom)
        {
            GeneralResponse<List<StudentViewModel>> response = new();
            try
            {
                var currentUser = await _userService.GetUserByName(userName);
                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                var users = await _userService.GetAllUser();
                var parentsViewModel = users.Data.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

                List<StudentViewModel> DataFilterViewModel = new();

                var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();
                var roomModel = await _roomService.GetByIdSaveViewModel(IdRoom);

                if (roomModel != null)
                {
                    var idSelects = roomModel.IdTypeRegisters?.Split(",");

                    if (idSelects != null)
                    {
                        foreach (var idSelect in idSelects)
                        {
                            var modelStudent = await _studentServices.GetBy(student => student.IdRoom
                            == null && student.IdTypeRegister == int.Parse(idSelect));

                            if (modelStudent != null)
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
                response.Data = DataFilterViewModel;
                response.messages.Add("Consulta éxitosa");

                return response;
            }
            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                response.result = false;

                return response;
            }
        }
        public async Task<GeneralResponse<int>> QuitStudentRoom(int idStudent)
        {
            GeneralResponse<int> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);
                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                await _studentServices.QuitStudentRoom(idStudent);

                response.messages.Add("Estudiante quitado del salón correctamente");
                response.result = true;
                response.Data = idStudent;

                return response;
            }
            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                response.result = false; 
                
                return response;

            }

           


           

        }
        public async Task<GeneralResponse<int>> AddStudentRoom(QuitAddStudentRoomDTO vm)
        {
            GeneralResponse<int> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);
                if (!currentUser.Roles.Contains(Roles.Profesor.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }
                foreach (var item in vm.IdStudents)
                {
                    await _studentServices.AddStudentRoom(item, vm.IdRoom);
                }
                response.messages.Add("Estudiante agregado del salón correctamente");
                response.result = true;
                response.Data = vm.IdStudents.FirstOrDefault();

                return response;
            }

            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                response.result = false;

                return response;
            }
        }

        #endregion

        #region Privates Methods

        private int CalculateAge(string fechaString)
        {
            if (!DateTime.TryParse(fechaString, out DateTime fechaDeNacimiento))
            {
                throw new ArgumentException("La fecha proporcionada no es válida.");
            }

            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaDeNacimiento.Year;

            // Ajustar si aún no ha cumplido años este año
            if (fechaActual.Month < fechaDeNacimiento.Month ||
                (fechaActual.Month == fechaDeNacimiento.Month && fechaActual.Day < fechaDeNacimiento.Day))
            {
                edad--;
            }

            return edad;
        }


        #endregion
    }
}

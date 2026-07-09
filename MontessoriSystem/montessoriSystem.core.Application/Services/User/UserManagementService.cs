using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Specialization;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Application.ViewModels.MaritalStatus;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Application.ViewModels.Professions;
using MontessoriSystem.Core.Application.ViewModels.RelationshipPerson;
using MontessoriSystem.Core.Application.ViewModels.TitlesAchieved;
using MontessoriSystem.Core.Application.ViewModels.User;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;


namespace MontessoriSystem.Core.Application.Services.User
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserService _userService;
        private readonly IEducationalLevelService _educationalLevelService;
        private readonly IProfessionsService _professionsService;
        private readonly IFileServices _fileServices;
        private readonly IMaterialStatusService _materialStatusService;
        private readonly INacionalityService _nacionalityService;
        private readonly IRelationshipPersonService _relationshipPersonService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly string _userName;

        public UserManagementService(IUserService userService, IFileServices fileServices, IEducationalLevelService educationalLevelService,
             IProfessionsService professionsService, IMaterialStatusService materialStatusService,
             INacionalityService nacionality, IRelationshipPersonService relationshipPersonService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _fileServices = fileServices;
            _educationalLevelService = educationalLevelService;
            _professionsService = professionsService;
            _materialStatusService = materialStatusService;
            _nacionalityService = nacionality;
            _relationshipPersonService = relationshipPersonService;
            _httpContextAccessor = httpContextAccessor;
            _userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        #region Principal Methods
        public async Task<GeneralResponse<LoginResponse>> Login(LoginViewModel model)
        {
            GeneralResponse<LoginResponse> response = new();

            try
            {
                response = await _userService.AuthenticateAsyncWebApi(model);

                if(response.Data != null)
                {
                    var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/User/{response.Data.UrlImage}");

                    response.Data.UrlImage = responseBase64.Data;

                }
               

                return response;
            }

            catch (Exception ex)
            {
                var error = ($"Error al convertir intentar hacer el login: {ex.Message}");
                response.result = false;
                response.messages.Add(error);

                return response;
            }
        }
        public async Task<(string message, bool result)> CreateOrUpdateUserAsync(SaveUserViewModel vm, int? idInstitu, int? institutionIdPrincipal, string origin)
        {
            try
            {              

                var user = await _userService.GetUserByName(_userName);

               
                if(!user.Roles.Contains(Roles.Admin.ToString()) && string.IsNullOrEmpty(vm.Id))
                {
                   return ("Usuario  sin permiso", false);
                }

                bool isAdmin = user.Roles.Contains(Roles.Admin.ToString());
                bool isNotSuperAdmin = !user.Roles.Contains(Roles.SuperAdmin.ToString());
                bool isEditingOtherUser = !user.Id.Equals(vm.Id);

                if (isNotSuperAdmin && isAdmin && isEditingOtherUser)
                {
                    var (message, isValid) = await ValidateAdminCannotEditOtherAdmins(vm.Id, vm.Roles);
                    if (!isValid)
                        return (message, false);
                }


                bool deleteImg = false;

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                if (vm.File != null && vm.File.Length > 0)
                {                   
                    
                    var pathNewFolder = $"FileUser/User";
                    var filePath = Path.Combine(uploadsPath, pathNewFolder);

                    GeneralResponse<string> responseFile = await _fileServices.CreateOrUpdateFile(vm.File, filePath);

                    vm.UrlImage = responseFile.Data;
                    deleteImg = true;
                }

                List<string> roles = vm.Roles
                .SelectMany(item => JsonConvert.DeserializeObject<List<string>>(item))
                .ToList();

                if (vm.Id != null)
                {
                    var exitRegister = await _userService.GetUserById(vm.Id);
                    if (deleteImg && exitRegister != null)
                    {
                        string rutaArchivo = $"FileUser/User/{exitRegister.UrlImage}";
                        var filePath = Path.Combine(uploadsPath, rutaArchivo);

                        await _fileServices.DeleteFile(filePath);
                    }
                    else
                    {
                        vm.UrlImage = exitRegister?.UrlImage;
                    }

                    var response = await _userService.UpdateUserAsync(vm, roles, vm.Id);
                    return (response.messages.FirstOrDefault() ?? "", response.result);
                }

                else
                {
                    vm.IdUserCreator = user.Id;

                    var response = await _userService.RegisterUsserAsync(vm, roles, origin);
                    return (response.messages.FirstOrDefault() ?? "", response.result);
                }
            }

            catch (Exception ex)
            {
                return (ex.Message, false);
            }            
        }
        public async Task<(List<AppliciationUserDTO> data, string message, bool result)> GetUsersAsync( List<string>? selectedRole = null)
        {
            try
            {             

                //var userName = User.Identity.Name;
                var user = await _userService.GetUserByName(_userName);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                    return (new List<AppliciationUserDTO>(), "Usuario  sin permiso", false);
                }                

                var DataFilterViewModel = _userService.GetAllUser().Result.Data;

                // Filtrar los resultados basados en las opciones seleccionadas
                if (selectedRole.Count() == 0)
                {
                    selectedRole.Add("Todos");
                }
                if (!(selectedRole.Contains("Todos")))
                {
                    DataFilterViewModel = DataFilterViewModel.Where(user =>
                     selectedRole.Any(option => user.Roles.Contains(option))).ToList();
                }

                var professionsViewModels = await _professionsService.GetAllViewModel();
                var educationalLevels = await _educationalLevelService.GetAllViewModel();
                var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
                var nationalityViewModels = await _nacionalityService.GetAllViewModel();
                var Relationships = await _relationshipPersonService.GetAllViewModel();

                foreach (var item in DataFilterViewModel)
                {
                    item.Sex = item.Gender == 1 ? "Masculino" : "Femenino";
                    item.EstadoDes = item.Estado ? "Activo" : "Inactivo";
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

                    if (item.UrlImage != null)
                    {
                        var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/User/{item.UrlImage}");
                        item.UrlImage = responseBase64.Data;
                    }
                }

                return (DataFilterViewModel,"", true);

            }

            catch (Exception ex)
            {
                return (new List<AppliciationUserDTO> (), ex.Message, false);
            }
        }
        public async Task<GeneralResponse<ParentsResponseDTO>> GetFathersAndMothers()
        {
            GeneralResponse<ParentsResponseDTO> response = new();
            response.Data = new ParentsResponseDTO();
            try
            {

                var parentsViewModel = _userService.GetAllUser().Result.Data;
                parentsViewModel = parentsViewModel.Where(user => user.Roles.Contains(Roles.Padre_Tutor.ToString())).ToList();

                var parentsModel = parentsViewModel.Where(pare => pare.Gender == 1).ToList();
                var MothersModel = parentsViewModel.Where(pare => pare.Gender != 1).ToList();

                response.Data.Fathers = GetParents(parentsModel);
                response.Data.Mothers = GetParents(MothersModel);

               response.messages.Add("Consulta exitosa");


                return response;

            }

            catch (Exception ex)
            {
                
                return new GeneralResponse<ParentsResponseDTO>
                {
                    result = false,
                    messages = new List<string> { ex.Message }
                };
            }
        }        
        public async Task<GeneralResponse<string>> ActiveInactive(string idUser)
        {
            GeneralResponse<string> response = new();
            try
            {               
                //var userName = User.Identity.Name;
                var user = await _userService.GetUserByName(_userName);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.messages.Add( "Usuario  sin permiso" );
                    response.result = false;
                    return response;
                }

                var userToActiveInactive = await _userService.GetUserById(idUser);

                if(userToActiveInactive != null)
                {
                    userToActiveInactive.Estado = !userToActiveInactive.Estado;

                    await _userService.UpdateUserAsync(userToActiveInactive, userToActiveInactive.Roles, idUser);
                }               

                return response;
            }

            catch (Exception ex)
            {
                var error = ($"Error al convertir intentar hacer el login: {ex.Message}");
                response.result = false;
                response.messages.Add(error);

                return response;
            }
        }       
        public List<ClassSelected<int>> GetTitlesAchieved(List<TitlesAchievedViewModel>? titlesAchievedViewModels)
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
        public List<ClassSelected<int>> GetSpecializations(List<SpecializationViewModel>? specializationViewModels)
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
        public List<ClassSelected<int>> GetEducationalLevel(List<EducationalLevelViewModel>? educationalLevelViewModels)
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
        public List<ClassSelected<int>> GetProfessions(List<ProfessionsViewModel>? professionsViewModels)
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
        public List<ClassSelected<int>> GetCivilStatus(List<MaritalStatusViewModel>? maritalStatusViewModels)
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
        public List<ClassSelected<int>> GetNationality(List<NationalityViewModel>? nationalityViewModels)
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
        public List<ClassSelected<int>> GetRelationship(List<RelationshipPersonViewModel>? relationshipPersonViewModels)
        {

            List<ClassSelected<int>> relationship = new List<ClassSelected<int>>();

            foreach (var item in relationshipPersonViewModels)
            {
                if (item.Name != "Tutor(a)")
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
        public async Task<GeneralResponse<List<string>>> GetAllRoles()
        {
            GeneralResponse<List<string>> response = new();

            var user = await _userService.GetUserByName(_userName);
            var targetResponse = await _userService.GetAllRoles(user.Roles);
            response.Data = targetResponse;
            response.result = true;
            response.messages.Add("Consulta exitosa");

            return response;
        }

        #endregion

        #region Teacher Room        
        public async Task<GeneralResponse<List<AppliciationUserDTO>>> GetTeacherRoom(int idRoom)
        {
            GeneralResponse<List<AppliciationUserDTO>> response = new();

            try
            {
                var user = await _userService.GetUserByName(_userName);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("Usuario sin permiso");
                    return response;
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
                    if (item.UrlImage != null)
                    {
                        var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/User/{item.UrlImage}");
                        item.UrlImage = responseBase64.Data;
                    }
                }

                response.Data = dataFilterViewModel;
                response.messages.Add("Consulta exitosa");

                return response;
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add($"Error al obtener los profesores: {ex.Message}");
                return response;
            }
        }
        public async Task<GeneralResponse<string>> QuitTeacherRoom(QuitAddTeacherRoomDTO vm)
        {
            GeneralResponse<string> response = new();
            try
            {
                var user = await _userService.GetUserByName(_userName);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("Usuario sin permiso");
                    return response;
                }

                foreach (var item in vm.IdTeachers)
                {
                    await _userService.QuitStudentTeacher(item, vm.IdRoom);
                }
                var count = vm.IdTeachers?.Count ?? 0;
                var mensaje = count == 1
                ? "Profesor quitado de la sala correctamente."
                : "Profesores quitados de la sala correctamente.";
                response.messages.Add(mensaje);
                return response;
            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add($"Error al obtener los profesores: {ex.Message}");
                return response;
            }
        }
        public async Task<GeneralResponse<string>> AddTeacherRoom(QuitAddTeacherRoomDTO vm)
        {
            GeneralResponse<string> response = new();
            try
            {
                var user = await _userService.GetUserByName(_userName);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("Usuario sin permiso");
                    return response;
                }
                foreach (var item in vm.IdTeachers)
                {
                    await _userService.AddStudentTeacher(item, vm.IdRoom);
                }
                var count = vm.IdTeachers?.Count ?? 0;
                var mensaje = count == 1
                    ? "Profesor agregado a la sala correctamente."
                    : "Profesores agregados a la sala correctamente.";
                response.messages.Add(mensaje);
                return response;

            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add($"Error al obtener los profesores: {ex.Message}");
                return response;
            }
           
            
        }
        public async Task<GeneralResponse<List<AppliciationUserDTO>>> GetTeacherToAddRoom(int? IdRoom)
        {
            GeneralResponse<List<AppliciationUserDTO>> response = new();

            try
            {
                var user = await _userService.GetUserByName(_userName);

                if (!user.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("Usuario sin permiso");
                    return response;
                }

                // Obtener todos los usuarios
                var dataFilterViewModel = _userService.GetAllUser().Result.Data;

                // Filtrar solo profesores y aquellos que tienen el valor de IdsRoom vacío o no contienen el IdRoom
                dataFilterViewModel = dataFilterViewModel.Where(u => u.Roles.Contains(Roles.Profesor.ToString())
                && (string.IsNullOrEmpty(u.IdsRoom) || !u.IdsRoom.Split(',').Contains(IdRoom.ToString()))).ToList();


                // Obtener las descripciones adicionales
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
                    if (item.UrlImage != null)
                    {
                        var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/User/{item.UrlImage}");
                        item.UrlImage = responseBase64.Data;
                    }
                }

                response.Data = dataFilterViewModel;
                response.messages.Add("Consulta exitosa");
                return response;

            }
            catch (Exception ex)
            {
                response.result = false;
                response.messages.Add($"Error al obtener los profesores: {ex.Message}");
                return response;
            }
        }
        
        #endregion



        #region Privates Methos 
        private List<ClassSelected<string>> GetParents(List<AppliciationUserDTO> parents)
        {
            List<ClassSelected<string>> parentsResponse = new List<ClassSelected<string>>();

            foreach (var item in parents)
            {
                ClassSelected<string> classSelected = new ClassSelected<string>
                {
                    Id = item.Id,
                    text = $"{item.FirstName} {item.LastName}",
                };

                parentsResponse.Add(classSelected);
            }
            return parentsResponse;
        }
        private async Task<(string Message, bool IsValid)> ValidateAdminCannotEditOtherAdmins(string? idUser, List<string> roles)
        {
            var deniedResponse = (
                "No puede crear o editar un usuario con el rol 'SuperAdmin' o 'Admin' porque usted posee el rol 'Admin'. " +
                "Esta acción está restringida por motivos de control y seguridad.", false
            );

            var targetRoles = roles
                .SelectMany(roleJson => JsonConvert.DeserializeObject<List<string>>(roleJson))
                .ToList();

            if (targetRoles.Any(role => role == Roles.Admin.ToString() || role == Roles.SuperAdmin.ToString()))
            {
                return deniedResponse;
            }

            if (!string.IsNullOrEmpty(idUser))
            {
                var user = await _userService.GetUserById(idUser);
                if (user == null)
                    return ("Usuario no encontrado.", false);

                if (user.Roles.Any(role => role == Roles.Admin.ToString() || role == Roles.SuperAdmin.ToString()))
                {
                    return deniedResponse;
                }
            }

            return (string.Empty, true);
        }

        #endregion

    }

}

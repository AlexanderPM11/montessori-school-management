using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.DTOS.General;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers.Date;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.User;
using MontessoriSystem.Core.Domain.Settings;
using System.Data;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services.General
{
    public class SelectionService: ISelectionService
    {
       
        private readonly IEducationalLevelService _educationalLevelService;
        private readonly IProvinceDomService _provinceDomService;
        private readonly IProfessionsService _professionsService;
        private readonly IMaterialStatusService _materialStatusService;
        private readonly INacionalityService _nacionalityService;
        private readonly IRelationshipPersonService _relationshipPersonService;
        private readonly ISpecializationService _specializationService;
        private readonly ITitlesAchievedsService _titlesAchievedsService;
        private readonly IGradeService _gradeService;
        private readonly ITypeRegisterService _typeRegisterService;
        private readonly IUserService _userService;
        private readonly IDateAndTimeManage _dateAndTimeManage;
        IConfiguration _configuration;


        public SelectionService( IEducationalLevelService educationalLevelService,
            IProfessionsService professionsService, IMaterialStatusService materialStatusService,
            INacionalityService nacionality, IRelationshipPersonService relationshipPersonService,
            ISpecializationService specializationService, ITitlesAchievedsService titlesAchievedsService,
            IGradeService gradeService, ITypeRegisterService typeRegisterService, IUserService userService,
            IConfiguration configuration, IOptions<EducationalPeriod> educationalPeriodOptions, IProvinceDomService provinceDomService, IDateAndTimeManage dateAndTimeManage)
        {
            _educationalLevelService = educationalLevelService;
            _professionsService = professionsService;
            _materialStatusService = materialStatusService;
            _nacionalityService = nacionality;
            _relationshipPersonService = relationshipPersonService;
            _specializationService = specializationService;
            _titlesAchievedsService = titlesAchievedsService;
            _gradeService = gradeService;
            _typeRegisterService = typeRegisterService;
            _userService = userService;
            _configuration = configuration;
            _provinceDomService = provinceDomService;
            _dateAndTimeManage = dateAndTimeManage;
        }


        public async Task<List<ClassSelected<int>>> GetTitlesAchieved()
        {
            var titlesAchievedViewModels = await _titlesAchievedsService.GetAllViewModel();
           
            return titlesAchievedViewModels?.Select(item => new ClassSelected<int>
            {
                Id = item.Id,
                text = item.Name
            }).ToList() ?? new List<ClassSelected<int>>();
        }

        public async Task<List<ClassSelected<int>>> GetSpecializations()
        {
            var specializationViewModels = await _specializationService.GetAllViewModel();
            
            return specializationViewModels?.Select(item => new ClassSelected<int>
            {
                Id = item.Id,
                text = item.Name
            }).ToList() ?? new List<ClassSelected<int>>();
        }

        public async Task<List<ClassSelected<int>>> GetEducationalLevel()
        {

            var educationalLevels = await _educationalLevelService.GetAllViewModel();
            
            return educationalLevels?.Select(item => new ClassSelected<int>
            {
                Id = item.Id,
                text = item.Name
            }).ToList() ?? new List<ClassSelected<int>>();
        }
        public async Task<List<ClassSelected<int>>> GetProvices()
        {

            var educationalLevels = await _provinceDomService.GetAllViewModel();
            
            return educationalLevels?.Select(item => new ClassSelected<int>
            {
                Id = item.Id,
                text = item.Name
            }).ToList() ?? new List<ClassSelected<int>>();
        }

        public async Task<List<ClassSelected<int>>> GetProfessions()
        {
            var professionsViewModels = await _professionsService.GetAllViewModel();          
                      
            return professionsViewModels?.Select(item => new ClassSelected<int>
            {
                Id = item.Id,
                text = item.Name
            }).ToList() ?? new List<ClassSelected<int>>();
        }

        public async Task<List<ClassSelected<int>>> GetCivilStatus()
        {
                var maritalStatusViewModels = await _materialStatusService.GetAllViewModel();
                return maritalStatusViewModels?.Select(item => new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name
                }).ToList() ?? new List<ClassSelected<int>>();
            }

        public async Task<List<ClassSelected<int>>> GetNationality()
        {
            var nationalityViewModels = await _nacionalityService.GetAllViewModel();
           
            return nationalityViewModels?.Select(item => new ClassSelected<int>
            {
                Id = item.Id,
                text = item.Name
            }).ToList() ?? new List<ClassSelected<int>>();
        }

        public async Task<List<ClassSelected<int>>> GetGrades()
        {
            List<ClassSelected<int>> grades = new List<ClassSelected<int>>();
            var gradesData = await _gradeService.GetAllViewModel();

            foreach (var item in gradesData)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = $"{item.Name}",
                };

                grades.Add(classSelected);
            }
            return grades;
        }
        public async Task<List<ClassSelected<int>>> GetLevels()
        {
            List<ClassSelected<int>> levels = new List<ClassSelected<int>>();
            var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();

            foreach (var item in typeRegisterViewModels)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = item.Id,
                    text = $"{item.Name}",
                };

                levels.Add(classSelected);
            }
            return levels;
        }
        public async Task<List<ClassSelected<int>>> GetRelationship()
        {
            var relationshipPersonViewModels = await _relationshipPersonService.GetAllViewModel();

            return relationshipPersonViewModels?
                .Where(item => item.Name != "Tutor(a)")
                .Select(item => new ClassSelected<int>
                {
                    Id = item.Id,
                    text = item.Name
                }).ToList() ?? new List<ClassSelected<int>>();
        }
        public async Task<List<ClassSelected<string>>> GetTeachersByIdRoom(int idRoom)
        {

            var teacherUsers = _userService.GetAllUser().Result.Data;

            // Filtrar solo profesores y aquellos que tienen asignado el IdRoom
            teacherUsers = teacherUsers
                .Where(u => u.Roles.Contains(Roles.Profesor.ToString())
                            && !string.IsNullOrEmpty(u.IdsRoom)
                            && u.IdsRoom.Split(',').Contains(idRoom.ToString()))
                .ToList();

            List<ClassSelected<string>> users = new List<ClassSelected<string>>();

            foreach (var adminUser in teacherUsers)
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
        public async Task<List<ClassSelected<string>>> GetTeachers()
        {
            var teacherUsers = _userService.GetAllUser().Result.Data;

            teacherUsers = teacherUsers.Where(userTeacher =>
                userTeacher.Roles.Contains(Roles.Profesor.ToString())
                && userTeacher.Statu).ToList();

            List<ClassSelected<string>> users = new List<ClassSelected<string>>();

            foreach (var adminUser in teacherUsers)
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
        private async Task<List<ClassSelected<string>>> GetUserByRole(List<AppliciationUserDTO?> userViewModels, Roles role)
        {
            userViewModels = userViewModels.Where(user =>
               user.Roles.Contains(role.ToString())).ToList();

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

        public async Task<Dictionary<string, List<ClassSelected<string>>>> GetGeneralsUserRoles()
        {
            var userViewModels = await _userService.GetAllUser();

            var Rector = await GetUserByRole(userViewModels.Data, Roles.Rector);
            var Cordinator = await GetUserByRole(userViewModels.Data, Roles.Cordinador);
            var Secretary = await GetUserByRole(userViewModels.Data, Roles.Secretario);
            var AdminUsers = await GetUserByRole(userViewModels.Data, Roles.Admin);

            return new Dictionary<string, List<ClassSelected<string>>>
            {
                { "Rector", Rector },
                { "Cordinador", Cordinator },
                { "Secretario", Secretary },
                { "Admin", AdminUsers }
            };
        }
        public Task<List<ClassSelected<string>>> GetPeriods(bool isPrimaria = false)
        {
            var periods = new List<ClassSelected<string>>();

            if (!isPrimaria)
            {
                var agosto = _configuration.GetSection("EducationalPeriod:Agosto_Diciembre");
                if (agosto.Exists())
                    periods.Add(new ClassSelected<string> { Id = "Agosto_Diciembre", text = agosto.Value });

                var enero = _configuration.GetSection("EducationalPeriod:Enero_Marzo");
                if (enero.Exists())
                    periods.Add(new ClassSelected<string> { Id = "Enero_Marzo", text = enero.Value });

                var abril = _configuration.GetSection("EducationalPeriod:Abril_Junio");
                if (abril.Exists())
                    periods.Add(new ClassSelected<string> { Id = "Abril_Junio", text = abril.Value });
            }
            else
            {
                var Agosto_Octubre = _configuration.GetSection("EducationalPeriodCalifict:Agosto_Octubre");
                if (Agosto_Octubre.Exists())
                    periods.Add(new ClassSelected<string> { Id = "Agosto_Octubre", text = Agosto_Octubre.Value });

                var Noviembre_Enero = _configuration.GetSection("EducationalPeriodCalifict:Noviembre_Enero");
                if (Noviembre_Enero.Exists())
                    periods.Add(new ClassSelected<string> { Id = "Noviembre_Enero", text = Noviembre_Enero.Value });

                var Febrero_Marzo = _configuration.GetSection("EducationalPeriodCalifict:Febrero_Marzo");
                if (Febrero_Marzo.Exists())
                    periods.Add(new ClassSelected<string> { Id = "Febrero_Marzo", text = Febrero_Marzo.Value });

                var Abril_Junio = _configuration.GetSection("EducationalPeriodCalifict:Abril_Junio");
                if (Abril_Junio.Exists())
                    periods.Add(new ClassSelected<string> { Id = "Febrero_Marzo", text = Abril_Junio.Value });
            }

            



            return Task.FromResult(periods);
        }
        public async Task<GeneralResponse<GendersCivStatuNacLevEducRelati>> GendersCivStatuNacLevEducRelati()
        {
            GeneralResponse<GendersCivStatuNacLevEducRelati> response = new();
            response.Data = new GendersCivStatuNacLevEducRelati();
            try
            {
                // Ejecutar todas las tareas en paralelo
                var civilStatusTask = GetCivilStatus();
                var nationalityTask = GetNationality();
                var educationalLevelTask = GetEducationalLevel();
                var relationshipTask = GetRelationship();
                var professionsTask = GetProfessions();
                var gradesTask = GetGrades();
                var levelsTask = GetLevels();

                // Esperar a que todas las tareas finalicen
                await Task.WhenAll(civilStatusTask, nationalityTask, educationalLevelTask, relationshipTask, professionsTask, gradesTask, levelsTask);

                response.Data.EducationalLevel = educationalLevelTask.Result;
                response.Data.Nationality = nationalityTask.Result;
                response.Data.CivilStatus = civilStatusTask.Result;
                response.Data.Relationship = relationshipTask.Result;
                response.Data.Professions = professionsTask.Result;
                response.Data.Grades = gradesTask.Result;
                response.Data.Levels = levelsTask.Result;

                response.result = true; 

                return response;
            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);

                return response;
            }
        }

        public async Task<GeneralResponse<List<string>>> GetDaysOfWeek()
        {
            GeneralResponse<List<string>> response = new();
            try
            {
                List<string> weekDays = _dateAndTimeManage.GetCurrentOrPreviousWeekDays();
                response.Data = weekDays; 

                return response;
            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);

                return response;
            }
        }


    }
}

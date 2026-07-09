using AutoMapper;
using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriod;
using MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriodWithCalification;
using MontessoriSystem.Core.Application.ViewModels.ObservationCommentEvaluation;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Application.ViewModels.User;
using System.Security.Claims;

namespace MontessoriSystem.Core.Application.Services.Reports
{
    public class ReportsCustomService : IReportsCustomService
    {
        private readonly IAchievementIndicatorsService _indicatorsService;
        private readonly IEvaluationsPeriodService _evaluationsPeriodService;
        private readonly IStudentServices _studentServices;
        private readonly IGradeService _gradeService;
        private readonly IUserService _userService;
        private readonly IEducationalInstitutionService _educationalInstitution;
        private readonly IObservationCommentEvaluationService _observationCommentEvaluation;
        private readonly IDateAndTimeManage _dateAndTimeManage;
        private readonly ISujectService _sujectService;
        private readonly IRoomTeacherService _roomTeacherService;
        private readonly IEvaluationsPeriodWithCalificationService _evaluationsPeriodWithCalification;
        private readonly IMapper _mapper;
        private readonly IProvinceDomService _provinceDomService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _userName;
        public ReportsCustomService(IAchievementIndicatorsService indicatorsService, IEvaluationsPeriodService evaluationsPeriodService,
            IStudentServices studentServices, IGradeService gradeService, IUserService userService,
            IEducationalInstitutionService educationalInstitution, IObservationCommentEvaluationService observationCommentEvaluation, IDateAndTimeManage dateAndTimeManage
            , ISujectService sujectService, IRoomTeacherService roomTeacherService, 
            IEvaluationsPeriodWithCalificationService evaluationsPeriodWithCalification, IMapper mapper, 
            IProvinceDomService provinceDomService, IHttpContextAccessor httpContextAccessor)
        {
            _indicatorsService = indicatorsService;
            _evaluationsPeriodService = evaluationsPeriodService;
            _studentServices = studentServices;
            _gradeService = gradeService;
            _userService = userService;
            _educationalInstitution = educationalInstitution;
            _observationCommentEvaluation = observationCommentEvaluation;
            _dateAndTimeManage = dateAndTimeManage;
            _sujectService = sujectService;
            _roomTeacherService = roomTeacherService;
            _evaluationsPeriodWithCalification = evaluationsPeriodWithCalification;
            _mapper = mapper;
            _provinceDomService = provinceDomService;
            _httpContextAccessor = httpContextAccessor;
            _userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }
        public async Task<(GeneralResponse<AchievementIndicatorStatusViewModelResponse>, string view, string fullNameStudent)> Report(int IdStudent, string? userName, bool preview = false, string period = "", int IdSubject = 0)
        {
            GeneralResponse<AchievementIndicatorStatusViewModelResponse> generalResponse = new();
            try
            {
                var currentUser = await _userService.GetUserByName(userName ?? _userName);

                var student = await _studentServices.GetByIdSaveViewModel(IdStudent);
                var grade = "";

                if (student.IdGrade != null)
                {
                    var result = await _gradeService.GetByIdSaveViewModel(student.IdGrade ?? 0);
                    grade = result.Name;
                }
                AchievementIndicatorStatusViewModelResponse reportModel = new();

                (reportModel, string view) = await GetReport(grade, IdStudent, student, currentUser, preview, period, IdSubject);

                if (reportModel != null)
                {
                    generalResponse.result = true;
                    generalResponse.Data = reportModel;
                    generalResponse.messages.Add("Proceso realizado correctamente !!");
                    return (generalResponse, view, $"{student.Name}  {student.Lastname}");
                }
                generalResponse.result = false;
                generalResponse.messages.Add("El informe para este grado está en proceso de construcción y estará disponible próximamente. " +
                    "¡Gracias por su paciencia!");

                return (generalResponse, "", "");

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<GeneralResponse<string>> UpdateEvaluationInitial(int idAchievementIndicator, string estado, int idStudent, string period)
        {

            GeneralResponse<string> generalResponse = new();

            try
            {
                SaveEvaluationsPeriodViewModel evaluationsPeriodViewModel = new();
                evaluationsPeriodViewModel.Periodo = period;
                evaluationsPeriodViewModel.Estado = estado;

                evaluationsPeriodViewModel.IdAchievementIndicator = idAchievementIndicator;
                evaluationsPeriodViewModel.IdStudent = idStudent;

                var student = await _studentServices.GetByIdSaveViewModel(idStudent);
                var gradeStudent = "";

                if (student.IdGrade != null)
                {
                    var result = await _gradeService.GetByIdSaveViewModel(student.IdGrade ?? 0);
                    gradeStudent = result.Name;
                }

                var existRegister = await _evaluationsPeriodService.GetBy(x => x.IdStudent.Equals(idStudent)
                && x.IdAchievementIndicator.Equals(idAchievementIndicator) && x.Periodo.Equals(period));

                if (existRegister != null && existRegister.Count > 0)
                {
                    var register = existRegister.FirstOrDefault();

                    evaluationsPeriodViewModel.LastModified = DateTime.Now;
                    evaluationsPeriodViewModel.LastModifiedBy = "Admin";
                    evaluationsPeriodViewModel.Year = register.Year;
                    evaluationsPeriodViewModel.GradeStudent = "Admin";

                    evaluationsPeriodViewModel.Created = register?.Created ?? DateTime.Now;
                    evaluationsPeriodViewModel.CreatedBy = register?.CreatedBy ?? "Admin";

                    evaluationsPeriodViewModel.Id = register.Id;
                    evaluationsPeriodViewModel.GradeStudent = register.GradeStudent;
                    evaluationsPeriodViewModel.Year = register.Year;

                    await _evaluationsPeriodService.Update(evaluationsPeriodViewModel, register.Id);

                }
                else
                {
                    evaluationsPeriodViewModel.GradeStudent = gradeStudent;
                    evaluationsPeriodViewModel.Year = DateTime.Now.Year;
                    var result = await _evaluationsPeriodService.Add(evaluationsPeriodViewModel);
                }

                generalResponse.result = true;
                generalResponse.messages.Add("Proceso realizado correctamente !!");

                return generalResponse;
            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add(ex.Message);
                return generalResponse;

            }
        }
        public async Task<GeneralResponse<string>> DeleteEvaluationInitial(int idAchievementIndicator, int idStudent, string period)
        {

            GeneralResponse<string> generalResponse = new();

            try
            {
                var existRegister = await _evaluationsPeriodService.GetBy(x => x.IdStudent.Equals(idStudent)
                && x.IdAchievementIndicator.Equals(idAchievementIndicator) && x.Periodo.Equals(period));

                if (existRegister != null && existRegister.Count > 0)
                {
                    var model = existRegister.FirstOrDefault();
                    await _evaluationsPeriodService.Delete(model.Id);
                }

                generalResponse.Data = "Proceso realizado correctamente !!";
                generalResponse.messages.Add("Proceso realizado correctamente !!");

                return generalResponse;
            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add(ex.Message);
                return generalResponse;

            }
        }
        public async Task<GeneralResponse<string>> SaveComments(int IdStudent, string comment1, string comment2, string period)
        {

            GeneralResponse<string> generalResponse = new();

            try
            {
                SaveViewModelObservationCommentEvaluation model = new();
                var student = await _studentServices.GetByIdSaveViewModel(IdStudent);
                var grade = "";

                if (student.IdGrade != null)
                {
                    var result = await _gradeService.GetByIdSaveViewModel(student.IdGrade ?? 0);
                    grade = result.Name;
                }
                var existRegister = await _observationCommentEvaluation.GetBy(x => x.IdStudent.Equals(IdStudent)
                && x.Period.Equals(period) && x.GradeStudent.Equals(grade));

                if (existRegister != null && existRegister.Count > 0)
                {
                    var modelExiist = existRegister.FirstOrDefault();
                    model.Period = period;
                    model.Comment1 = comment1?.Trim() ?? "";
                    model.Comment2 = comment2?.Trim() ?? "";
                    model.IdStudent = IdStudent;
                    model.GradeStudent = grade;
                    model.Id = modelExiist.Id;
                    model.Year = modelExiist.Year;

                    await _observationCommentEvaluation.Update(model, modelExiist.Id);
                }
                else
                {
                    model.Period = period;
                    model.Comment1 = comment1?.Trim() ?? "";
                    model.Comment2 = comment2?.Trim() ?? "";
                    model.IdStudent = IdStudent;
                    model.GradeStudent = grade;
                    model.Year = DateTime.Now.Year;

                    await _observationCommentEvaluation.Add(model);
                }

                generalResponse.Data = "Proceso realizado correctamente !!";
                generalResponse.messages.Add("Proceso realizado correctamente !!");

                return generalResponse;
            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add(ex.Message);
                return generalResponse;

            }
        }

        public async Task<GeneralResponse<string>> UpdateCalification(int achievementId, int score, string period, int idStudent, int selectedGradeId, bool isRp = false, int? rp = null)
        {

            GeneralResponse<string> generalResponse = new();

            try
            {
                var exitEval = await _evaluationsPeriodWithCalification.GetBy(x => x.IdAchievementIndicator.Equals(achievementId)
                && x.Periodo.Equals(period) && x.IdSubject.Equals(selectedGradeId) && x.IdStudent.Equals(idStudent));

                if (exitEval.Count == 0)
                {
                    EvaluationsPeriodWithCalificationSaveViewModel model = new();
                    model.IdSubject = selectedGradeId;
                    model.IdStudent = idStudent;
                    model.IdAchievementIndicator = achievementId;
                    model.Periodo = period;
                    model.Calification = score;
                    
                    if (isRp)
                    {
                        model.RecuperaPedg = rp;
                    }

                    model.Calification = score;
                    model.Year = DateTime.Now.Year;
                    model.GradeStudent = await GetStudentGrade(idStudent);

                    var result = await _evaluationsPeriodWithCalification.Add(model);
                }
                else
                {
                    var model = _mapper.Map<EvaluationsPeriodWithCalificationSaveViewModel>(exitEval.FirstOrDefault());
                    
                    model.Calification = score;
                    
                    if (isRp || rp ==  0)
                    {
                        model.RecuperaPedg = rp;
                    }

                    await _evaluationsPeriodWithCalification.Update(model, model.Id);
                }


                generalResponse.result = true;
                generalResponse.messages.Add("Proceso realizado correctamente !!");

                return generalResponse;
            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add(ex.Message);
                return generalResponse;

            }
        }
        public async Task<GeneralResponse<string>> DeleteCalification(int achievementId,string period, int idStudent, int selectedGradeId)
        {

            GeneralResponse<string> generalResponse = new();

            try
            {
                var exitEval = await _evaluationsPeriodWithCalification.GetBy(x => x.IdAchievementIndicator.Equals(achievementId)
                && x.Periodo.Equals(period) && x.IdSubject.Equals(selectedGradeId) && x.IdStudent.Equals(idStudent));

                if (exitEval.Count > 0)
                {
                    var model=  exitEval.FirstOrDefault();
                    await _evaluationsPeriodWithCalification.Delete(model.Id);
                }
                generalResponse.result = true;
                generalResponse.messages.Add("Proceso realizado correctamente !!");

                return generalResponse;
            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add(ex.Message);
                return generalResponse;

            }
        }
        public async Task<(GeneralResponse<AchievementIndicatorStatusViewModelResponse>, string view)> GetDataReport(int IdStudent, string period, int IdSubject = 0)
        {
            GeneralResponse<AchievementIndicatorStatusViewModelResponse> generalResponse = new();

            try
            {
                var grade = await GetStudentGrade(IdStudent);

                switch (grade)
                {
                    case "Párvulo I":
                        generalResponse = await GetDataStudentReport(IdStudent, period, 0, grade);
                        return (generalResponse, "ParvuloI/ParvuloIReport_6_1");

                    case "Párvulo II":
                        generalResponse = await GetDataStudentReport(IdStudent, period, 0, grade);
                        return (generalResponse, "ParvuloII/ParvuloIIReport");

                    case "Párvulo III":
                        generalResponse = await GetDataStudentReport(IdStudent, period, 0, grade);
                        return (generalResponse, "ParvuloIII/ParvuloIIIReport");

                    case "Pre-Kinder":
                        generalResponse = await GetDataStudentReport(IdStudent, period, 0, grade);
                        return (generalResponse, "Prekinder/PrekinderReport");

                    case "Kinder":
                        generalResponse = await GetDataStudentReport(IdStudent, period, 0, grade);
                        return (generalResponse, "Kinder/KinderReport");

                    case "Prekinder":
                        generalResponse = await GetDataStudentReport(IdStudent, period, 0, grade);
                        return (generalResponse, "Prekinder/PrekinderReport");

                    case "Preprimaria":
                        generalResponse = await GetDataStudentReport(IdStudent, period, 0, grade);
                        return (generalResponse, "Preprimario/PreprimarioReport");

                    case "Segundo":
                        generalResponse = await GetDataStudentReport(IdStudent, period, IdSubject, grade);
                        return (generalResponse, "SecondGrade/SecondGradeReport");
                    case "Tercero":
                        generalResponse = await GetDataStudentReport(IdStudent, period, IdSubject, grade);
                        return (generalResponse, "TerceroGrade/TerceroGradeReport");
                    
                    case "Cuarto":
                        generalResponse = await GetDataStudentReport(IdStudent, period, IdSubject, grade);
                        return (generalResponse, "CuartoGrade/CuartoGradeReport");
                    
                    case "Quinto":
                        generalResponse = await GetDataStudentReport(IdStudent, period, IdSubject, grade);
                        return (generalResponse, "QuintoGrade/QuintoGradeReport");

                    case "Sexto":
                        generalResponse = await GetDataStudentReport(IdStudent, period, IdSubject, grade);
                        return (generalResponse, "SextoGrade/SextoGradeReport");
                    case "Primero Secundaria":
                        generalResponse = await GetDataStudentReport(IdStudent, period, IdSubject, grade);
                        return (generalResponse, "Secundaria_Primero/PrimeroSecundariaReport");

                    default:
                        generalResponse.result = false;
                        generalResponse.messages.Add("El informe para este grado está en proceso de construcción y estará disponible próximamente. " +
                                "¡Gracias por su paciencia!");
                        return (generalResponse, "");
                }
            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add(ex.Message);
                return (generalResponse, "");
            }

        }
        public async Task<string> GetStudentGrade(int IdStudent)
        {
            var student = await _studentServices.GetByIdSaveViewModel(IdStudent);
            var grade = "";

            if (student.IdGrade != null)
            {
                var result = await _gradeService.GetByIdSaveViewModel(student.IdGrade ?? 0);
                grade = result.Name;
            }
            return grade;
        }
        public async Task<List<ClassSelected<int>>> GetSubjectsByTeacher(string IdTeacher, int IdRoom)
        {
            var student = await _userService.GetUserById(IdTeacher);

            var roomTeacherViewModels = await _roomTeacherService.GetBy(x => x.IdTeacher.Equals(IdTeacher) && x.IdRoom.Equals(IdRoom));

            List<ClassSelected<int>> result = new();

            List<ClassSelected<int>> grades = await GetSujects();

            foreach (var teacher in roomTeacherViewModels)
            {
                var selectList = new ClassSelected<int>();
                selectList.text = grades.Where(grade => grade.Id == teacher.IdSuject).FirstOrDefault().text;
                selectList.Id = grades.Where(grade => grade.Id == teacher.IdSuject).FirstOrDefault().Id;

                result.Add(selectList);
            }
            return result;
        }

        #region Private Methods
        private async Task<(AchievementIndicatorStatusViewModelResponse, string view)> GetReport(string grade, int IdStudent, StudentSaveViewModel student, SaveUserViewModel currentUser, bool preview = false, string period = "", int IdSubject = 0)
        {
            AchievementIndicatorStatusViewModelResponse response = new();

            try
            {
                switch (grade)
                {
                    case "Párvulo I":
                        response = await ReportStudent(grade, IdStudent, student, currentUser);
                        return (response, preview ? "ParvuloI/ParvuloIPreviewTemplate" : "ParvuloI/ParvuloITemplate");

                    case "Párvulo II":
                        response = await ReportStudent(grade, IdStudent, student, currentUser);
                        return (response, preview ? "ParvuloII/ParvuloIIPreviewTemplate" : "ParvuloII/ParvuloIITemplate");

                    case "Párvulo III":
                        response = await ReportStudent(grade, IdStudent, student, currentUser);
                        return (response, preview ? "ParvuloIII/ParvuloIIIPreviewTemplate" : "ParvuloIII/ParvuloIIITemplate");

                    case "Pre-Kinder":
                        response = await ReportStudent(grade, IdStudent, student, currentUser);
                        return (response, preview ? "Prekinder/PrekinderPreviewTemplate" : "Prekinder/PrekinderTemplate");

                    case "Kinder":
                        response = await ReportStudent(grade, IdStudent, student, currentUser);
                        return (response, preview ? "Kinder/KinderPreviewTemplate" : "Kinder/KinderTemplate");

                    case "Prekinder":
                        response = await ReportStudent(grade, IdStudent, student, currentUser);
                        return (response, preview ? "Prekinder/PrekinderPreviewTemplate" : "Prekinder/PrekinderTemplate");

                    case "Preprimaria":
                        response = await ReportStudent(grade, IdStudent, student, currentUser);
                        return (response, preview ? "Preprimario/PreprimarioPreviewTemplate" : "Preprimario/PreprimarioTemplate");

                    case "Segundo":
                        response = await ReportStudent(grade, IdStudent, student, currentUser, period, IdSubject, preview);
                        return (response, preview ? "SecondGrade/SecondGradePreviewTemplate" : "SecondGrade/SecondGradeTemplate");
                    
                    case "Tercero":
                        response = await ReportStudent(grade, IdStudent, student, currentUser, period, IdSubject, preview);
                        return (response, preview ? "TerceroGrade/TerceroGradePreviewTemplate" : "TerceroGrade/TerceroGradeTemplate");
                    
                    case "Cuarto":
                        response = await ReportStudent(grade, IdStudent, student, currentUser, period, IdSubject, preview);
                        return (response, preview ? "CuartoGrade/CuartoGradePreviewTemplate" : "CuartoGrade/CuartoGradeTemplate");
                    
                    case "Quinto":
                        response = await ReportStudent(grade, IdStudent, student, currentUser, period, IdSubject, preview);
                        return (response, preview ? "QuintoGrade/QuintoGradePreviewTemplate" : "QuintoGrade/QuintoGradeTemplate");
                    
                    case "Sexto":
                        response = await ReportStudent(grade, IdStudent, student, currentUser, period, IdSubject, preview);
                        return (response, preview ? "SextoGrade/SextoGradePreviewTemplate" : "SextoGrade/SextoGradeTemplate");
                    
                    case "Primero Secundaria":
                        response = await ReportStudent(grade, IdStudent, student, currentUser, period, IdSubject, preview);
                        return (response, preview ? "Secundaria_Primero/SecundariaPrimeroPreviewTemplate" : "Secundaria_Primero/SecundariaPrimeroTemplate");

                    default:
                        return (null, "");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<AchievementIndicatorStatusViewModelResponse> ReportStudent(string grade, int IdStudent, StudentSaveViewModel student, SaveUserViewModel currentUser, string period = "", int IdSubject = 0, bool preview = false)
        {
            AchievementIndicatorStatusViewModelResponse response = new();
            List<AchievementIndicatorStatusViewModel> model = new();

            List<AchievementIndicatorsViewModel> achievementIndicatorsViewModels = new();

            List<EvaluationsPeriodViewModel> evaluationsPeriods = new();
            List<EvaluationsPeriodWithCalificationViewModel> evaluationsPeriodsWithCalifica = new();

            if (new HashSet<string> { "Segundo", "Tercero", "Cuarto", "Quinto", "Sexto" ,}.Contains(grade))
            {
                if (preview)
                {
                    var subjects = await _sujectService.GetByIdSaveViewModel(IdSubject);

                    achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade) && x.CodeSubject.Equals(subjects.Code));

                    var subjectName = await _sujectService.GetByIdSaveViewModel(IdSubject);
                    response.SubjectName = subjectName.Name ?? "";

                    evaluationsPeriodsWithCalifica = await _evaluationsPeriodWithCalification.GetBy(x =>
                    x.IdStudent.Equals(IdStudent)
                    && x.GradeStudent.Equals(grade)
                    && x.IdSubject.Equals(IdSubject) );
                }
                else
                {
                    achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade));
                    
                    evaluationsPeriodsWithCalifica = await _evaluationsPeriodWithCalification.GetBy(x =>
                    x.IdStudent.Equals(IdStudent)
                    && x.GradeStudent.Equals(grade));
                }

                // Consulta para obtener todos los AchievementIndicatorsViewModel e identificar si están en EvaluationsPeriodViewModel
                model = achievementIndicatorsViewModels.Select(indicator => new AchievementIndicatorStatusViewModel
                {
                    Indicator = indicator,                 
                    PeriodosEstados = evaluationsPeriodsWithCalifica
                    .Where(ep => ep.IdAchievementIndicator == indicator.Id)
                    .Select(ep => new PeriodoEstadoViewModel
                    {
                        Periodo = ep.Periodo,
                        Calification = ep.Calification ?? 0
                    }).ToList()

                }).ToList();
            }

            else if (new HashSet<string> { "Primero Secundaria", "Segundo Secundaria", "Tercero Secundaria",
                    "Cuarto Secundaria", "Quinto Secundaria", "Sexto Secundaria" }.Contains(grade))
            {
                if (preview)
                {
                    var subjects = await _sujectService.GetByIdSaveViewModel(IdSubject);

                    achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade) && x.CodeSubject.Equals(subjects.Code));

                    var subjectName = await _sujectService.GetByIdSaveViewModel(IdSubject);
                    response.SubjectName = subjectName.Name ?? "";

                    evaluationsPeriodsWithCalifica = await _evaluationsPeriodWithCalification.GetBy(x =>
                    x.IdStudent.Equals(IdStudent)
                    && x.GradeStudent.Equals(grade)
                    && x.IdSubject.Equals(IdSubject));
                }
                else
                {
                    achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade));

                    evaluationsPeriodsWithCalifica = await _evaluationsPeriodWithCalification.GetBy(x =>
                    x.IdStudent.Equals(IdStudent)
                    && x.GradeStudent.Equals(grade));
                }

                // Consulta para obtener todos los AchievementIndicatorsViewModel e identificar si están en EvaluationsPeriodViewModel
                model = achievementIndicatorsViewModels.Select(indicator => new AchievementIndicatorStatusViewModel
                {
                    Indicator = indicator,
                    PeriodosEstados = evaluationsPeriodsWithCalifica
                    .Where(ep => ep.IdAchievementIndicator == indicator.Id)
                    .Select(ep => new PeriodoEstadoViewModel
                    {
                        Periodo = ep.Periodo,
                        Calification = ep.Calification ?? 0,
                        RecuperaPedg = ep.RecuperaPedg,

                    }).ToList(),
                   

                }).ToList();
            }
            else
            {
                achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade));

                evaluationsPeriods = await _evaluationsPeriodService.GetBy(x => x.IdStudent.Equals(IdStudent)
                && x.GradeStudent.Equals(grade) );

                // Consulta para obtener todos los AchievementIndicatorsViewModel e identificar si están en EvaluationsPeriodViewModel
                model = achievementIndicatorsViewModels.Select(indicator => new AchievementIndicatorStatusViewModel
                {
                    Indicator = indicator,
                    ExistsInEvaluationsPeriod = evaluationsPeriods.Any(ep => ep.IdAchievementIndicator == indicator.Id),
                    PeriodosEstados = evaluationsPeriods
                    .Where(ep => ep.IdAchievementIndicator == indicator.Id)
                    .Select(ep => new PeriodoEstadoViewModel
                    {
                        Periodo = ep.Periodo,
                        Estado = ep.Estado
                    }).ToList()

                }).ToList();
            }               

            response.vmPrinc = model;            

            var allGrades = await _gradeService.GetAllViewModel();            

            if (!string.IsNullOrEmpty(student.Sexo))
            {
                student.SexDes = int.Parse(student.Sexo) == 1 ? "Masculino" : "Femenino";
            }

            if (student.IdGrade != null && student.IdGrade > 0)
            {
                var gradeSelect = allGrades.FirstOrDefault(x => x.Id == student.IdGrade);
                student.GradeDes = gradeSelect?.Name;
            }
            student.Age = _dateAndTimeManage.CalculateAge(student.BornDate);

            if (!string.IsNullOrEmpty(student.IdFather))
            {
                var father = await _userService.GetUserById(student.IdFather);
                student.IdFatherDesc = $"{father?.FirstName} {father?.LastName}";
            }
            else
            {
                var mother = await _userService.GetUserById(student.IdMother);
                student.IdMotherDesc = $"{mother?.FirstName} {mother?.LastName}";
            }

            // Encuentra el índice del grado actual en la lista
            var currentGradeIndex = allGrades.FindIndex(x => x.Id == student.IdGrade);

            // Verifica si existe un grado siguiente en la lista
            if (currentGradeIndex >= 0 && currentGradeIndex < allGrades.Count - 1)
            {
                var nextGrade = allGrades[currentGradeIndex + 1];
                response.NextGradeDes = nextGrade.Name;
            }
            else
            {
                response.NextGradeDes = student.GradeDes ?? "";
            }

            var observationComments = await _observationCommentEvaluation.GetBy(x => 
            x.IdStudent.Equals(IdStudent)
            && x.GradeStudent.Equals(grade) );
            
            var educaModel = await _educationalInstitution.GetAllViewModel();

            response.StudentViewModel = student;
            response.NameTeacher = $"{currentUser.FirstName} {currentUser.LastName}";
            response.EducationalInstitutionViewModel = educaModel.FirstOrDefault() ?? new ViewModels.EducationalInstitution.EducationalInstitutionViewModel();

            var provice = await _provinceDomService.GetByIdSaveViewModel(educaModel.FirstOrDefault().IdProvinceDom ?? 0);

            response.NameProvince = provice.Name;
            response.NameMunicipality = educaModel.FirstOrDefault().NameMunicipality;

            response.vmPrinc = model;
            response.ObservationCommentEvaluationViewModel = observationComments;

            return response;
        }
        private async Task<GeneralResponse<AchievementIndicatorStatusViewModelResponse>> GetDataStudentReport(int IdStudent, string period, int IdSubject = 0, string grade = "")
        {
            GeneralResponse<AchievementIndicatorStatusViewModelResponse> generalResponse = new();

            try
            {
                var student = await _studentServices.GetByIdSaveViewModel(IdStudent);                

                List<AchievementIndicatorStatusViewModel> model = new();
                List<AchievementIndicatorsViewModel> achievementIndicatorsViewModels = new();
                List<EvaluationsDetails> evaluationsDetails = new();
                if (new HashSet<string> { "Segundo", "Tercero", "Cuarto", "Quinto", "Sexto" }.Contains(grade))
                {
                   List<EvaluationsPeriodWithCalificationViewModel> evaluationsPeriodsWithCalifica = await _evaluationsPeriodWithCalification.GetBy(x =>
                   x.IdStudent.Equals(IdStudent)
                   && x.Periodo.Equals(period)
                   && x.GradeStudent.Equals(grade)
                   && x.IdSubject.Equals(IdSubject));
                    
                    var subjects = await _sujectService.GetByIdSaveViewModel(IdSubject);

                    achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade) && x.CodeSubject.Equals(subjects.Code));

                    // Consulta para obtener todos los AchievementIndicatorsViewModel e identificar si están en EvaluationsPeriodViewModel
                    model = achievementIndicatorsViewModels.Select(indicator => new AchievementIndicatorStatusViewModel
                    {
                        Indicator = indicator,
                        Calification = evaluationsPeriodsWithCalifica
                        .Where(ep => ep.IdAchievementIndicator == indicator.Id)
                        .Select(ep => ep.Calification)
                        .FirstOrDefault()
                    }).ToList();
                }

                else if ( new HashSet<string> { "Primero Secundaria", "Segundo Secundaria", "Tercero Secundaria", 
                    "Cuarto Secundaria", "Quinto Secundaria", "Sexto Secundaria" }.Contains(grade) )
                {
                    List<EvaluationsPeriodWithCalificationViewModel> evaluationsPeriodsWithCalifica = await _evaluationsPeriodWithCalification.GetBy(x =>
                    x.IdStudent.Equals(IdStudent)
                    && x.Periodo.Equals(period)
                    && x.GradeStudent.Equals(grade)
                    && x.IdSubject.Equals(IdSubject));

                    var subjects = await _sujectService.GetByIdSaveViewModel(IdSubject);

                    achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade) && x.CodeSubject.Equals(subjects.Code));

                    // Consulta para obtener todos los AchievementIndicatorsViewModel e identificar si están en EvaluationsPeriodViewModel
                    model = achievementIndicatorsViewModels.Select(indicator => new AchievementIndicatorStatusViewModel
                    {
                        Indicator = indicator,

                        Calification = evaluationsPeriodsWithCalifica
                        .Where(ep => ep.IdAchievementIndicator == indicator.Id)
                        .Select(ep => ep.Calification)
                        .FirstOrDefault(),

                        Periodo = period,

                        RecuperaPedg = evaluationsPeriodsWithCalifica
                        .Where(ep => ep.IdAchievementIndicator == indicator.Id)
                        .Select(ep => ep.RecuperaPedg)
                        .FirstOrDefault() ?? 0,

                    }).ToList();
                }

                else
                        {
                    achievementIndicatorsViewModels = await _indicatorsService.GetBy(x => x.AssociatedGrades.Equals(grade));

                    List<EvaluationsPeriodViewModel> evaluationsPeriods = await _evaluationsPeriodService.GetBy(x =>
                    x.IdStudent.Equals(IdStudent)
                    && x.Periodo.Equals(period) );

                    // Consulta para obtener todos los AchievementIndicatorsViewModel e identificar si están en EvaluationsPeriodViewModel
                    model = achievementIndicatorsViewModels.Select(indicator => new AchievementIndicatorStatusViewModel
                    {
                        Indicator = indicator,
                        ExistsInEvaluationsPeriod = evaluationsPeriods.Any(ep => ep.IdAchievementIndicator == indicator.Id),
                        Estado = evaluationsPeriods
                        .Where(ep => ep.IdAchievementIndicator == indicator.Id)
                        .Select(ep => ep.Estado)
                        .FirstOrDefault() ?? "",
                        Periodo = period,
                    }).ToList();

                    int indicatorIndex = 0;
                    var evaluations = GenerateEvaluations(grade);

                    foreach (var evaItem in evaluations)
                    {
                        evaItem.Indicators = model.Skip(indicatorIndex).Take(evaItem.IndicatorCount).ToList();
                        indicatorIndex += evaItem.IndicatorCount;
                        evaluationsDetails.Add(evaItem);
                    }
                }

                AchievementIndicatorStatusViewModelResponse response = new();
                response.vmPrinc = model;
                response.EvaluationsDetails = evaluationsDetails;

                var existRegister = await _observationCommentEvaluation.GetBy(x => 
                x.IdStudent.Equals(IdStudent)
                && x.Period.Equals(period)
                && x.Year.Equals(DateTime.Now.Year)
                && x.GradeStudent.Equals(grade)
                );

                if (existRegister != null && existRegister.Count > 0)
                {
                    var modelResp = existRegister.FirstOrDefault();
                    response.Comment1 = modelResp?.Comment1 ?? "";
                    response.Comment2 = modelResp?.Comment2 ?? "";
                }

                generalResponse.Data = response;
                return generalResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<List<ClassSelected<string>>> GetTeachers(int idRoom)
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
        private async Task<List<ClassSelected<int>>> GetSujects()
        {

            var grades = await _sujectService.GetAllViewModel();


            List<ClassSelected<int>> targetResult = new List<ClassSelected<int>>();

            foreach (var grade in grades)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = grade.Id,
                    text = $"{grade.Name}",
                };

                targetResult.Add(classSelected);
            }
            return targetResult;
        }

        #region Data

        #region Kinder

        private List<(string Title, string SubTitle, int IndicatorCount)> rawDataPrekinder = new ()
        {
            ("Competencia Fundamental: Comunicativa", "Comunicativa", 3),
            ("Dominio Artístico y Creativo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Psicomotor y de Salud", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Descubrimiento del mundo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Cognitivo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Comunicativo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Socioemocional", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Artístico y Creativo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Psicomotor y de Salud", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Descubrimiento del mundo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Cognitivo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Competencias Fundamentales", "Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Socioemocional", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Artístico y Creativo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Psicomotor y de Salud", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Descubrimiento del Mundo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Cognitivo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Comunicativo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 4)
        };

        private List<(string Title, string SubTitle, int IndicatorCount)> rawDatakinder = new()
        {
            ("Competencia Fundamental: Comunicativa", "Comunicativa", 3),
            ("Dominio Artístico y Creativo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Psicomotor y de Salud", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Descubrimiento del mundo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Cognitivo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Comunicativo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Socioemocional", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Artístico y Creativo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Psicomotor y de Salud", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Descubrimiento del mundo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Cognitivo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Comunicativo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica", 3),
            ("Dominio Socioemocional", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Artístico y Creativo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Psicomotor y de Salud", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Descubrimiento del Mundo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Cognitivo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Comunicativo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3)
        };

        #endregion

        #region Parvulo I, II, III

        private List<(string Title, string SubTitle, int IndicatorCount)> rawDataParvulo01 = new()
        {
            ("Ámbito de Experiencia (Párvulo I 45 días a 6 meses):", "Relaciones socioafectivas, identidad y autonomía", 5),
            ("Ámbito de Experiencia: (Párvulo I 45 días a 6 meses)", "Lenguaje e interacción", 4),
            ("Ámbito de Experiencia: (Párvulo I 45 días a 6 meses)", "Descubrimiento de su cuerpo y su relación con el entorno", 14),

            ("Ámbito de Experiencia: (Párvulo I 6 meses a 1 año)", "Relaciones socioafectivas, identidad y autonomía", 9),
            ("Ámbito de Experiencia: (Párvulo I 6 meses a 1 año)", "Lenguaje e interacción", 6),
            ("Ámbito de Experiencia: (Párvulo I 6 meses a 1 año)", "Descubrimiento de su cuerpo y su relación con el entorno", 17)
        };

        private List<(string Title, string SubTitle, int IndicatorCount)> rawDataParvulo02 = new()
        {
            ("Ámbito de Experiencia:", "Relaciones socioafectivas, identidad y autonomía", 8),
            ("Ámbito de Experiencia:", "Lenguaje e interacción", 8),
            ("Ámbito de Experiencia:", "Descubrimiento de su cuerpo y su relación con el entorno", 6)
        };

        private List<(string Title, string SubTitle, int IndicatorCount)> rawDataParvulo03 = new()
        {
            ("Ámbito de Experiencia:", "Socio afectiva, identidad y autonomía", 12),
            ("Ámbito de Experiencia:", "Descubrimiento de su cuerpo y su relación con el entorno", 17),
            ("Ámbito de Experiencia:", "Lenguaje e interacción", 18),
        };

        #endregion

        private List<(string Title, string SubTitle, int IndicatorCount)> rawDataPreprimario = new()
        {
            ("Competencia Fundamental: Comunicativa", "Comunicativa", 3),
            ("Dominio Artístico y Creativo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Psicomotor y de Salud", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Descubrimiento del mundo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Cognitivo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Comunicativo", "Competencia Fundamental: Comunicativa", 3),
            ("Dominio Socioemocional", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Artístico y Creativo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Psicomotor y de Salud", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Descubrimiento del mundo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Cognitivo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica.", 3),
            ("Dominio Comunicativo", "Competencias Fundamentales: Pensamiento Lógico, Creativo y Crítico; Resolución de Problemas; Tecnológica y Científica", 3),
            ("Dominio Socioemocional", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Artístico y Creativo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Psicomotor y de Salud", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Descubrimiento del Mundo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3),
            ("Dominio Cognitivo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 2),
            ("Dominio Comunicativo", "Competencias Fundamentales: Ética y Ciudadana; Desarrollo Personal y Espiritual; Ambiental y de la Salud", 3)
        };

        private List<EvaluationsDetails> GenerateEvaluations(string grade)
        {
            var response = new List<EvaluationsDetails>();

            switch (grade)
            {
                case "Párvulo I":
                    response = rawDataParvulo01
                    .Select((data, index) => new EvaluationsDetails
                    {
                        Id = $"EVAL{index + 1:D3}",
                        Title = data.Title,
                        SubTitle = data.SubTitle,
                        IndicatorCount = data.IndicatorCount
                    }).ToList();
                    return response;

                case "Párvulo II":
                    response = rawDataParvulo02
                    .Select((data, index) => new EvaluationsDetails
                    {
                        Id = $"EVAL{index + 1:D3}",
                        Title = data.Title,
                        SubTitle = data.SubTitle,
                        IndicatorCount = data.IndicatorCount
                    }).ToList();
                    return response;

                case "Párvulo III":
                    response = rawDataParvulo03
                    .Select((data, index) => new EvaluationsDetails
                    {
                        Id = $"EVAL{index + 1:D3}",
                        Title = data.Title,
                        SubTitle = data.SubTitle,
                        IndicatorCount = data.IndicatorCount
                    }).ToList();
                    return response;

                case "Pre-Kinder":
                    response = rawDataPrekinder
                    .Select((data, index) => new EvaluationsDetails
                    {
                        Id = $"EVAL{index + 1:D3}",
                        Title = data.Title,
                        SubTitle = data.SubTitle,
                        IndicatorCount = data.IndicatorCount
                    }).ToList();
                    return response;

                case "Kinder":
                    response = rawDatakinder
                    .Select((data, index) => new EvaluationsDetails
                    {
                        Id = $"EVAL{index + 1:D3}",
                        Title = data.Title,
                        SubTitle = data.SubTitle,
                        IndicatorCount = data.IndicatorCount
                    }).ToList();
                    return response;

                case "Preprimaria":
                    response = rawDataPreprimario
                    .Select((data, index) => new EvaluationsDetails
                    {
                        Id = $"EVAL{index + 1:D3}",
                        Title = data.Title,
                        SubTitle = data.SubTitle,
                        IndicatorCount = data.IndicatorCount
                    }).ToList();
                    return response;
            }

            return response;
        }

        #endregion

        #endregion

    }
}

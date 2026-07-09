using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MontessoriSystem.Core.Application.Helpers;
using MontessoriSystem.Core.Application.Helpers.DataBase;
using MontessoriSystem.Core.Application.Helpers.Date;
using MontessoriSystem.Core.Application.Helpers.ImportAndExport;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.Helpers.NewFolder;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.Services.Adjunto;
using MontessoriSystem.Core.Application.Services.Attendance;
using MontessoriSystem.Core.Application.Services.CustomInstitutionService;
using MontessoriSystem.Core.Application.Services.General;
using MontessoriSystem.Core.Application.Services.Reports;
using MontessoriSystem.Core.Application.Services.Room;
using MontessoriSystem.Core.Application.Services.Students;
using MontessoriSystem.Core.Application.Services.Suject;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Domain.Settings;
using MontessoriSystem.Core.Domain.Settings.Grades;
using MontessoriSystem.Core.Domain.Settings.Period;
using System.Reflection;

namespace MontessoriSystem.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddCoreApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IStudentServices, StudentService>();
            services.AddTransient<ISectionServices, SectionService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdjuntoServices, AdjuntoServices>();
            services.AddTransient<ITipoAdjuntoService, TipoAdjuntoServices>();
            services.AddTransient<IEducationalInstitutionService, EducationalInstitutionService>();
            services.AddTransient<IFileServices, FileServices>();
            services.AddTransient<ITeachingPeriodsService, TeachingPeriodsService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<ISujectService, SujectService>();
            services.AddTransient<IRoomScheduleService, RoomScheduleService>();
            services.AddTransient<IGradeService, GradeService>();
            services.AddTransient<IRoomTeacherService, RoomTeacherService>();           
            services.AddTransient<IProvinceDomService, ProvinceDomService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IGroupGradeService, GroupGradeService>();
            services.AddTransient<IEducationalLevelService, EducationalLevelService>();
            services.AddTransient<IProfessionsService, ProfessionsService>();
            services.AddTransient<ISpecializationService, SpecializationService>();
            services.AddTransient<ITitlesAchievedsService, TitlesAchievedService>();
            services.AddTransient<IMaterialStatusService, MaritalStatuService>();
            services.AddTransient<INacionalityService, NacionalityService>();
            services.AddTransient<IRelationshipPersonService, RelationshipPersonService>();
            services.AddTransient<IAttendanceService, AttendanceService>();
            services.AddTransient<ITypeRegisterService, TypeRegisterService>();
            services.AddTransient<IInstitutionalCenterUsersService, InstitutionalCenterUsersService>();
            services.AddTransient<IUserManagementService, UserManagementService>();
            services.AddTransient<ISelectionService, SelectionService>();
            services.AddTransient<IInitDataService, InitDataService>();
            services.AddTransient<IAchievementIndicatorsService, AchievementIndicatorsService>();
            services.AddTransient<IEvaluationsPeriodService, EvaluationsPeriodService>();
            services.AddTransient<IObservationCommentEvaluationService, ObservationCommentEvaluationService>();
            services.AddTransient<IReportsCustomService, ReportsCustomService>();
            services.AddTransient<IEvaluationsPeriodWithCalificationService, EvaluationsPeriodWithCalificationService>();
            services.AddTransient<IStudentManagementService, StudentManageServices>();
            services.AddTransient<ICustAdjuntoService, CustomAdjuntoService>();
            services.AddTransient<ICustomRoomService, CustomRoomService>();
            services.AddTransient<ICustomRoomTeacherService, CustomRoomTeacherService>();
            services.AddTransient<ICustomSujectService, CustomSujectService>();
            services.AddTransient<ICustomInstitutionService, CustomInstitutionService>();
            services.AddTransient<ICustomAttendanceServices, CustomAttendanceServices>();
            services.AddTransient<ICivilStatusService, CivilStatusServices>();

            #region Helpers
            services.AddTransient<IValidateInfoDataBase, ValidateInfoDataBase>();
            services.AddTransient<IDateAndTimeManage, DateAndTimeManage>();
            services.AddTransient<IReportAttendance, ReportAttendanceHelper>();
            services.AddTransient<IExcelManager, ExcelManager>();
            services.AddTransient<IInitDataHelper, InitDataHelper>();
            #endregion


            services.Configure<EducationalPeriod>(configuration.GetSection("EducationalPeriod"));
            services.Configure<EducationalPeriodCalifict>(configuration.GetSection("EducationalPeriodCalifict"));
            services.Configure<IMG_ParvuloIII>(configuration.GetSection("ImageBase64Pdf:IMG_ParvuloIII"));
            services.Configure<Prekinder>(configuration.GetSection("ImageBase64Pdf:Prekinder"));
            services.Configure<Kinder>(configuration.GetSection("ImageBase64Pdf:Kinder"));
            services.Configure<Preprimario>(configuration.GetSection("ImageBase64Pdf:Preprimario"));
            services.Configure<SecondGrade>(configuration.GetSection("ImageBase64Pdf:SecondGrade"));
            services.Configure<TerceroGrade>(configuration.GetSection("ImageBase64Pdf:TerceroGrade"));
            services.Configure<CuartoGrade>(configuration.GetSection("ImageBase64Pdf:CuartoGrade"));
            services.Configure<QuintoGrade>(configuration.GetSection("ImageBase64Pdf:QuintoGrade"));
            services.Configure<SextoGrade>(configuration.GetSection("ImageBase64Pdf:SextoGrade"));

            services.Configure<SecundariaPrimero>(configuration.GetSection("ImageBase64Pdf:SecundariaPrimero"));

            #endregion
        }
    }
}

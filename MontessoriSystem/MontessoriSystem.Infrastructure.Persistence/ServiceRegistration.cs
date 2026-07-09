using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Core.Domain.Settings;
using MontessoriSystem.Infrastructure.Persistence.Contexts;
using MontessoriSystem.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context
            services.AddDbContext<ApplicationContext>(options =>
            {

                string connectionString = Environment.GetEnvironmentVariable("DefaultConnection")?.Trim() ?? "";

                // Si la variable de entorno no está definida, usa la de appsettings.json
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }
                try
                {
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                    throw;
                }

            }, ServiceLifetime.Transient);
            #endregion

            #region Dependency inyeccion

            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IAdjuntoRepository, AdjuntoRepository>();
            services.AddTransient<ISectionRepository, SectionRepository>();
            services.AddTransient<ITipoAdjuntoRepository, TipoAdjuntoRepository>();
            services.AddTransient<IEducationalInstitutionRepository, EducationalInstitutionRepository>();
            services.AddTransient<ITeachingPeriodsRepository, TeachingPeriodsRepository>();
            services.AddTransient<IRoomRespository, RoomRepository>();
            services.AddTransient<ISujectRepository, SujectRepository>();
            services.AddTransient<IRoomSchedule, RoomScheduleRepository>();
            services.AddTransient<IGradeRepository, GradeRepository>();
            services.AddTransient<IRoomTeacherRepository, RoomTeacherRepository>();
            services.AddTransient<IProvinceDomRepository, ProvinceDomRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IGroupGradeRepository, GroupGradeRepository>();
            services.AddTransient<IEducationalLevelRepository, EducationalLevelRepository>();
            services.AddTransient<IProfessionsRepository, ProfessionsRepository>();
            services.AddTransient<ISpecializationRepository, SpecializationRepository>();
            services.AddTransient<ITitlesAchievedRepository, TitlesAchievedRepository>();
            services.AddTransient<IMaritalStatusRepository, MaritalStatuRepository>();
            services.AddTransient<INationalityRepository, NationalityRepsository>();
            services.AddTransient<IRelationshipPersonRepository, RelationshipPersonRepository>();
            services.AddTransient<IAttendanceRepository, AttendanceRepository>();
            services.AddTransient<ITypeRegisterRepository, TypeRegisterRepository>();
            services.AddTransient<IInstitutionalCenterUsersRepository, InstitutionalCenterUsersRepository>();
            services.AddTransient<IInitDataRepository, InitDataRepository>();
            services.AddTransient<IAchievementIndicatorsRepository, AchievementIndicatorsRepository>();
            services.AddTransient<IEvaluationsPeriodRepository, EvaluationsPeriodRepository>();
            services.AddTransient<IObservationCommentEvaluationRepository, ObservationCommentEvaluationRepository>();            
            services.AddTransient<IEvaluationsPeriodWithCalificationRepository, EvaluationsPeriodWithCalificationRepository>();            
            services.AddTransient<ICivilStatusRepository, CivilStatusRepository>();            
           
            services.AddTransient<IUserManagementService, UserManagementService>();            

            #endregion
        
        
        }
    }

}

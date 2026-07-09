using AutoMapper;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.Specialization;
using MontessoriSystem.Core.Application.ViewModels.AchievementIndicators;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using MontessoriSystem.Core.Application.ViewModels.Attendance;
using MontessoriSystem.Core.Application.ViewModels.CivilStatus;
using MontessoriSystem.Core.Application.ViewModels.Department;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriod;
using MontessoriSystem.Core.Application.ViewModels.EvaluationsPeriodWithCalification;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using MontessoriSystem.Core.Application.ViewModels.Group;
using MontessoriSystem.Core.Application.ViewModels.GroupGrade;
using MontessoriSystem.Core.Application.ViewModels.InitData;
using MontessoriSystem.Core.Application.ViewModels.InstitutionalCenterUsers;
using MontessoriSystem.Core.Application.ViewModels.MaritalStatus;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Application.ViewModels.ObservationCommentEvaluation;
using MontessoriSystem.Core.Application.ViewModels.Professions;
using MontessoriSystem.Core.Application.ViewModels.Province;
using MontessoriSystem.Core.Application.ViewModels.RelationshipPerson;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Application.ViewModels.RoomSchedule;
using MontessoriSystem.Core.Application.ViewModels.RoomTeacher;
using MontessoriSystem.Core.Application.ViewModels.Section;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using MontessoriSystem.Core.Application.ViewModels.TeachingPeriods;
using MontessoriSystem.Core.Application.ViewModels.TipoAdjunto;
using MontessoriSystem.Core.Application.ViewModels.TitlesAchieved;
using MontessoriSystem.Core.Application.ViewModels.TypeRegister;
using MontessoriSystem.Core.Application.ViewModels.User;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {

        public GeneralProfile()
        {
            #region Student

            CreateMap<Student, StudentViewModel>()
                .ReverseMap();

            CreateMap<Student, StudentSaveViewModel>()
                .ReverseMap();
            CreateMap<StudentViewModel, StudentSaveViewModel>()
                .ReverseMap();

            #endregion

            #region Section

            CreateMap<Section, SectionViewModel>()
                .ReverseMap();

            CreateMap<Section, SectionSaveViewModel>()
                .ReverseMap();

            #endregion
            
            #region Login

            CreateMap<AuthenticationResponse, LoginViewModel>()
                .ForMember(x => x.HasError, x=>x.Ignore())
                .ForMember(x => x.Error, x=>x.Ignore())
                .ReverseMap();
            
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, x=>x.Ignore())
                .ForMember(x => x.Error, x=>x.Ignore())
                .ReverseMap();

            CreateMap<ForgotPassswordRequest, ForgotPassWordViewModel>()
                .ReverseMap(); 
            
            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ReverseMap();

              CreateMap<ForgotPassswordRequest, ForgotPassWordViewModel>()
                .ReverseMap();

            CreateMap<AuthenticationResponse, LoginResponse>()
               .ReverseMap(); 
            
            CreateMap<RegisterRequest, SaveUserViewModel>()
                 .ForMember(x => x.HasError, x => x.Ignore())
                .ForMember(x => x.Error, x => x.Ignore())
               .ReverseMap();
               

            #endregion
       
            #region Adjunto
            CreateMap<AdjuntoViewModel, Adjunto>()
                .ReverseMap();
             CreateMap<SaveAdjuntoViewModel, Adjunto>()
                .ReverseMap();

               

            #endregion

            #region Tipo  Adjunto
            CreateMap<TipoAdjuntoViewModel, TipoAdjunto>()
                .ReverseMap();
             CreateMap<SaveTipoAdjuntoViewModel, TipoAdjunto>()
                .ReverseMap();

            #endregion

            #region Usuarios
            CreateMap<AppliciationUserDTO,SaveUserViewModel >()
                .ForMember(x=>x.HasError, x=>x.Ignore())
                .ForMember(x=>x.Error, x=>x.Ignore())   
                .ReverseMap();
             CreateMap<AppliciationUserDTO, UserViewModel>()
                .ForMember(x=>x.HasError, x=>x.Ignore())
                .ForMember(x=>x.Error, x=>x.Ignore())   
                .ReverseMap();
             CreateMap<SaveUserViewModel, UserViewModel>()
                .ReverseMap();

            #endregion

            #region EducationalInstitution
            CreateMap<SaveEducationalInstitutionViewModel, EducationalInstitution>() 
                .ReverseMap();
            CreateMap<EducationalInstitutionViewModel, EducationalInstitution>() 
                .ReverseMap();
            CreateMap<EducationalInstitutionViewModel, SaveEducationalInstitutionViewModel>() 
                .ReverseMap();
            #endregion

            #region TeachingPeriods

            CreateMap<TeachingPeriodsSaveViewModel, TeachingPeriods>() 
                .ReverseMap();
            CreateMap<TeachingPeriodsViewModel, TeachingPeriods>() 
                .ReverseMap();
            CreateMap<TeachingPeriodsViewModel, TeachingPeriodsSaveViewModel>() 
                .ReverseMap();

            #endregion

            #region Room

            CreateMap<RoomSaveViewModel, Room>()
                .ReverseMap();
            CreateMap<RoomoViewModel, Room>()
                .ReverseMap();

            CreateMap<RoomSaveViewModel, RoomoViewModel>()
                .ReverseMap();

              CreateMap<RoomScheduleViewModel, RoomSchedule>()
                .ReverseMap();
              CreateMap<SaveViewModelRoomSchedule, RoomSchedule>()
                .ReverseMap();
             CreateMap<SaveViewModelRoomSchedule, RoomScheduleViewModel>()
                .ReverseMap();


            CreateMap<RoomTeacherSaveViewModel, RoomTeacher>()
               .ReverseMap();

            CreateMap<RoomTeacherViewModel, RoomTeacher>()
               .ReverseMap(); ;

             CreateMap<RoomTeacherSaveViewModel, RoomTeacherViewModel>()
                .ReverseMap();

            #endregion

            #region Suject

            CreateMap<SujectSaveViewModel, Suject>()
                .ReverseMap();
            CreateMap<SujectViewModel, Suject>()
                .ReverseMap();
            CreateMap<SujectViewModel, SujectSaveViewModel>()
                .ReverseMap();

            #endregion

            #region Grades

            CreateMap<GradeSaveViewModel, Grade>()
                .ReverseMap();
            CreateMap<GradeViewModel, Grade>()
                .ReverseMap();
            CreateMap<GradeSaveViewModel, GradeViewModel>()
                .ReverseMap();

            #endregion

            #region Province

            CreateMap<ProvinceDomViewModel, ProvinceDom>()
                .ReverseMap();
            CreateMap<ProvinceDomSaveViewModel, ProvinceDom>()
                .ReverseMap();
            CreateMap<ProvinceDomViewModel, ProvinceDomSaveViewModel>()
                .ReverseMap();

            CreateMap<DepartmentViewModel, Department>()
                .ReverseMap();
            CreateMap<DepartmentSaveViewModel, Department>()
                .ReverseMap();
            CreateMap<DepartmentViewModel, DepartmentSaveViewModel>()
                .ReverseMap();

            #endregion

            #region Group

            CreateMap<GroupViewModel, GroupCenter>()
                .ReverseMap();
            CreateMap<GroupSaveViewModel, GroupCenter>()
                .ReverseMap();
            CreateMap<GroupViewModel, GroupSaveViewModel>()
                .ReverseMap();

              CreateMap<GroupGradeSaveViewModel, GroupGrade>()
                .ReverseMap();
            CreateMap<GroupGradeViewModel, GroupGrade>()
                .ReverseMap();
            CreateMap<GroupGradeSaveViewModel, GroupGradeViewModel>()
                .ReverseMap();

            #endregion

            #region EducationalLevel

            CreateMap<EducationalLevelSaveViewModel, EducationalLevel>()
               .ReverseMap();
            CreateMap<EducationalLevelViewModel, EducationalLevel>()
                .ReverseMap();
            CreateMap<EducationalLevelViewModel, EducationalLevelSaveViewModel>()
                .ReverseMap();

            #endregion

            #region Professions

            CreateMap<ProfessionsViewModel, Professions>()
               .ReverseMap();
            CreateMap<ProfessionsSaveViewModel, Professions>()
                .ReverseMap();
            CreateMap<ProfessionsViewModel, ProfessionsViewModel>()
                .ReverseMap();

            #endregion

            #region Specialization

            CreateMap<SpecializationViewModel, Specializations>()
               .ReverseMap();
            CreateMap<SpecializationsSaveViewModel, Specializations>()
                .ReverseMap();
            CreateMap<SpecializationsSaveViewModel, SpecializationViewModel>()
                .ReverseMap();

            #endregion

            #region TitlesAchieved

            CreateMap<TitlesAchievedViewModel, TitlesAchieved>()
               .ReverseMap();
            CreateMap<TitlesAchievedSaveViewModel, TitlesAchieved>()
                .ReverseMap();
            CreateMap<TitlesAchievedViewModel, TitlesAchievedSaveViewModel>()
                .ReverseMap();

            #endregion

            #region MaritalStatuService

            CreateMap<MaritalStatusSaveViewModel, MaritalStatus>()
               .ReverseMap();
            CreateMap<MaritalStatusViewModel, MaritalStatus>()
                .ReverseMap();
            CreateMap<MaritalStatusSaveViewModel, MaritalStatusViewModel>()
                .ReverseMap();

            #endregion

            #region Nationality

            CreateMap<NationalityViewModel, Nationality>()
               .ReverseMap();
            CreateMap<NationalitySaveViewModel, Nationality>()
                .ReverseMap();
            CreateMap<NationalityViewModel, NationalitySaveViewModel>()
                .ReverseMap();

            #endregion

            #region RelationshipPerson

            CreateMap<RelationshipPersonSaveViewModel, RelationshipPerson>()
               .ReverseMap();
            CreateMap<RelationshipPersonViewModel, RelationshipPerson>()
                .ReverseMap();
            CreateMap<RelationshipPersonSaveViewModel, RelationshipPersonViewModel>()
                .ReverseMap();

            #endregion

            #region RelationshipAttendancePerson

            CreateMap<AttendanceViewModel, Attendance>()
               .ReverseMap();
            CreateMap<AttendanceSaveViewModel, Attendance>()
                .ReverseMap();
            CreateMap<AttendanceViewModel, AttendanceSaveViewModel>()
                .ReverseMap();

            #endregion

            #region TypeRegister

            CreateMap<TypeRegisterSaveViewModel, TypeRegister>()
               .ReverseMap();
            CreateMap<TypeRegisterViewModel, TypeRegister>()
                .ReverseMap();
            CreateMap<TypeRegisterSaveViewModel, TypeRegisterViewModel>()
                .ReverseMap();

            #endregion

            #region TypeRegister

            CreateMap<InstitutionalCenterUsersSaveViewModel, InstitutionalCenterUsers>()
               .ReverseMap();
            CreateMap<InstitutionalCenterUsersViewModel, InstitutionalCenterUsers>()
                .ReverseMap();
            CreateMap<InstitutionalCenterUsersSaveViewModel, InstitutionalCenterUsersViewModel>()
                .ReverseMap();

            #endregion

            #region InitData

            CreateMap<InitDataSaveViewModel, InitData>()
               .ReverseMap();
            CreateMap<InitDataViewModel, InitData>()
                .ReverseMap();
            CreateMap<InitDataViewModel, InitDataSaveViewModel>()
                .ReverseMap();

            #endregion

            #region EvaluationsPeriod

            CreateMap<EvaluationsPeriodViewModel, EvaluationsPeriod>()
               .ReverseMap();
            CreateMap<SaveEvaluationsPeriodViewModel, EvaluationsPeriod>()
                .ReverseMap();
            CreateMap<EvaluationsPeriodViewModel, SaveEvaluationsPeriodViewModel>()
                .ReverseMap();

            #endregion

            #region AchievementIndicatorsViewModel

            CreateMap<AchievementIndicatorsViewModel, AchievementIndicators>()
               .ReverseMap();
            CreateMap<SaveAchievementIndicatorsViewModel, AchievementIndicators>()
                .ReverseMap();
            CreateMap<AchievementIndicatorsViewModel, SaveAchievementIndicatorsViewModel>()
                .ReverseMap();

            #endregion

            #region ObservationCommentEvaluation

            CreateMap<ObservationCommentEvaluationViewModel, ObservationCommentEvaluation>()
               .ReverseMap();
            CreateMap<SaveViewModelObservationCommentEvaluation, ObservationCommentEvaluation>()
                .ReverseMap();
            CreateMap<SaveViewModelObservationCommentEvaluation, ObservationCommentEvaluationViewModel>()
                .ReverseMap();

            #endregion

            #region EvaluationsPeriodWithCalification

            CreateMap<EvaluationsPeriodWithCalificationSaveViewModel, EvaluationsPeriodWithCalification>()
               .ReverseMap();
            CreateMap<EvaluationsPeriodWithCalificationViewModel, EvaluationsPeriodWithCalification>()
                .ReverseMap();
            CreateMap<EvaluationsPeriodWithCalificationSaveViewModel, EvaluationsPeriodWithCalificationViewModel>()
                .ReverseMap();

            #endregion

            #region Civil Status

            CreateMap<SaveCivilStatusViewModel, MaritalStatus>()
               .ReverseMap();
            CreateMap<CivilStatusViewModel, MaritalStatus>()
                .ReverseMap();
            CreateMap<SaveCivilStatusViewModel, MaritalStatus>()
                .ReverseMap();

            #endregion

        }
    }
}

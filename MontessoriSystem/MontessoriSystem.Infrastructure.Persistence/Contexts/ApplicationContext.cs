using Microsoft.EntityFrameworkCore;
using MontessoriSystem.Core.Domain.Common;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<TipoAdjunto> TipoAdjuntos { get; set; }
        public DbSet<Adjunto> Adjuntos { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<EducationalInstitution> EducationalInstitution { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<RoomSchedule> RoomSchedule { get; set; }
        public DbSet<RoomTeacher> RoomTeacher { get; set; }
        public DbSet<Suject> Suject { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<TeachingPeriods> SelectivePeriod { get; set; }
        public DbSet<ProvinceDom> ProvinceDom { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<GroupCenter> GroupCenter { get; set; }
        public DbSet<GroupGrade> GroupGrade { get; set; }
        public DbSet<EducationalLevel> EducationalLevel { get; set; }
        public DbSet<Professions> Professions { get; set; }
        public DbSet<Specializations> Specializations { get; set; }
        public DbSet<TitlesAchieved> TitlesAchieved { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }
        public DbSet<Nationality> Nationality { get; set; }
        public DbSet<RelationshipPerson> RelationshipPerson { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<TypeRegister> TypeRegister { get; set; }
        public DbSet<InstitutionalCenterUsers> InstitutionalCenterUsers { get; set; }
        public DbSet<ObservationCommentEvaluation> ObservationCommentEvaluation { get; set; }
        public DbSet<AchievementIndicators> AchievementIndicators { get; set; }
        public DbSet<EvaluationsPeriod> EvaluationsPeriod { get; set; }
        public DbSet<EvaluationsPeriodWithCalification> EvaluationsPeriodWithCalification { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken= new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API

            #region Tables

            modelBuilder.Entity<Student>()
                .ToTable("Student");

           modelBuilder.Entity<InitData>()
                .ToTable("InitData");
            
            modelBuilder.Entity<TipoAdjunto>()
                .ToTable("TipoAdjunto");

            modelBuilder.Entity<Adjunto>()
                .ToTable("Adjunto");

            modelBuilder.Entity<Section>()
                .ToTable("Section");

            modelBuilder.Entity<TeachingPeriods>()
                .ToTable("TeachingPeriods");

            modelBuilder.Entity<Suject>()
                .ToTable("Suject");

            modelBuilder.Entity<Room>()
                .ToTable("Room");
              modelBuilder.Entity<RoomSchedule>()
                .ToTable("RoomSchedule");

             modelBuilder.Entity<RoomTeacher>()
                .ToTable("RoomTeacher");

            modelBuilder.Entity<EducationalInstitution>()
                .ToTable("EducationalInstitution");

            modelBuilder.Entity<ProvinceDom>()
                .ToTable("ProvinceDom");

             modelBuilder.Entity<Department>()
                .ToTable("Department");

            modelBuilder.Entity<GroupGrade>()
                .ToTable("GroupGrade");

              modelBuilder.Entity<GroupCenter>()
                .ToTable("Group");

              modelBuilder.Entity<EducationalLevel>()
                .ToTable("EducationalLevel");

             modelBuilder.Entity<Professions>()
                .ToTable("Profession");

            modelBuilder.Entity<Specializations>()
                .ToTable("Specializations");

             modelBuilder.Entity<TitlesAchieved>()
                .ToTable("TitlesAchieved");

            modelBuilder.Entity<MaritalStatus>()
                .ToTable("MaritalStatus");

             modelBuilder.Entity<Nationality>()
                .ToTable("Nationality");

            modelBuilder.Entity<RelationshipPerson>()
                .ToTable("RelationshipPerson");

            modelBuilder.Entity<Attendance>()
                .ToTable("Attendance");

             modelBuilder.Entity<TypeRegister>()
                .ToTable("TypeRegister");

             modelBuilder.Entity<InstitutionalCenterUsers>()
                .ToTable("InstitutionalCenterUsers");

            modelBuilder.Entity<EvaluationsPeriod>()
                .ToTable("EvaluationsPeriod");

             modelBuilder.Entity<AchievementIndicators>()
                .ToTable("AchievementIndicators");

              modelBuilder.Entity<EvaluationsPeriodWithCalification>()
                .ToTable("EvaluationsPeriodWithCalification");

            #endregion

            #region Primary keys

            modelBuilder.Entity<Student>()
                .HasKey(x => x.Id);
            
            modelBuilder.Entity<TipoAdjunto>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Adjunto>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Section>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<TypeRegister>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<InstitutionalCenterUsers>()
                .HasKey(sc => new { sc.CenterId, sc.UserId });

            #endregion

            #region Relationships

            #region Student

            modelBuilder.Entity<Student>()
            .HasOne(s => s.TypeRegister)
            .WithMany(tr => tr.Student)
            .HasForeignKey(s => s.IdTypeRegister)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Student>()
            .HasOne(s => s.Grade)
            .WithMany(tr => tr.Students)
            .HasForeignKey(s => s.IdGrade)
            .OnDelete(DeleteBehavior.SetNull);


            #endregion

            #region Adjunto

            modelBuilder.Entity<Adjunto>()
                .HasOne<TipoAdjunto>(x => x.TipoAdjunto)
                .WithMany(x => x.Adjuntos)
                .HasForeignKey(x => x.IdTipoAdjunto)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Adjunto>()
                .HasOne<Student>(x => x.Student)
                .WithMany(x => x.Adjuntos)
                .HasForeignKey(x => x.IdStudent)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Tipo Adjunto          

            #endregion

            #region Tipo Section          

            #endregion

            #region Educational  Institution

            //Suject
            modelBuilder.Entity<Suject>()
                .HasOne<EducationalInstitution>(x => x.EducationalInstitution)
                .WithMany(x => x.Suject)
                .HasForeignKey(s => s.IdEducationalInsti)
                .OnDelete(DeleteBehavior.SetNull);
            //Room            
            modelBuilder.Entity<Room>()
               .HasOne<EducationalInstitution>(x => x.EducationalInstitution)
               .WithMany(x => x.Room)
               .HasForeignKey(s => s.IdEducationalInsti)
               .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<Student>()
            //.HasOne(s => s.EducationalInstitution)
            //.WithMany(tr => tr.Student)
            //.HasForeignKey(s => s.IdInstitu)
            //.OnDelete(DeleteBehavior.SetNull);
            
            //Grade            
            modelBuilder.Entity<Grade>()
               .HasOne(x => x.EducationalInstitution)
               .WithMany(x => x.Grade)
               .HasForeignKey(s => s.IdEducationalInsti)
               .OnDelete(DeleteBehavior.SetNull);
        
            //RoomTeacher
            modelBuilder.Entity<RoomTeacher>()
               .HasOne<Suject>(x => x.Suject)
               .WithMany(x => x.RoomTeacher)
               .HasForeignKey(s => s.IdSuject)
               .OnDelete(DeleteBehavior.Cascade);

            //RoomTeacher
            modelBuilder.Entity<RoomTeacher>()
               .HasOne<Room>(x => x.Room)
               .WithMany(x => x.RoomTeachers)
               .HasForeignKey(s => s.IdRoom)
               .OnDelete(DeleteBehavior.Cascade);

            //TeachingPeriods
            modelBuilder.Entity<TeachingPeriods>()
                .HasOne<EducationalInstitution>(x => x.EducationalInstitution)
                .WithMany(x => x.SelectivePeriod)
                .HasForeignKey(s => s.IdEducationalInsti)
                .OnDelete(DeleteBehavior.Cascade);

            //Department          

            modelBuilder.Entity<EducationalInstitution>()
               .HasOne<Department>(x => x.Department)
               .WithMany(x => x.Institutions)
               .HasForeignKey(s => s.IdDepartment)
               .OnDelete(DeleteBehavior.SetNull);
            //ProvinceDom
            modelBuilder.Entity<EducationalInstitution>()
                .HasOne<ProvinceDom>(x => x.ProvinceDom)
                .WithMany(x => x.Institutions)
                .HasForeignKey(s => s.IdProvinceDom)
                .OnDelete(DeleteBehavior.SetNull);


            #endregion

            #region Room

            //Room
            //
            //modelBuilder.Entity<Room>()
            //   .HasOne<TypeRegister>(x => x.TypeRegister)
            //   .WithMany(x => x.Room)
            //   .HasForeignKey(x => x.IdTypeRegister)
            //   .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RoomSchedule>()
               .HasOne<Room>(x => x.Room)
               .WithMany(x => x.Schedules)
               .HasForeignKey(s => s.IdRoom)
               .OnDelete(DeleteBehavior.Cascade);

             modelBuilder.Entity<RoomTeacher>()
               .HasOne<Room>(x => x.Room)
               .WithMany(x => x.RoomTeachers)
               .HasForeignKey(s => s.IdRoom)
               .OnDelete(DeleteBehavior.Cascade);

              modelBuilder.Entity<GroupCenter>()
               .HasOne<Room>(x => x.Room)
               .WithMany(x => x.GroupCenter)
               .HasForeignKey(s => s.IdRoom)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
               .HasOne<Room>(x => x.Room)
               .WithMany(x => x.Student)
               .HasForeignKey(s => s.IdRoom)
               .OnDelete(DeleteBehavior.SetNull);

            #endregion

            #region  Group
            //GroupGrade
            modelBuilder.Entity<GroupGrade>()
                 .HasOne<GroupCenter>(x => x.GroupCenter)
                 .WithMany(x => x.GroupGrade)
                 .HasForeignKey(s => s.GroupId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupGrade>()
                 .HasOne<Grade>(x => x.Grade)
                 .WithMany(x => x.GroupGrade)
                 .HasForeignKey(s => s.GradeId)
                 .OnDelete(DeleteBehavior.Cascade);


            #endregion

            #region Attendance

            modelBuilder.Entity<Attendance>()
              .HasOne<Room>(x => x.Room)
              .WithMany(x => x.Attendance)
              .HasForeignKey(s => s.IdRoom)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
              .HasOne<Student>(x => x.Student)
              .WithMany(x => x.Attendance)
              .HasForeignKey(s => s.IdStudent)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
              .HasOne<Suject>(x => x.Suject)
              .WithMany(x => x.Attendance)
              .HasForeignKey(s => s.IdSuject)
              .OnDelete(DeleteBehavior.SetNull);

            // Índices
            modelBuilder.Entity<Attendance>()
            .HasIndex(x => new { x.IdRoom, x.Date });

            modelBuilder.Entity<Attendance>()
                .HasIndex(x => new { x.IdStudent, x.Date });


            #endregion

            #region EvaluationsPeriod AND EvaluationsPeriodWithCalification

            modelBuilder.Entity<EvaluationsPeriodWithCalification>()
              .HasOne<Student>(x => x.Student)
              .WithMany(x => x.EvaluationsPeriodWithCalification)
              .HasForeignKey(s => s.IdStudent)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EvaluationsPeriod>()
              .HasOne<Student>(x => x.Student)
              .WithMany(x => x.EvaluationsPeriod)
              .HasForeignKey(s => s.IdStudent)
              .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<EvaluationsPeriod>()
            //  .HasOne<AchievementIndicators>(x => x.AchievementIndicator)
            //  .WithMany(x => x.EvaluationsPeriod)
            //  .HasForeignKey(s => s.IdAchievementIndicator)
            //  .OnDelete(DeleteBehavior.SetNull);

             modelBuilder.Entity<ObservationCommentEvaluation>()
              .HasOne<Student>(x => x.Student)
              .WithMany(x => x.ObservationCommentEvaluation)
              .HasForeignKey(s => s.IdStudent)
              .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region InstitutionalCenterUsers

            modelBuilder.Entity<InstitutionalCenterUsers>()
              .HasOne<EducationalInstitution>(x => x.EducationalInstitution)
              .WithMany(x => x.InstitutionalCenterUsers)
              .HasForeignKey(s => s.CenterId)
              .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #endregion

        }



    }
}

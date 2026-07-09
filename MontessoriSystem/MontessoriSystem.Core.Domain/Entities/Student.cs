using MontessoriSystem.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class Student: AuditableBaseEntity
    {
        public string? Code { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Sexo { get; set; }
        public int Age { get; set; }
        public string? Direction { get; set; }
        public string? Tel { get; set; }
        public string? TelFather { get; set; }
        public string? TelMother { get; set; }
        public string? Estado { get; set; } = "Activo";
        public string BornDate { get; set; }
        public string? Email { get; set; }
        public string? UrlImg { get; set; }


        public string? Book { get; set; }
        public string? Folio { get; set; }
        public string? RelationPersonLiveWith { get; set; }
        public bool? CarriedPreprimary { get; set; }
        public string? NEAE { get; set; }
        public string? DiseasesAllergic { get; set; }
        public string? MedicinesUse { get; set; }
        public string? EmergencyPerson { get; set; }
        public string? EmergencyTel { get; set; }
        public int? IdNacionality { get; set; }
        public int? NumberSiblings { get; set; }
        public string? AgesSiblings { get; set; }
        public int? PlaceBetweenSiblings { get; set; }
        public string? DoctorPediatrician { get; set; }

        

        //Navigation property
        public int? IdInstitu{ get; set; }
        public virtual EducationalInstitution? EducationalInstitution { get; set; } 
        public int? IdRoom{ get; set; } 
        public virtual TypeRegister? TypeRegister { get; set; } 
        public int? IdTypeRegister { get; set; }

        public virtual Grade? Grade { get; set; }
        public int? IdGrade { get; set; }
        public Room? Room { get; set; }
        public IEnumerable<Adjunto> Adjuntos { get; set; }
        public IEnumerable<Attendance> Attendance { get; set; }
        public IEnumerable<EvaluationsPeriod> EvaluationsPeriod { get; set; }
        public IEnumerable<EvaluationsPeriodWithCalification> EvaluationsPeriodWithCalification { get; set; }
        public IEnumerable<ObservationCommentEvaluation> ObservationCommentEvaluation { get; set; }
        public string? IdFather { get; set; }
        public string? IdMother { get; set; }

    }



}

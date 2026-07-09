using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Student
{
    public class StudentViewModel
    {
        public string Code { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Sexo { get; set; }
        public int Age { get; set; }
        public string Direction { get; set; }
        public string TelFather { get; set; }
        public string? Tel { get; set; }
        public string TelMother { get; set; }
        public string? Estado { get; set; }
        public string BornDate { get; set; }
        public int IdInstitu { get; set; }
        public int? IdRoom { get; set; }
        public int? IdTypeRegister { get; set; }
        public int? IdGrade { get; set; }
        public string Book { get; set; }
        public string Folio { get; set; }
        public string RelationPersonLiveWith { get; set; }
        public bool CarriedPreprimary { get; set; }
        public string? NEAE { get; set; }
        public string? DiseasesAllergic { get; set; }
        public string? MedicinesUse { get; set; }
        public string? EmergencyPerson { get; set; }
        public string? EmergencyTel { get; set; }
        public string? IdFather { get; set; }
        public string? IdMother { get; set; }
        public int? IdNacionality { get; set; }
        public int? NumberSiblings { get; set; }
        public int? PlaceBetweenSiblings { get; set; }
        public string? DoctorPediatrician { get; set; }
        public string? UrlImg { get; set; }

        // Custom
        public string RelationPersonLiveWithDesc { get; set; }
        public string? IdFatherDesc { get; set; }
        public string? IdMotherDesc { get; set; }
        public string? SexDes { get; set; }
        public string? GradeDes { get; set; }
        public string? Nacionality { get; set; }
        public string? AgesSiblings { get; set; }
        public string Level { get; set; }
        public int NumberList { get; set; }

        //Navigation property
        public IEnumerable<MontessoriSystem.Core.Domain.Entities.Adjunto> Adjuntos { get; set; }
      


     
    }
}

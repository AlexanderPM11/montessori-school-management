using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Student
{
    public class StudentSaveViewModel
    {
        public string? Code { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe de especificar una fecha de Nacimiento.")]
        public string BornDate { get; set; }
        [Required(ErrorMessage = "Debe de especificar una Nombre.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debe de especificar una Apellido.")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Debe de especificar el Sexo.")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "Debe de especificar la Edad.")]
        public int Age { get; set; }
        //[Required(ErrorMessage = "Debe de especificar una Direccion.")]
        public string? Direction { get; set; }
        [Required(ErrorMessage = "Debe de especificar el telefono del padre.")]
        public string? TelFather { get; set; }
        [Required(ErrorMessage = "Debe de especificar el telefono de la madre.")]
        public string? TelMother { get; set; }
        [Required(ErrorMessage = "Debe de seleccionar una Seccion.")]
        //public int IdSeccionStudent { get; set; }
        public int IdInstitu { get; set; }
        public int? IdRoom { get; set; }
        public int IdTypeRegister { get; set; }
        public string? Tel { get; set; }
        public string? Book { get; set; }
        public string? Email { get; set; }
        public string? Folio { get; set; }
        public string? RelationPersonLiveWith { get; set; }
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
        public string? AgesSiblings { get; set; }
        public int? PlaceBetweenSiblings { get; set; }
        public string? DoctorPediatrician { get; set; }
        public string? UrlImg { get; set; }
        public int? IdGrade { get; set; }
        public string? GradeDes { get; set; }
        // Custom
        public string? RelationPersonLiveWithDesc { get; set; }
        public string? IdFatherDesc { get; set; }
        public string? IdMotherDesc { get; set; }
        public string? SexDes { get; set; }
        public string? Nacionality { get; set; }
        public IFormFile? File { get; set; }
    }
}

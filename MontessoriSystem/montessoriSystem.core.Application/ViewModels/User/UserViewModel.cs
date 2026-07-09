using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Campo Requerido")]
        public List<string> Roles { get; set; }
        public bool HasError { get; set; } = false;
        public string? Error { get; set; }
        public string? UrlImage { get; set; }
        public bool Statu { get; set; } = true;
        public DateTime DateBirth { get; set; } = DateTime.Now;
        public int Gender { get; set; }
        public bool Estado { get; set; } = true;
        public string Addres { get; set; }
        public string? IdUserCreator { get; set; }
        public int? InstitutionId { get; set; }
        public int? InstitutionIdPrincipal { get; set; }




        //custom
        public string? Tel { get; set; }
        public string? Profession { get; set; }
        public string? Occupation { get; set; }
        public string? Job { get; set; }
        public string? PlaceWork { get; set; }
        public string IdentificationId { get; set; }
        public string IdNivelEducativo { get; set; }
        public string? EducationLevel { get; set; }
        public string? Sex { get; set; }
        public string? ProfessionDesc { get; set; }
        public string? JobDesc { get; set; }
        public string? CivilStatus { get; set; }
        public string? TitleAchieved { get; set; }
        public string? StudiesCurrentlyPursuing { get; set; }
        public string? Nationality { get; set; }
        public int? YearsServiceEducationalSystem { get; set; }
        public int? YearServiceGrade { get; set; }
        public int? AreaSpecialization { get; set; }
        public bool? WorksAnActivityDiferentThanteaching { get; set; } = false;
        public string? Specify { get; set; }
        public string? RelationshipId { get; set; }

        //Mapper properties
        public string? CivilStatusDesc { get; set; }
        public string? NationalityDesc { get; set; }
        public string? AreaSpecializationDesc { get; set; }
        public string? TitleAchievedDesc { get; set; }
        public int? YearsServiceEducationalSystemDesc { get; set; }
        public string? StudiesCurrentlyPursuingDesc { get; set; }
        public string? Relationship { get; set; }
        public string? NoBook { get; set; }
        public string? NoFolio { get; set; }


        //Api Properti
        public string? Token { get; set; }
    }
}

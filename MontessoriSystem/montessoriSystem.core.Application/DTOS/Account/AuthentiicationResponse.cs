using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Addres { get; set; }
        public DateTime DateBirth { get; set; }
        public int Gender { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool Estado { get; set; } = true;
        public string? IdUserCreator { get; set; }
        public int? InstitutionId { get; set; }
        public int? InstitutionIdPrincipal { get; set; }

        public string? UrlImage { get; set; }
        public bool Statu { get; set; } = true;

        public string? Tel { get; set; }
        public string? Profession { get; set; }
        public string? Occupation { get; set; }
        public string? Job { get; set; }
        public string? PlaceWork { get; set; }
        public string IdentificationId { get; set; }
        public string IdNivelEducativo { get; set; }

        public string? TitleAchieved { get; set; }
        public string? StudiesCurrentlyPursuing { get; set; }
        public string? CivilStatus { get; set; }
        public string? Nationality { get; set; }
        public int? YearsServiceEducationalSystem { get; set; }
        public int? YearServiceGrade { get; set; }
        public int? AreaSpecialization { get; set; }
        public bool? WorksAnActivityDiferentThanteaching { get; set; } = false;
        public string? Specify { get; set; }
        public string? RelationshipId { get; set; }

        public string? NoBook { get; set; }
        public string? NoFolio { get; set; }
        public string? IdsRoom { get; set; }

        //Api Properti
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}

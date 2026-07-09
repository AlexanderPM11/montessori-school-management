using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.EducationalInstitution
{
    public class EducationalInstitutionViewModel: AuditableBaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string NameMunicipality { get; set; }
        public string? Phone { get; set; }
        public string Mobile { get; set; }
        public string IdRector { get; set; }
        public string IdCordinator { get; set; }
        public int? IdProvinceDom { get; set; }
        public int? IdDepartment { get; set; }
        public string? IdSecretary { get; set; }
        public string? AcademicResolution { get; set; }
        public string? EducationalRegistry { get; set; }
        public string? Website { get; set; }
        public string? UrlLogo { get; set; }
        public bool IsMainSchool { get; set; }
        public string? IdUser { get; set; }
        public string UserAssignmentId { get; set; }

        public string Session { get; set; }
        public string Regional { get; set; }
        public string District { get; set; }

        public string? NameRector { get; set; } = "";
        public string? NameCordinator { get; set; } = "";
        public string? NameSecretary { get; set; } = "";
        public string? NameAdmin { get; set; } = "";
        public string? base64Img { get; set; } = "";


        public IFormFile File { get; set; }

    }
}

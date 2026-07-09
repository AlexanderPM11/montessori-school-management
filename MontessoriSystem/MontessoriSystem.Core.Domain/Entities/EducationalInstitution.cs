using MontessoriSystem.Core.Domain.Common;

namespace MontessoriSystem.Core.Domain.Entities
{
    public class EducationalInstitution : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string NameMunicipality { get; set; }
        public string? Phone { get; set; }
        public string Mobile { get; set; }
        public string? IdRector { get; set; }
        public string? IdCordinator { get; set; }
        public string? IdSecretary { get; set; }
        public string? AcademicResolution { get; set; }
        public string? EducationalRegistry { get; set; }
        public string? Website { get; set; }
        public string? UrlLogo { get; set; }
        public string Session { get; set; }
        public string Regional { get; set; }
        public string District { get; set; }
        public bool IsMainSchool { get; set; } = false;

        //Navigation property
        public string? IdUser { get; set; }
        public string? UserAssignmentId { get; set; }

        public ProvinceDom ProvinceDom { get; set; }
        public int? IdProvinceDom { get; set; }
        public Department Department { get; set; }
        public int? IdDepartment { get; set; }

        public IEnumerable<TeachingPeriods> SelectivePeriod { get; set; }
        public IEnumerable<Room> Room { get; set; }
        public IEnumerable<Suject>  Suject { get; set; }
        public IEnumerable<Grade>? Grade { get; set; }
        public IEnumerable<Student>? Student { get; set; }
        public IEnumerable<InstitutionalCenterUsers> InstitutionalCenterUsers { get; set; }
     
    }

}

using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Application.ViewModels.ObservationCommentEvaluation;
using MontessoriSystem.Core.Application.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.AchievementIndicators
{
    public class AchievementIndicatorStatusViewModelResponse
    {
        public List<AchievementIndicatorStatusViewModel> vmPrinc { get; set; }
        public List<EvaluationsDetails> EvaluationsDetails { get; set; }
        public StudentSaveViewModel StudentViewModel { get; set; }
        public EducationalInstitutionViewModel EducationalInstitutionViewModel { get; set; }
        public string NameTeacher { get; set; }
        public string NameProvince { get; set; }
        public string NameMunicipality { get; set; }
        public string NextGradeDes { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Period { get; set; }
        public int IdStudent { get; set; }
        public string SubjectName { get; set; }
        public string Base64Preview { get; set; }

        public List<ObservationCommentEvaluationViewModel> ObservationCommentEvaluationViewModel { get; set; }

    }
    public class AchievementIndicatorStatusViewModel
    {
        public AchievementIndicatorsViewModel Indicator { get; set; }
        public bool ExistsInEvaluationsPeriod { get; set; }
        public string Estado { get; set; }
        public string Periodo { get; set; }
        public int? Calification { get; set; }
        public int RecuperaPedg { get; set; }
        public List<PeriodoEstadoViewModel> PeriodosEstados { get; set; }

    }
    public class EvaluationsDetails
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public int IndicatorCount { get; set; }
        public List<AchievementIndicatorStatusViewModel> Indicators { get; set; }   

    }
    public class PeriodoEstadoViewModel
    {
        public string Periodo { get; set; }
        public string Estado { get; set; }
        public int Calification { get; set; }
        public int? RecuperaPedg { get; set; }
    }
}

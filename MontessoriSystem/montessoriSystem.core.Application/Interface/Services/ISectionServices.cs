using MontessoriSystem.Core.Application.ViewModels.Section;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Interface.Services
{
    public interface ISectionServices: IGenericService<SectionSaveViewModel, SectionViewModel, Section>
    {
        Task ActiveInactiveSection(int id);
    }
}

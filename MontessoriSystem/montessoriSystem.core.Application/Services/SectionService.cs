using AutoMapper;
using MontessoriSystem.Core.Application.Interface.Repository;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Section;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services
{
    public class SectionService : GenericService<SectionSaveViewModel, SectionViewModel, Section>, ISectionServices
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IMapper _mapper;

        public SectionService(ISectionRepository sectionRepository, IMapper mapper)
        : base(sectionRepository, mapper)
        {           
            _mapper = mapper;
            _sectionRepository = sectionRepository;
        }
        public async Task ActiveInactiveSection(int id)
        {
            var section = await _sectionRepository.GetByIdAsync(id);
            if (section != null)
            {
                if (section.Estado == "Activo")
                {
                    section.Estado = "Inactivo";
                }
                else
                {
                    section.Estado = "Activo";
                }
                await _sectionRepository.UpdateAsync(section, id);
            }
        }

    }
}

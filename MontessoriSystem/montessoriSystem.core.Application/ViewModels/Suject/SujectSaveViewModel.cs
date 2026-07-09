using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.Suject
{
    public class SujectSaveViewModel:AuditableBaseModel
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? IdEducationalInsti { get; set; }


        public IFormFile? File { get; set; }

    }
}

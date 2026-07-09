using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Reports
{
    public class SaveCommentsRequestDTO
    {
        public int IdStudent { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Period { get; set; }
    }
}

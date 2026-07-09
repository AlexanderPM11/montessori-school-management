using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Common
{
    public class GeneralResponse<T>
    {
        public T Data { get; set; }
        public bool result { get; set; } = true;
        public List<string> messages { get; set; }= new List<string>();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Account
{
    public class ConfirmEmail
    {
        public string userId { get; set; }
        public string Token { get; set; }
    }
}

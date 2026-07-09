using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Account
{
    public class ResponseTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

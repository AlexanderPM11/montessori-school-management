using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.DTOS.Account
{
    public class AuthenticationRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }

    }
}

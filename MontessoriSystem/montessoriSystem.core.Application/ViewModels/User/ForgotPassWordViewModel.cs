using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.User
{
    public class ForgotPassWordViewModel
    {
        //[Required(ErrorMessage = "")]
        public string Email { get; set; }
    }
}

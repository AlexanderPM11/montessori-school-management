using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Campo Requerido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string token { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string ConfirmPassword { get; set; }
    }
}

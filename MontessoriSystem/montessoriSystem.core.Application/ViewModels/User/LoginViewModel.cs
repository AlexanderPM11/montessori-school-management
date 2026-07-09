using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe de especificar una contrasena")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe de especificar un correo  electronico.")]
        public string Email { get; set; }

        [JsonIgnore]
        public string? Error { get; set; }
        [JsonIgnore]
        public bool? HasError { get; set; }
    }
}

using crudSignalR.Core.Application.Dtos.EmailService;
using crudSignalR.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Domain.Entities;
using MontessoriSystem.Core.Domain.Settings;
using MontessoriSystem.Infrastructure.Identity.Entities;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MontessoriSystem.Infrastructure.Identity.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;

        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IEmailService  emailService, RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwtSettings, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _configuration = configuration;
        }
        public async Task<GeneralResponse<AuthenticationResponse>> AuthenticateAsyncWebApi(AuthenticationRequest request)
        {
            GeneralResponse<AuthenticationResponse> generalResponse = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Cuenta no encontrada para: {request.Email}");
                return generalResponse;
            }
            if (!user.EmailConfirmed)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Cuenta sin confirmar para: {request.Email}");
                return generalResponse;
            }
            if (!user.EmailConfirmed)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Cuenta no confirmada para: {request.Email}");
                return generalResponse;
            }
            if (!user.Statu)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Su cuenta esta inactiva, contacte con el administrador.");
                return generalResponse;
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Invalid credentials for {request.Email}");
                return generalResponse;
            }
            AuthenticationResponse authentiicationResponse = new AuthenticationResponse();
            authentiicationResponse.Id = user.Id;
            authentiicationResponse.Email = user.Email;
            authentiicationResponse.UserName = user.UserName;
            authentiicationResponse.FirstName = user.FirstName;
            authentiicationResponse.LastName = user.LastName;
            authentiicationResponse.UrlImage = user.UrlImage;
            authentiicationResponse.Addres = user.Addres;
            authentiicationResponse.DateBirth = user.DateBirth;
            authentiicationResponse.Statu = user.Statu;
            authentiicationResponse.Gender = user.Gender;
            authentiicationResponse.InstitutionId = user.InstitutionId;
            authentiicationResponse.InstitutionIdPrincipal = user.InstitutionIdPrincipal;

            authentiicationResponse.Tel = user.Tel;
            authentiicationResponse.PhoneNumber = user.PhoneNumber;
            authentiicationResponse.Profession = user.Profession;
            authentiicationResponse.Occupation = user.Occupation;
            authentiicationResponse.Job = user.Job;
            authentiicationResponse.PlaceWork = user.PlaceWork;
            authentiicationResponse.IdentificationId = user.IdentificationId;
            authentiicationResponse.IdNivelEducativo = user.IdNivelEducativo;

            authentiicationResponse.CivilStatus = user.CivilStatus;
            authentiicationResponse.Nationality = user.Nationality;
            authentiicationResponse.YearsServiceEducationalSystem = user.YearsServiceEducationalSystem;
            authentiicationResponse.YearServiceGrade = user.YearServiceGrade;
            authentiicationResponse.AreaSpecialization = user.AreaSpecialization;
            authentiicationResponse.TitleAchieved = user.TitleAchieved;
            authentiicationResponse.StudiesCurrentlyPursuing = user.StudiesCurrentlyPursuing;
            authentiicationResponse.WorksAnActivityDiferentThanteaching = user.WorksAnActivityDiferentThanteaching;
            authentiicationResponse.Specify = user.Specify;
            authentiicationResponse.RelationshipId = user.RelationshipId;
            authentiicationResponse.UrlImage = user.UrlImage;

            authentiicationResponse.NoBook = user.NoBook;
            authentiicationResponse.NoFolio = user.NoFolio;

            var roles = await _userManager.GetRolesAsync(user);
            authentiicationResponse.Roles = roles.ToList();
            authentiicationResponse.IsVerified = user.EmailConfirmed;

            authentiicationResponse.Token = await GenerateJwtToken(user);
            authentiicationResponse.RefreshToken = GenerateRefreshToken();

            generalResponse.Data = authentiicationResponse;

            user.RefreshToken = authentiicationResponse.RefreshToken;

            await _userManager.UpdateAsync(user);

            return generalResponse;
        }
        public async Task<GeneralResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            GeneralResponse<AuthenticationResponse> generalResponse = new();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Cuenta no encontrada para: {request.Email}");
                return generalResponse;
            }
            if (!user.EmailConfirmed)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Cuenta sin confirmar para: {request.Email}");
                return generalResponse;
            }
            if (!user.EmailConfirmed)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Cuenta no confirmada para: {request.Email}");
                return generalResponse;
            }
            if (!user.Statu)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Su cuenta esta inactiva, contacte con el administrador.");
                return generalResponse;
            }


            var result= await _signInManager.PasswordSignInAsync(user, request.Password,false,false);
            if (!result.Succeeded)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Invalid credentials for {request.Email}");
                return generalResponse;
            }
            AuthenticationResponse authentiicationResponse = new AuthenticationResponse();
            authentiicationResponse.Id = user.Id;
            authentiicationResponse.Email = user.Email;
            authentiicationResponse.UserName = user.UserName;
            authentiicationResponse.UrlImage = user.UrlImage;
            authentiicationResponse.Addres = user.Addres;
            authentiicationResponse.DateBirth = user.DateBirth;
            authentiicationResponse.Statu = user.Statu;
            authentiicationResponse.Gender = user.Gender;
            authentiicationResponse.InstitutionId = user.InstitutionId;
            authentiicationResponse.InstitutionIdPrincipal = user.InstitutionIdPrincipal;

            authentiicationResponse.Tel = user.Tel;
            authentiicationResponse.Profession = user.Profession;
            authentiicationResponse.Occupation = user.Occupation;
            authentiicationResponse.Job = user.Job;
            authentiicationResponse.PlaceWork = user.PlaceWork;
            authentiicationResponse.IdentificationId = user.IdentificationId;
            authentiicationResponse.IdNivelEducativo = user.IdNivelEducativo;

            authentiicationResponse.CivilStatus = user.CivilStatus;
            authentiicationResponse.Nationality = user.Nationality;
            authentiicationResponse.YearsServiceEducationalSystem = user.YearsServiceEducationalSystem;
            authentiicationResponse.YearServiceGrade = user.YearServiceGrade;
            authentiicationResponse.AreaSpecialization = user.AreaSpecialization;
            authentiicationResponse.TitleAchieved = user.TitleAchieved;
            authentiicationResponse.StudiesCurrentlyPursuing = user.StudiesCurrentlyPursuing;
            authentiicationResponse.WorksAnActivityDiferentThanteaching = user.WorksAnActivityDiferentThanteaching;
            authentiicationResponse.Specify = user.Specify;
            authentiicationResponse.RelationshipId = user.RelationshipId;
            
            authentiicationResponse.NoBook = user.NoBook;
            authentiicationResponse.NoFolio = user.NoFolio;

            var roles =await _userManager.GetRolesAsync(user);
            authentiicationResponse.Roles = roles.ToList();

            authentiicationResponse.IsVerified = user.EmailConfirmed;

            generalResponse.Data = authentiicationResponse;

            return generalResponse;
        }
        public async Task SignOutAsync()
        {
           await _signInManager.SignOutAsync();
        }
        public async Task<GeneralResponse<ResponseTokenDTO>> RefreshToken(GenerateTokenRequestDTO generateToken)
        {
            GeneralResponse<ResponseTokenDTO> generalResponse = new();

            var user = await _userManager.FindByEmailAsync(generateToken.Email);
            if (user == null)
            {
                generalResponse.result = false;
                generalResponse.messages.Add($"Cuenta no encontrada para: {generateToken.Email}");
                return generalResponse;
            }
            if ( user.RefreshToken != generateToken.RefreshToken)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("Invalid access token or refresh token");
                return generalResponse;
            }

            user.RefreshToken = GenerateRefreshToken();

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                generalResponse.result = false;
                foreach (var error in result.Errors)
                {
                    generalResponse.messages.Add(error.Description);
                }
                return generalResponse;
            }

            ResponseTokenDTO responseTokenRequestDTO = new();
            responseTokenRequestDTO.RefreshToken = user.RefreshToken;
            responseTokenRequestDTO.Token = await GenerateJwtToken(user);

            generalResponse.Data= responseTokenRequestDTO;

            return generalResponse;
        }
        public async Task<GeneralResponse<string>> RegisterUserAsync(RegisterRequest request, List<string> roles, string origin, bool sendEmail = false, bool apiURl = false)
        {
            try
            {
                GeneralResponse<string> generalResponse = new();

                string email = request.Email.Trim();
                if (string.IsNullOrEmpty(email))
                {
                    generalResponse.Data = "";
                    generalResponse.result = false;
                    generalResponse.messages.Add("El correo no puede estar vacio");
                    return generalResponse;
                }

                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    generalResponse.Data= user.Id;
                    generalResponse.result = false;
                    generalResponse.messages.Add($"Este correo ya esta en uso: {request.Email}");
                    return generalResponse;
                }
                var userSameName = await _userManager.FindByNameAsync(request.UserName);
                if (userSameName != null)
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add($"Este nombre de usuario ya esta en uso: {request.UserName}");
                    return generalResponse;
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    Email = request.Email.Trim(),
                    EmailConfirmed = true, //Esto esta asi temporalmente
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.UserName,
                    IdUserCreator = request.IdUserCreator,
                    UrlImage = request.UrlImage,
                    Addres = request.Addres,
                    Statu = true,
                    Estado = true,
                    Gender = request.Gender,
                    DateBirth = request.DateBirth,
                    InstitutionId = request.InstitutionId,
                    InstitutionIdPrincipal = request.InstitutionIdPrincipal,

                    Tel = request.Tel,
                    Profession = request.Profession,
                    Occupation = request.Occupation,
                    Job = request.Job,
                    PlaceWork = request.PlaceWork,
                    IdentificationId = request.IdentificationId,
                    IdNivelEducativo = request.IdNivelEducativo,

                    CivilStatus = request.CivilStatus,
                    Nationality = request.Nationality,
                    YearsServiceEducationalSystem = request.YearsServiceEducationalSystem,
                    YearServiceGrade = request.YearServiceGrade,
                    AreaSpecialization = request.AreaSpecialization,
                    TitleAchieved = request.TitleAchieved,
                    StudiesCurrentlyPursuing = request.StudiesCurrentlyPursuing,
                    WorksAnActivityDiferentThanteaching = request.WorksAnActivityDiferentThanteaching,
                    Specify = request.Specify,
                    RelationshipId = request.RelationshipId,
                    NoFolio = request.NoFolio,
                    NoBook = request.NoBook,

                };

                string password = GenerateStrongPassword(16);
                var result = await _userManager.CreateAsync(applicationUser, password);

                if (!result.Succeeded)
                {
                    generalResponse.result = false;
                    foreach (var error in result.Errors)
                    {
                        generalResponse.messages.Add(error.Description);
                    }
                    return generalResponse;
                }
                foreach (var role in roles)
                {
                    await _userManager.AddToRoleAsync(applicationUser, role.ToString());
                }

                //if (sendEmail)
                //{
                //    string verificationUri = await SendVerificationEmaiilURL(applicationUser, origin,apiURl);
                //    await _emailService.SendAsync(
                //        new EmailRequest
                //        {
                //            To = applicationUser.Email,
                //            Body = $"Por favor, confirme su correo electrónico en la siguiente URL:  {verificationUri}",
                //            Subject = "Nuevo Usuario"
                //        });
                //}

                //await _emailService.SendAsync(
                //    new EmailRequest
                //    {
                //        To = applicationUser.Email,
                //        Body = $"Su cuenta ha sido creada exitosamente. Su nueva contraseña es: {password}. " +
                //               $"Por razones de seguridad, le recomendamos cambiarla después de iniciar sesión.",
                //        Subject = "Credenciales de Acceso"
                //    }
                //);

                generalResponse.messages.Add("Cuenta registrada exitosamente");
                generalResponse.Data = applicationUser.Id;
                return generalResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public async Task<GeneralResponse<bool>> DeleteUser(string idUser)
        {
            GeneralResponse<bool> generalResponse = new();
            var user = await _userManager.FindByIdAsync(idUser);
            if (user == null) {
                generalResponse.messages.Add("Usuario no encontrado.");
                generalResponse.Data = false;
                return generalResponse;
            };

            var response = await _userManager.DeleteAsync(user);
            if (response.Succeeded)
            {
                generalResponse.messages.Add("Usuario eliminado exitosamente.");
                generalResponse.Data = true;
                return generalResponse;
            }
            var errorMessage = $"Ocurrió un error. Se encontraron {response.Errors.Count()} errores: " +
                                   string.Join("; ", response.Errors.Select(e => e.Description));
            generalResponse.messages.Add(errorMessage);
            generalResponse.Data = false;

            return generalResponse;
        }
        public async Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllMyAdminUsers(string IdUserCreator)
        {
            GeneralResponse<List<AppliciationUserDTO>> userListResponse =
               new();
            userListResponse.Data = new List<AppliciationUserDTO>();

            try
            {
                var users = _userManager.Users.Where(user => user.IdUserCreator == IdUserCreator).ToList();

                foreach (var item in users)
                {
                    var roles = await _userManager.GetRolesAsync(item);

                    if (roles.Contains(Roles.Admin.ToString()))
                    {
                        AppliciationUserDTO user = new();
                        user.Id = item.Id;
                        user.UserName = item.UserName;
                        user.Email = item.Email;
                        user.PhoneNumber = item.PhoneNumber;
                        user.FirstName = item.FirstName;
                        user.LastName = item.LastName;
                        user.LastName = item.LastName;
                        user.Statu = item.Statu;
                        user.UrlImage = item.UrlImage;
                        user.Addres = item.Addres;
                        user.DateBirth = item.DateBirth;
                        user.Gender = item.Gender;
                        user.InstitutionId = item.InstitutionId;
                        user.InstitutionIdPrincipal = item.InstitutionIdPrincipal;

                        user.Tel = item.Tel;
                        user.Profession = item.Profession;
                        user.Occupation = item.Occupation;
                        user.Job = item.Job;
                        user.PlaceWork = item.PlaceWork;
                        user.IdentificationId = item.IdentificationId;
                        user.IdNivelEducativo = item.IdNivelEducativo;

                        user.CivilStatus = item.CivilStatus;
                        user.Nationality = item.Nationality;
                        user.YearsServiceEducationalSystem = item.YearsServiceEducationalSystem;
                        user.YearServiceGrade = item.YearServiceGrade;
                        user.AreaSpecialization = item.AreaSpecialization;
                        user.TitleAchieved = item.TitleAchieved;
                        user.StudiesCurrentlyPursuing = item.StudiesCurrentlyPursuing;
                        user.WorksAnActivityDiferentThanteaching = item.WorksAnActivityDiferentThanteaching;
                        user.Specify = item.Specify;
                        user.RelationshipId = item.RelationshipId;
                        user.RelationshipId = item.RelationshipId;

                        user.Roles = roles.ToList();

                        userListResponse.Data.Add(user);
                    }

                }
                return userListResponse;
            }
            catch (Exception ex)
            {
                userListResponse.result = false;
                userListResponse.messages.Add("Ocurrio un error al  obtener los  usuarios" + ex.Message);
                return userListResponse;
            }

        }
        public async Task<List<String>> GetAllRoles(List<string> rolesUser)
        {
            List<string> listroles = new List<string>();

            var roles = _roleManager.Roles.ToList();

            if (rolesUser.Contains(Roles.SuperAdmin.ToString()))
            {
                listroles = roles.Select(x => x.Name).ToList();
            }
            else if (rolesUser.Contains(Roles.Admin.ToString()))
            {
                foreach (var item in roles)
                {
                    if (item.Name != Roles.SuperAdmin.ToString()  && item.Name != Roles.Admin.ToString())
                    {
                        listroles.Add(item.Name);
                    }
                }
            }
            else return rolesUser;

            return listroles;
        }
        public async Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllMyUsers(string IdUserCreator)
        {
            GeneralResponse<List<AppliciationUserDTO>> userListResponse =
               new();
            userListResponse.Data = new List<AppliciationUserDTO>();

            try
            {
                var users = _userManager.Users.Where(user => user.IdUserCreator == IdUserCreator).ToList();

                foreach (var item in users)
                {
                    var roles = await _userManager.GetRolesAsync(item);

                    AppliciationUserDTO user = new()
                    {
                        Id = item.Id,
                        UserName = item.UserName,
                        Email = item.Email,
                        PhoneNumber = item.PhoneNumber,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Statu = item.Statu,
                        UrlImage = item.UrlImage,
                        Addres = item.Addres,
                        DateBirth = item.DateBirth,
                        Gender = item.Gender,
                        InstitutionId = item.InstitutionId,
                        InstitutionIdPrincipal = item.InstitutionIdPrincipal,

                        Tel = item.Tel,
                        Profession = item.Profession,
                        Occupation = item.Occupation,
                        Job = item.Job,
                        PlaceWork = item.PlaceWork,
                        IdentificationId = item.IdentificationId,
                        IdNivelEducativo = item.IdNivelEducativo,

                        CivilStatus = item.CivilStatus,
                        Nationality = item.Nationality,
                        YearsServiceEducationalSystem = item.YearsServiceEducationalSystem,
                        YearServiceGrade = item.YearServiceGrade,
                        AreaSpecialization = item.AreaSpecialization,
                        TitleAchieved = item.TitleAchieved,
                        StudiesCurrentlyPursuing = item.StudiesCurrentlyPursuing,
                        WorksAnActivityDiferentThanteaching = item.WorksAnActivityDiferentThanteaching,
                        Specify = item.Specify,
                        RelationshipId = item.RelationshipId,
                        IdsRoom = item.IdsRoom,

                        Roles = roles.ToList()
                    };

                    userListResponse.Data.Add(user);
                }
                return userListResponse;
            }
            catch (Exception ex)
            {
                userListResponse.result = false;
                userListResponse.messages.Add("Ocurrio un error al  obtener los  usuarios" + ex.Message);
                return userListResponse;
            }

        }
        public async Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllUsersByIdIinstitutionPrincipal(int institutionIdPrincipal)
        {
            GeneralResponse<List<AppliciationUserDTO>> userListResponse =
               new();
            userListResponse.Data = new List<AppliciationUserDTO>();

            try
            {
                var users = _userManager.Users.Where(user => user.InstitutionIdPrincipal == institutionIdPrincipal).ToList();


                foreach (var item in users)
                {
                    var roles = await _userManager.GetRolesAsync(item);

                    AppliciationUserDTO user = new()
                    {
                        Id = item.Id,
                        UserName = item.UserName,
                        Email = item.Email,
                        PhoneNumber = item.PhoneNumber,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Statu = item.Statu,
                        UrlImage = item.UrlImage,
                        Addres = item.Addres,
                        DateBirth = item.DateBirth,
                        Gender = item.Gender,
                        InstitutionId = item.InstitutionId,
                        InstitutionIdPrincipal = item.InstitutionIdPrincipal,

                        Tel = item.Tel,
                        Profession = item.Profession,
                        Occupation = item.Occupation,
                        Job = item.Job,
                        PlaceWork = item.PlaceWork,
                        IdentificationId = item.IdentificationId,
                        IdNivelEducativo = item.IdNivelEducativo,

                        TitleAchieved = item.TitleAchieved,
                        StudiesCurrentlyPursuing = item.StudiesCurrentlyPursuing,
                        CivilStatus = item.CivilStatus,
                        Nationality = item.Nationality,
                        YearsServiceEducationalSystem = item.YearsServiceEducationalSystem,
                        YearServiceGrade = item.YearServiceGrade,
                        AreaSpecialization = item.AreaSpecialization,
                        WorksAnActivityDiferentThanteaching = item.WorksAnActivityDiferentThanteaching,
                        Specify = item.Specify,
                        RelationshipId = item.RelationshipId,
                        IdsRoom = item.IdsRoom,

                        Roles = roles.ToList()
                    };

                    userListResponse.Data.Add(user);
                }

                return userListResponse;
            }
            catch (Exception ex)
            {
                userListResponse.result = false;
                userListResponse.messages.Add("Ocurrio un error al  obtener los  usuarios" + ex.Message);
                return userListResponse;
            }

        }
        public async Task<GeneralResponse<List<AppliciationUserDTO>>> GetUserByIdInstitu(int IdInstitu)
        {
            GeneralResponse<List<AppliciationUserDTO>> userListResponse =
               new();
            userListResponse.Data = new List<AppliciationUserDTO>();

            try
            {
                var users = _userManager.Users.Where(user => user.InstitutionId == IdInstitu).ToList();

                foreach (var item in users)
                {
                    var roles = await _userManager.GetRolesAsync(item);

                    AppliciationUserDTO user = new()
                    {
                        Id = item.Id,
                        UserName = item.UserName,
                        Email = item.Email,
                        PhoneNumber = item.PhoneNumber,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Statu = item.Statu,
                        UrlImage = item.UrlImage,
                        Addres = item.Addres,
                        DateBirth = item.DateBirth,
                        Gender = item.Gender,
                        InstitutionId = item.InstitutionId,
                        InstitutionIdPrincipal = item.InstitutionIdPrincipal,

                        Tel = item.Tel,
                        Profession = item.Profession,
                        Occupation = item.Occupation,
                        Job = item.Job,
                        PlaceWork = item.PlaceWork,
                        IdentificationId = item.IdentificationId,
                        IdNivelEducativo = item.IdNivelEducativo,

                        CivilStatus = item.CivilStatus,
                        Nationality = item.Nationality,
                        YearsServiceEducationalSystem = item.YearsServiceEducationalSystem,
                        YearServiceGrade = item.YearServiceGrade,
                        AreaSpecialization = item.AreaSpecialization,
                        TitleAchieved = item.TitleAchieved,
                        StudiesCurrentlyPursuing = item.StudiesCurrentlyPursuing,
                        WorksAnActivityDiferentThanteaching = item.WorksAnActivityDiferentThanteaching,
                        Specify = item.Specify,
                        RelationshipId = item.RelationshipId,
                        IdsRoom = item.IdsRoom,

                        Roles = roles.ToList()
                    };

                    userListResponse.Data.Add(user);
                }
                return userListResponse;
            }
            catch (Exception ex)
            {
                userListResponse.result = false;
                userListResponse.messages.Add("Ocurrio un error al  obtener los  usuarios" + ex.Message);
                return userListResponse;
            }

        }
        public async Task<GeneralResponse<AppliciationUserDTO>> GetUserById(string idUser)
        {
            GeneralResponse<AppliciationUserDTO> userResponse =
                new();

            try
            {
                var user = await _userManager.FindByIdAsync(idUser);

                if (user != null)
                {
                    AppliciationUserDTO AppliciationUserDTO = new();
                    AppliciationUserDTO.Id = user.Id;
                    AppliciationUserDTO.UserName = user.UserName;
                    AppliciationUserDTO.Email = user.Email;
                    AppliciationUserDTO.PhoneNumber = user.PhoneNumber;
                    AppliciationUserDTO.FirstName = user.FirstName;
                    AppliciationUserDTO.LastName = user.LastName;
                    AppliciationUserDTO.LastName = user.LastName;
                    AppliciationUserDTO.Statu = user.Statu;
                    AppliciationUserDTO.UrlImage = user.UrlImage;
                    AppliciationUserDTO.Addres = user.Addres;
                    AppliciationUserDTO.DateBirth = user.DateBirth;
                    AppliciationUserDTO.Gender = user.Gender;
                    AppliciationUserDTO.InstitutionId = user.InstitutionId;
                    AppliciationUserDTO.IdUserCreator = user.IdUserCreator;

                    AppliciationUserDTO.Tel = user.Tel;
                    AppliciationUserDTO.Profession = user.Profession;
                    AppliciationUserDTO.Occupation = user.Occupation;
                    AppliciationUserDTO.Job = user.Job;
                    AppliciationUserDTO.PlaceWork = user.PlaceWork;
                    AppliciationUserDTO.IdentificationId = user.IdentificationId;
                    AppliciationUserDTO.IdNivelEducativo = user.IdNivelEducativo;

                    AppliciationUserDTO.CivilStatus = user.CivilStatus;
                    AppliciationUserDTO.Nationality = user.Nationality;
                    AppliciationUserDTO.YearsServiceEducationalSystem = user.YearsServiceEducationalSystem;
                    AppliciationUserDTO.YearServiceGrade = user.YearServiceGrade;
                    AppliciationUserDTO.AreaSpecialization = user.AreaSpecialization;
                    AppliciationUserDTO.TitleAchieved = user.TitleAchieved;
                    AppliciationUserDTO.StudiesCurrentlyPursuing = user.StudiesCurrentlyPursuing;
                    AppliciationUserDTO.WorksAnActivityDiferentThanteaching = user.WorksAnActivityDiferentThanteaching;
                    AppliciationUserDTO.Specify = user.Specify;
                    AppliciationUserDTO.RelationshipId = user.RelationshipId;
                    AppliciationUserDTO.NoBook = user.NoBook;
                    AppliciationUserDTO.NoFolio = user.NoFolio;
                    AppliciationUserDTO.IdsRoom = user.IdsRoom;
                    AppliciationUserDTO.Statu = user.Statu;
                    AppliciationUserDTO.Estado = user.Estado;

                    var roles = await _userManager.GetRolesAsync(user);
                    AppliciationUserDTO.Roles = roles.ToList();

                    userResponse.Data = AppliciationUserDTO;
                }
                else
                {
                    userResponse.result = false;
                    userResponse.messages.Add("Usuario no encontrado");
                    return userResponse;
                }

                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.result = false;
                userResponse.messages.Add("Ocurrio un error al  obtener el  usuario" + ex.Message);
                return userResponse;
            }

        }
        public async Task<GeneralResponse<AppliciationUserDTO>> GetUserByName(string name)
        {
            GeneralResponse<AppliciationUserDTO> userResponse =
                new();

            try
            {
                var user = _userManager.Users.Where(us => us.UserName == name).FirstOrDefault();

                if (user != null)
                {
                    AppliciationUserDTO AppliciationUserDTO = new();
                    AppliciationUserDTO.Id = user.Id;
                    AppliciationUserDTO.UserName = user.UserName;
                    AppliciationUserDTO.Email = user.Email;
                    AppliciationUserDTO.PhoneNumber = user.PhoneNumber;
                    AppliciationUserDTO.FirstName = user.FirstName;
                    AppliciationUserDTO.LastName = user.LastName;
                    AppliciationUserDTO.LastName = user.LastName;
                    AppliciationUserDTO.Statu = user.Statu;
                    AppliciationUserDTO.UrlImage = user.UrlImage;
                    AppliciationUserDTO.Addres = user.Addres;
                    AppliciationUserDTO.Gender = user.Gender;
                    AppliciationUserDTO.InstitutionId = user.InstitutionId;
                    AppliciationUserDTO.InstitutionIdPrincipal = user.InstitutionIdPrincipal;
                    AppliciationUserDTO.IdUserCreator = user.IdUserCreator;

                    AppliciationUserDTO.Tel = user.Tel;
                    AppliciationUserDTO.Profession = user.Profession;
                    AppliciationUserDTO.Occupation = user.Occupation;
                    AppliciationUserDTO.Job = user.Job;
                    AppliciationUserDTO.PlaceWork = user.PlaceWork;
                    AppliciationUserDTO.IdentificationId = user.IdentificationId;
                    AppliciationUserDTO.IdNivelEducativo = user.IdNivelEducativo;

                    AppliciationUserDTO.CivilStatus = user.CivilStatus;
                    AppliciationUserDTO.Nationality = user.Nationality;
                    AppliciationUserDTO.YearsServiceEducationalSystem = user.YearsServiceEducationalSystem;
                    AppliciationUserDTO.YearServiceGrade = user.YearServiceGrade;
                    AppliciationUserDTO.AreaSpecialization = user.AreaSpecialization;
                    AppliciationUserDTO.TitleAchieved = user.TitleAchieved;
                    AppliciationUserDTO.StudiesCurrentlyPursuing = user.StudiesCurrentlyPursuing;
                    AppliciationUserDTO.WorksAnActivityDiferentThanteaching = user.WorksAnActivityDiferentThanteaching;
                    AppliciationUserDTO.Specify = user.Specify;
                    AppliciationUserDTO.RelationshipId = user.RelationshipId;
                    AppliciationUserDTO.IdsRoom = user.IdsRoom;

                    var roles = await _userManager.GetRolesAsync(user);
                    AppliciationUserDTO.Roles = roles.ToList();
                    userResponse.Data = AppliciationUserDTO;
                }
                else
                {
                    userResponse.result = false;
                    userResponse.messages.Add("Usuario no encontrado");
                    return userResponse;
                }

                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.result = false;
                userResponse.messages.Add("Ocurrio un error al  obtener el  usuario" + ex.Message);
                return userResponse;
            }

        }
        public async Task<GeneralResponse<List<AppliciationUserDTO>>> GetAllUser()
        {
            GeneralResponse<List<AppliciationUserDTO>> userListResponse =
                new();
            userListResponse.Data = new List<AppliciationUserDTO>();

            try
            {
                var users = _userManager.Users.ToList();

                foreach (var user in users)
                {
                    AppliciationUserDTO AppliciationUserDTO = new();
                    AppliciationUserDTO.Id = user.Id;
                    AppliciationUserDTO.UserName = user.UserName;
                    AppliciationUserDTO.Email = user.Email;
                    AppliciationUserDTO.PhoneNumber = user.PhoneNumber;
                    AppliciationUserDTO.FirstName = user.FirstName;
                    AppliciationUserDTO.LastName = user.LastName;
                    AppliciationUserDTO.LastName = user.LastName;
                    AppliciationUserDTO.Statu = user.Statu;
                    AppliciationUserDTO.UrlImage = user.UrlImage;
                    AppliciationUserDTO.Addres = user.Addres;
                    AppliciationUserDTO.DateBirth = user.DateBirth;
                    AppliciationUserDTO.Gender = user.Gender;
                    AppliciationUserDTO.InstitutionId = user.InstitutionId;
                    AppliciationUserDTO.InstitutionIdPrincipal = user.InstitutionIdPrincipal;
                    AppliciationUserDTO.IdUserCreator = user.IdUserCreator;

                    AppliciationUserDTO.Tel = user.Tel;
                    AppliciationUserDTO.Profession = user.Profession;
                    AppliciationUserDTO.Occupation = user.Occupation;
                    AppliciationUserDTO.Job = user.Job;
                    AppliciationUserDTO.PlaceWork = user.PlaceWork;
                    AppliciationUserDTO.IdentificationId = user.IdentificationId;
                    AppliciationUserDTO.IdNivelEducativo = user.IdNivelEducativo;

                    AppliciationUserDTO.CivilStatus = user.CivilStatus;
                    AppliciationUserDTO.Nationality = user.Nationality;
                    AppliciationUserDTO.YearsServiceEducationalSystem = user.YearsServiceEducationalSystem;
                    AppliciationUserDTO.YearServiceGrade = user.YearServiceGrade;
                    AppliciationUserDTO.AreaSpecialization = user.AreaSpecialization;
                    AppliciationUserDTO.TitleAchieved = user.TitleAchieved;
                    AppliciationUserDTO.StudiesCurrentlyPursuing = user.StudiesCurrentlyPursuing;
                    AppliciationUserDTO.WorksAnActivityDiferentThanteaching = user.WorksAnActivityDiferentThanteaching;
                    AppliciationUserDTO.Specify = user.Specify;
                    AppliciationUserDTO.RelationshipId = user.RelationshipId;
                    AppliciationUserDTO.Estado = user.Estado;
                    AppliciationUserDTO.NoBook = user.NoBook;
                    AppliciationUserDTO.NoFolio = user.NoFolio;
                    AppliciationUserDTO.IdsRoom = user.IdsRoom;

                    var roles = await _userManager.GetRolesAsync(user);
                    AppliciationUserDTO.Roles = roles.ToList();


                    userListResponse.Data.Add(AppliciationUserDTO);

                }
                return userListResponse;
            }
            catch (Exception ex)
            {
                userListResponse.result = false;
                userListResponse.messages.Add("Ocurrio un error al  obtener los  usuarios" + ex.Message);
                return userListResponse;
            }

        }
        public async Task<GeneralResponse<string>> UpdateUserAsync(RegisterRequest request, List<string> roles, string Id)
        {
            GeneralResponse<string> generalResponse = new();

            var user = await _userManager.FindByIdAsync(Id);
                        
            var userSameName = await _userManager.FindByNameAsync(request.UserName);
            if (userSameName != null)
            {
                if (user.UserName.Trim() != userSameName.UserName.Trim())
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add($"Este nombre de usuario ya esta en uso: {request.UserName}");
                    return generalResponse;
                }
            }

            var userSameEmail = await _userManager.FindByNameAsync(request.UserName);

            if (userSameEmail != null)
            {
                if (user.Email.Trim() != userSameEmail.Email.Trim())
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add($"Este correo ya esta en uso: {request.Email}");
                    return generalResponse;
                }                   
            }    

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.Gender = request.Gender;

            user.Tel = request.Tel;
            user.Profession = request.Profession;
            user.Occupation = request.Occupation;
            user.Job = request.Job;
            user.PlaceWork = request.PlaceWork;
            user.IdentificationId = request.IdentificationId;
            user.IdNivelEducativo = request.IdNivelEducativo;

            user.CivilStatus = request.CivilStatus;
            user.Nationality = request.Nationality;
            user.YearsServiceEducationalSystem = request.YearsServiceEducationalSystem;
            user.YearServiceGrade = request.YearServiceGrade;
            user.AreaSpecialization = request.AreaSpecialization;
            user.TitleAchieved = request.TitleAchieved;
            user.StudiesCurrentlyPursuing = request.StudiesCurrentlyPursuing;
            user.WorksAnActivityDiferentThanteaching = request.WorksAnActivityDiferentThanteaching;
            user.Specify = request.Specify;
            user.RelationshipId = request.RelationshipId;
            user.Estado = request.Estado;
            user.Statu = request.Statu;
            user.NoFolio = request.NoFolio;
            user.NoBook = request.NoBook;


            if (!(request.FromProfile))
            {
                //user.Statu = request.Statu;
                //user.UserName = request.UserName;
            }

            if ((request.UpdateIdsRoom))
            {
                user.IdsRoom = request.IdsRoom;
            }
              
            user.UrlImage = request.UrlImage;
            user.Addres = request.Addres;
            user.DateBirth = request.DateBirth;

            var result= await _userManager.UpdateAsync(user);          

            if (!result.Succeeded)
            {
                generalResponse.result = false;
                foreach (var error in result.Errors)
                {
                    generalResponse.messages.Add(error.Description);
                }
                return generalResponse;
            }

            if ( !(request.FromProfile) )
            {
                var oldUserRoles = await _userManager.GetRolesAsync(user);

                //Delete old roles to add new
                foreach (var item in oldUserRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }

                foreach (var role in roles)
                {
                    await _userManager.AddToRoleAsync(user, role.ToString());
                }
            }

            generalResponse.messages.Add($"Datos actualizados exitosamente");

            return generalResponse;
        }

        public async Task<GeneralResponse<bool>> QuitStudentRoom(string idUser, int IdRoom)
        {
            GeneralResponse<bool> generalResponse = new();
            var teacher = await _userManager.FindByIdAsync(idUser);
            if (teacher != null)
            {
                string idRoom = IdRoom.ToString();

                if (!string.IsNullOrEmpty(teacher.IdsRoom))
                {
                    // Divide los valores actuales en una lista
                    var idsList = teacher.IdsRoom.Split(',').ToList();

                    // Convierte idRoom a string y verifica si existe en la lista
                    if (idsList.Contains(idRoom.ToString()))
                    {
                        idsList.Remove(idRoom.ToString()); // Elimina el idRoom de la lista
                        teacher.IdsRoom = string.Join(",", idsList); // Reconstruye la cadena
                    }
                    else
                    {
                        generalResponse.result = false;
                        generalResponse.messages.Add("El IdRoom no existe en la lista.");
                        return generalResponse;
                    }
                }
                var result = await _userManager.UpdateAsync(teacher);
            }

            generalResponse.messages.Add("Proceso realizado correctamente !!");
            return generalResponse;

        }
        public async Task<GeneralResponse<bool>> AddStudentRoom(string idUser, int IdRoom)
        {
            GeneralResponse<bool> generalResponse = new();
            var teacher = await _userManager.FindByIdAsync(idUser);
            if (teacher != null)
            {
                string idRoom = IdRoom.ToString();

                // Verifica si ya hay valores en IdsRoom
                if (!string.IsNullOrEmpty(teacher.IdsRoom))
                {                    
                    // Divide los valores existentes, verifica duplicados y agrega el nuevo idRoom
                    var idsList = teacher.IdsRoom.Split(',').ToList();
                    if (!idsList.Contains(idRoom))
                    {
                        idsList.Add(idRoom);
                    }
                    teacher.IdsRoom = string.Join(",", idsList); // Une los valores nuevamente
                }
                else
                {
                    // Si está vacío, asigna directamente el idRoom
                    teacher.IdsRoom = idRoom;
                }
                var result = await _userManager.UpdateAsync(teacher);
            }

            generalResponse.messages.Add("Proceso realizado correctamente !!");
            return generalResponse;
        }



        public async Task<GeneralResponse<string>> ConfirmEmailAsync(string userId, string token)
        {
            GeneralResponse<string> generalResponse = new();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("No account register this user");
                return generalResponse;
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user,token);
            if (!result.Succeeded)
            {
                generalResponse.result = false;
                foreach (var error in result.Errors)
                {
                    generalResponse.messages.Add(error.Description);
                }
                return generalResponse;
            }
            generalResponse.Data = $"Accout confirm for  {user.Email}. Now you can use our app";
            return generalResponse;
        }
        public async Task<GeneralResponse<string>> ForgotPasswordAync( ForgotPassswordRequest request,string origin)
        {
            GeneralResponse<string> generalResponse = new();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("No account register this user");
                return generalResponse;
            }
            string verificationUri = await SendForgotPasswordURL(user, origin);
            
            string emailTemplate = $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {{
                            font-family: Arial, Helvetica, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            border-radius: 10px;
                            padding: 30px;
                            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                        }}
                        h2 {{
                            color: #2c3e50;
                        }}
                        p {{
                            font-size: 15px;
                            color: #555555;
                            line-height: 1.6;
                        }}
                        .button {{
                            display: inline-block;
                            margin-top: 20px;
                            padding: 12px 25px;
                            background-color: #007bff;
                            color: #ffffff !important;
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 12px;
                            color: #999999;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <h2>Recuperación de Contraseña</h2>
                        <p>Hola <b>{user.FirstName + " " + user.LastName}</b>,</p>
                        <p>Hemos recibido una solicitud para restablecer la contraseña de tu cuenta. 
                           Si fuiste tú, haz clic en el siguiente botón para establecer una nueva contraseña:</p>
        
                        <a href=""{verificationUri}"" class=""button"">Restablecer Contraseña</a>
        
                        <p>Si no realizaste esta solicitud, puedes ignorar este correo de manera segura. 
                           Tu cuenta permanecerá protegida.</p>
        
                        <div class=""footer"">
                            <p>© {DateTime.Now.Year} - Smart System. Todos los derechos reservados.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await _emailService.SendAsync(
               new EmailRequest
               {
                   To = user.Email,
                   Body = emailTemplate,
                   Subject = "New Registered"
               }
               );
            generalResponse.Data = $"Revise su correo electrónico ({user.Email}) para restablecer su contraseña.";
            return generalResponse;
        }  
        public async Task<GeneralResponse<string>> ResetPasswordAsyn(ResetPasswordRequest request,string origin)
        {
            GeneralResponse<string> generalResponse = new();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("No account register this user");
                return generalResponse;
            }
            request.token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));
            var target= await _userManager.ResetPasswordAsync(user, request.token, request.Password);

            if (!target.Succeeded)
            {
                generalResponse.result = false;
                foreach (var error in target.Errors)
                {
                    generalResponse.messages.Add(error.Description);
                }
                return generalResponse;
            }
            generalResponse.Data = $"Contraseña cambiada para {user.Email}. Ahora puedes usar nuestra aplicación.";
            return generalResponse;
        }

        #region Private method
        private string GenerateStrongPassword(int length)
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-_=+<>?";

            string allChars = upperCase + lowerCase + digits + specialChars;
            char[] password = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];

                rng.GetBytes(randomBytes);

                for (int i = 0; i < length; i++)
                {
                    password[i] = allChars[randomBytes[i] % allChars.Length];
                }
            }

            // Garantizar que al menos tenga una mayúscula, minúscula, número y caracter especial
            Random rnd = new Random();
            password[rnd.Next(length)] = upperCase[rnd.Next(upperCase.Length)];
            password[rnd.Next(length)] = lowerCase[rnd.Next(lowerCase.Length)];
            password[rnd.Next(length)] = digits[rnd.Next(digits.Length)];
            password[rnd.Next(length)] = specialChars[rnd.Next(specialChars.Length)];

            return new string(password);
        }
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("UserName", user.UserName)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<string>  SendVerificationEmaiilURL(ApplicationUser user, string origin, bool apiURl = false)
        {

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string route = "";
            Uri Uri = null ;

            if (!apiURl)
            {
                route = "User/ConfirmEmail";
                Uri = new Uri(string.Concat($"{origin}/", route));
            }
            else
            {
                var url = _configuration["UrlAplication:Url"];
                route = "api/v1/Account/ConfirmEmail";
                Uri = new Uri(string.Concat($"{origin}/", route));
            }           

            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri.ToString(), "token", code);

            return verificationUri;
        } 
        private async Task<string>  SendForgotPasswordURL(ApplicationUser user, string origin)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //var route = "User/ResetPassword";
            //var Uri = new Uri(string.Concat($"{origin}/", route));

            string url = Environment.GetEnvironmentVariable("URL_Send_Forgot_Password")?.Trim() 
                ?? _configuration["URL_Send_Forgot_Password"];

            var verificationUri = QueryHelpers.AddQueryString(url, "token", code);

            return verificationUri;
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        #endregion
    }
}

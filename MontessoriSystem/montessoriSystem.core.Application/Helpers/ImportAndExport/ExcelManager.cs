using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.General;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Application.ViewModels.Helpers.ExcelManager;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Application.ViewModels.User;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MontessoriSystem.Core.Application.Helpers.ImportAndExport
{
    public class ExcelManager : IExcelManager
    {
        private readonly IStudentServices _studentServices;
        private readonly IUserService _userService;
        private readonly IProvinceDomService _provinceDom;
        private readonly IDepartmentService _departmentService;
        private readonly IEducationalInstitutionService _educationalInstitutionServices;
        private readonly IRoomService _roomService;
        private readonly ITypeRegisterService _typeRegisterService;
        private readonly IGradeService _gradeService;
        private readonly ISelectionService _selectionService;


        public ExcelManager(IStudentServices studentServices, IUserService userService, IProvinceDomService provinceDom, IDepartmentService departmentService, 
            IEducationalInstitutionService educationalInstitutionService, IRoomService roomService, ITypeRegisterService typeRegisterService, IGradeService gradeService, ISelectionService selectionService)
        {
            _studentServices = studentServices;
            _userService = userService;
            _provinceDom = provinceDom;
            _departmentService = departmentService;
            _educationalInstitutionServices = educationalInstitutionService;
            _roomService = roomService;
            _typeRegisterService = typeRegisterService;
            _gradeService = gradeService;
            _selectionService = selectionService;
            
        }
        public async Task<GeneralResponse<string>> StudentAndParentsImport(IFormFile excelFile)
        {
            GeneralResponse<string> generalResponse = new GeneralResponse<string>();
            List<StudentAndParentsViewModel> studentAndParentsViewModel = new List<StudentAndParentsViewModel>();

            try
            {

                #region Obtener los datos del excel

                var grades = await _gradeService.GetAllViewModel();

                if (excelFile != null && excelFile.Length > 0)
                {
                    // Configuración necesaria para ExcelDataReader
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    // Leemos el archivo recibido
                    using (var stream = new MemoryStream())
                    {
                        await excelFile.CopyToAsync(stream);
                        stream.Position = 0; // Resetear la posición del stream al inicio

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var table = result.Tables[0];

                            for (int i = 1; i < table.Rows.Count; i++)
                            {
                                var row = table.Rows[i];
                                int? gradeId = grades.FirstOrDefault(x => x.Name.Trim().Equals(row[12]?.ToString()?.Trim() ?? ""))?.Id ?? grades.FirstOrDefault().Id;

                                StudentAndParentsViewModel persona = new StudentAndParentsViewModel
                                {
                                    // Datos estudiantes
                                    Codigo = row[0]?.ToString(),
                                    Nombre = row[1]?.ToString(),
                                    Apellido1 = row[2]?.ToString(),
                                    Apellido2 = row[3]?.ToString(),
                                    FechaNacimientoEst = DateTime.TryParse(row[4]?.ToString(), out DateTime fechaNacEst) ? fechaNacEst : (DateTime?)null,
                                    Sexo = row[5]?.ToString(),
                                    Telefono = row[6]?.ToString(),
                                    NoFolio = FormatScientificNumber(row[7]?.ToString() ?? "") ,
                                    NoLibro = FormatScientificNumber(row[8]?.ToString() ??  ""),
                                    Email = row[9]?.ToString(),
                                    Nivel = row[10]?.ToString(),
                                    Grupo = row[11]?.ToString(),
                                    IdGrado = gradeId ?? 0,

                                    // Datos Representante #1
                                    RepNombre = row[13]?.ToString(),
                                    RepApellido1 = row[14]?.ToString(),
                                    RepApellido2 = row[15]?.ToString(),
                                    RepTelefono = row[16]?.ToString(),
                                    RepEmail = row[17]?.ToString(),
                                    Sexo_rep1 = row[18]?.ToString(),
                                    Cedula = row[19]?.ToString(),
                                    FechaNacimiento = DateTime.TryParse(row[20]?.ToString(), out DateTime fechaNac) ? fechaNac : (DateTime?)null,

                                    // Datos Representante #2
                                    Rep2Nombre = row[21]?.ToString(),
                                    Rep2Apellido1 = row[22]?.ToString(),
                                    Rep2Apellido2 = row[23]?.ToString(),
                                    Rep2Telefono = row[24]?.ToString(),
                                    Rep2Email = row[25]?.ToString(),
                                    SexoRep2 = row[26]?.ToString(),
                                    CedulaRep2 = row[27]?.ToString(),
                                    FechaNacimientoRep2 = DateTime.TryParse(row[28]?.ToString(), out DateTime fechaNacRep2) ? fechaNacRep2 : (DateTime?)null
                                };

                                studentAndParentsViewModel.Add(persona);
                            }
                        }
                    }
                }

                else
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add("El archivo Excel que intentaste cargar no es válido. " +
                        "Por favor, revisa que el formato y los datos sean correctos y vuelve a intentarlo. " +
                        "Si el problema persiste, asegúrate de que el archivo no esté dañado o ponte en contacto con soporte.");
                }

                #endregion

                #region Insertar los datos

                Random random = new Random();

                var students = await _studentServices.GetAllViewModel();

                foreach (var studentAndParentsView in studentAndParentsViewModel)
                {
                    int randomNumber = random.Next(10, 100);

                    #region Representante #1

                    SaveUserViewModel saveUserViewModelRep1 = new();
                    saveUserViewModelRep1.FirstName = studentAndParentsView.RepNombre ?? "";
                    saveUserViewModelRep1.LastName = studentAndParentsView?.RepApellido1 + " " + studentAndParentsView?.RepApellido2;
                    saveUserViewModelRep1.Tel = studentAndParentsView?.RepTelefono ?? "";
                    saveUserViewModelRep1.Email = studentAndParentsView?.RepEmail?.Replace(" ","") ?? "";
                    saveUserViewModelRep1.Gender = int.Parse(studentAndParentsView?.Sexo_rep1 ?? "1");
                    saveUserViewModelRep1.IdentificationId = studentAndParentsView?.Cedula ?? "";
                    saveUserViewModelRep1.DateBirth = studentAndParentsView?.FechaNacimiento ?? DateTime.Now;
                    saveUserViewModelRep1.UserName = CleanUsername($"{studentAndParentsView?.RepNombre}{studentAndParentsView?.RepApellido1}{randomNumber}").Replace(" ","");

                    var rep1 = await _userService.RegisterUsserAsync(saveUserViewModelRep1, new List<string> { Roles.Padre_Tutor.ToString() }, "");

                    #endregion

                    #region Representante #2

                    SaveUserViewModel saveUserViewModelRep2 = new();
                    saveUserViewModelRep2.FirstName = studentAndParentsView?.Rep2Nombre ?? "";
                    saveUserViewModelRep2.LastName = studentAndParentsView?.Rep2Apellido1 + " " + studentAndParentsView?.Rep2Apellido2;
                    saveUserViewModelRep2.Tel = studentAndParentsView?.Rep2Telefono ?? "";
                    saveUserViewModelRep2.Email = studentAndParentsView?.Rep2Email?.Replace(" ", "") ?? "";
                    saveUserViewModelRep2.Gender = int.Parse(studentAndParentsView?.SexoRep2 ?? "1");
                    saveUserViewModelRep2.IdentificationId = studentAndParentsView?.CedulaRep2 ?? "";
                    saveUserViewModelRep2.DateBirth = studentAndParentsView?.FechaNacimientoRep2 ?? DateTime.Now;
                    saveUserViewModelRep2.UserName = CleanUsername($"{studentAndParentsView?.Rep2Nombre}{studentAndParentsView?.Rep2Apellido1}{randomNumber}").Replace(" ","");

                    var rep2 = await _userService.RegisterUsserAsync(saveUserViewModelRep2, new List<string> { Roles.Padre_Tutor.ToString() }, "");

                    #endregion

                    #region Datos Estudiantes #2
                    
                    var existStudent = students.FirstOrDefault(est => est.Code == studentAndParentsView.Codigo);
                    var niveles = await _selectionService.GetEducationalLevel();

                    if (existStudent == null)
                    {
                        StudentSaveViewModel studentSaveViewModel = new StudentSaveViewModel();
                        studentSaveViewModel.Code = studentAndParentsView?.Codigo;
                        studentSaveViewModel.Name = studentAndParentsView.Nombre;
                        studentSaveViewModel.Lastname = studentAndParentsView?.Apellido1 + " " + studentAndParentsView?.Apellido2;

                        studentSaveViewModel.BornDate = studentAndParentsView?.FechaNacimientoEst?.ToString("yyyy-MM-dd")
                                                         ?? DateTime.Now.ToString("yyyy-MM-dd");

                        studentSaveViewModel.Tel = studentAndParentsView?.Telefono; 
                        studentSaveViewModel.Folio = studentAndParentsView?.NoFolio; 
                        studentSaveViewModel.Book = studentAndParentsView?.NoLibro; 
                        studentSaveViewModel.Sexo = studentAndParentsView?.Sexo ?? ""; 
                        studentSaveViewModel.Email = studentAndParentsView?.Email; 
                        studentSaveViewModel.Age = GetAge(studentAndParentsView?.FechaNacimientoEst ?? DateTime.Now);
                        studentSaveViewModel.IdGrade = studentAndParentsView?.IdGrado;

                        #region Validaciones para agregar informaciones al estudiante

                        if(rep1.Data == "6c7b0bb5-9033-4a11-96a8-f38f3e5e7d97")
                        {
                            Console.WriteLine("El ID del padre es el mismo que el ID de la madre.");
                        }

                        if (!string.IsNullOrEmpty(studentAndParentsView?.Nivel))
                        {
                            studentSaveViewModel.IdTypeRegister = niveles?.FirstOrDefault(x => x.text.ToLower()
                            .Equals(studentAndParentsView?.Nivel?.ToLower()
                            ))?.Id ?? 1;

                        }
                        else
                        {
                            studentSaveViewModel.IdTypeRegister = 1;
                        }

                        if (saveUserViewModelRep1.Gender == 1)
                        {
                            if ( !string.IsNullOrEmpty(rep1.Data) )
                                studentSaveViewModel.IdFather = rep1.Data;
                            studentSaveViewModel.TelFather = saveUserViewModelRep1.Tel;
                        }

                        else
                        {
                            if ( !string.IsNullOrEmpty(rep1.Data) )
                                studentSaveViewModel.IdMother = rep1.Data;
                            studentSaveViewModel.TelMother = saveUserViewModelRep1.Tel;
                        }

                        if (saveUserViewModelRep2.Gender == 1)
                        {
                            if (!string.IsNullOrEmpty(rep2.Data))
                                studentSaveViewModel.IdFather = rep2.Data;
                            studentSaveViewModel.TelFather = saveUserViewModelRep2.Tel;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(rep2.Data))
                                studentSaveViewModel.IdMother = rep2.Data;
                            studentSaveViewModel.TelMother = saveUserViewModelRep2.Tel;
                        }

                        #endregion

                        await _studentServices.Add(studentSaveViewModel);
                    }                    


                    #endregion

                }

                #endregion

            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("Ha ocurrido un error en el sistema. Por favor, inténtelo de nuevo o contacte al administrador. " + ex.Message );
                return generalResponse;
            }

            generalResponse.messages.Add("¡Estudiantes y representantes agregados con éxito!");
            return generalResponse;
        }

        public async Task<GeneralResponse<string>> TeacherImport(IFormFile excelFile, string origin, int IdInstitu)
        {
            GeneralResponse<string> generalResponse = new GeneralResponse<string>();
            List<SaveUserViewModel> teachers = new List<SaveUserViewModel>();

            try
            {

                #region Obtener los datos del excel

                var CivilStatus = new Dictionary<string, int>()
                {
                    { "soltero",1},
                    {"casado",1},
                    {"divorciado",3},
                    {"viudo",4},
                    { "separado",5},
                    { "en una relación",5},
                    { "comprometido",6}
                };

                var nacionalitis = new Dictionary<string, int>()
                {
                    { "dominicana",1},
                    { "argentina",2},
                    { "española",3},
                    {"chilena",4},
                    {"colombiana",5},
                    { "peruana",6},
                    { "venezolana",7},
                    {"cubana",8},
                    { "ecuatoriana",9},
                    { "boliviana",10}
                };

                if (excelFile != null && excelFile.Length > 0)
                {
                    // Configuración necesaria para ExcelDataReader
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    // Leemos el archivo recibido
                    using (var stream = new MemoryStream())
                    {
                        await excelFile.CopyToAsync(stream);
                        stream.Position = 0; // Resetear la posición del stream al inicio

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var table = result.Tables[0];
                            Random random = new Random();

                            for (int i = 1; i < table.Rows.Count; i++)
                            {
                                var row = table.Rows[i];
                                
                                int randomNumber = random.Next(10, 100);
                                var fecha_nac = DateTime.TryParse(row[4]?.ToString(), out DateTime fechaNacRep2) ? fechaNacRep2 : (DateTime?)null;
                                
                                SaveUserViewModel teacher = new SaveUserViewModel
                                {
                                    // Datos estudiantes
                                    FirstName = row[0].ToString()!,
                                    LastName = row[1]?.ToString() + " " + row[2]?.ToString(),
                                    Gender = int.Parse(row[3]?.ToString() ?? "1"),
                                    DateBirth = fecha_nac ?? DateTime.Now,
                                    IdentificationId = FormatearCardId(row[5]?.ToString() ?? ""),
                                    CivilStatus = CivilStatus.TryGetValue( row[6]?.ToString()?.ToLower() ?? "soltero", out int idCiSt) ? idCiSt.ToString() : "" ,
                                    Nationality = nacionalitis.TryGetValue(row[7]?.ToString()?.ToLower() ?? "soltero", out int idNacio) ? idNacio.ToString() : "",
                                    Email = row[8]?.ToString()?.Replace(" ", "") ?? "",
                                    PhoneNumber = FormatearTel(row[9]?.ToString() ??  ""),
                                    Addres = row[10]?.ToString() ?? "",
                                    UserName = CleanUsername($"{row[0].ToString()}{row[1].ToString()}{randomNumber}").Replace(" ", ""),
                                    Password = "123Pass$$word!"
                                };

                                teachers.Add(teacher);
                            }
                        }
                    }
                }

                else
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add("El archivo Excel que intentaste cargar no es válido. " +
                        "Por favor, revisa que el formato y los datos sean correctos y vuelve a intentarlo. " +
                        "Si el problema persiste, asegúrate de que el archivo no esté dañado o ponte en contacto con soporte.");
                }

                #endregion

                #region Insertar los datos

                string validatiionMessage = "Alguno(s) maestro(s) no se crearon, quizá(s) porque ya estaban registrado(s).";
                bool showValidatiionMessage = false;
                foreach (var teacher in teachers)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(10, 100);



                    var rep1 = await _userService.RegisterUsserAsync(teacher, new List<string> { Roles.Profesor.ToString() }, origin);

                    if (!rep1.result)
                    {
                        showValidatiionMessage = true;
                    }
                }

                generalResponse.messages.Add( $"¡Profesores agregados con éxito! { (showValidatiionMessage ? validatiionMessage: "" ) } ");
                return generalResponse;

                #endregion

            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("Ha ocurrido un error en el sistema. Por favor, inténtelo de nuevo o contacte al administrador. " + ex.Message);
                return generalResponse;
            }
        }
        public async Task<GeneralResponse<string>> AdministrativeImport(IFormFile excelFile, string origin)
        {
            GeneralResponse<string> generalResponse = new GeneralResponse<string>();
            List<SaveUserViewModel> Administratives = new List<SaveUserViewModel>();

            try
            {

                #region Obtener los datos del excel

                GeneralResponse<GendersCivStatuNacLevEducRelati> response = new();

                response = await _selectionService.GendersCivStatuNacLevEducRelati();
                var responseData = response.Data;

                if (excelFile != null && excelFile.Length > 0)
                {
                    // Configuración necesaria para ExcelDataReader
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    // Leemos el archivo recibido
                    using (var stream = new MemoryStream())
                    {
                        await excelFile.CopyToAsync(stream);
                        stream.Position = 0; // Resetear la posición del stream al inicio

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var table = result.Tables[0];
                            Random random = new Random();

                            for (int i = 1; i < table.Rows.Count; i++)
                            {
                                var row = table.Rows[i];

                                int randomNumber = random.Next(10, 100);
                                var fecha_nac = DateTime.TryParse(row[4]?.ToString(), out DateTime fechaNacRep2) ? fechaNacRep2 : (DateTime?)null;

                                SaveUserViewModel administrative = new SaveUserViewModel
                                {
                                    // Datos estudiantes
                                    UserName = CleanUsername($"{row[0].ToString()}{row[1].ToString()}{randomNumber}").Replace(" ", ""),
                                    FirstName = row[0].ToString()?.Trim()!,
                                    LastName = row[1]?.ToString()?.Trim() + " " + row[2]?.ToString()?.Trim(),
                                    Gender = int.Parse(row[3]?.ToString()?.Trim() ?? "1"),
                                    DateBirth = fecha_nac ?? DateTime.Now,
                                    IdentificationId = FormatearCardId(row[5]?.ToString()?.Trim() ?? ""),
                                    NoFolio = FormatScientificNumber(row[6]?.ToString()?.Trim() ?? ""),
                                    NoBook = FormatScientificNumber(row[7]?.ToString()?.Trim() ?? ""),                                    

                                    CivilStatus = responseData?.CivilStatus?
                                    .FirstOrDefault(x => x.text.ToLower().Equals(row[8]?.ToString()?.ToLower().Trim() ?? "soltero"))?
                                    .Id.ToString() ?? "",

                                    Nationality = responseData?.Nationality?
                                    .FirstOrDefault(x => x.text.ToLower().Equals(row[9]?.ToString()?.ToLower().Trim() ?? "dominicana"))?
                                    .Id.ToString() ?? "",

                                    IdNivelEducativo = responseData?.EducationalLevel?
                                    .FirstOrDefault(x => x.text.ToLower().Equals(row[10]?.ToString()?.ToLower().Trim() ?? "secundaria"))?
                                    .Id.ToString() ?? "",

                                    RelationshipId = responseData?.Relationship?
                                    .FirstOrDefault(x => x.text.ToLower().Equals(row[11]?.ToString()?.ToLower().Trim() ?? "padre"))?
                                    .Id.ToString() ?? "",

                                    Email = row[12]?.ToString()?.Trim()?.Replace(" ", "") ?? "",
                                    Tel = FormatearTel(row[13]?.ToString()?.Trim() ?? ""),
                                    PhoneNumber = FormatearTel(row[14]?.ToString()?.Trim() ?? ""),                                   
                                    Addres = row[15]?.ToString()?.Trim() ?? "",
                                    
                                    Profession = responseData?.Professions?
                                    .FirstOrDefault(x => x.text.ToLower().Equals(row[16]?.ToString()?.ToLower().Trim() ?? "ingeniero en sistemas"))?
                                    .Id.ToString() ?? "",

                                    Job = responseData?.Professions?
                                    .FirstOrDefault(x => x.text.ToLower().Equals(row[17]?.ToString()?.ToLower().Trim() ?? "ingeniero en sistemas"))?
                                    .Id.ToString() ?? "",

                                    Roles = !string.IsNullOrWhiteSpace(row[18]?.ToString()) 
                                            ? row[18].ToString().Trim().Split(",").ToList()
                                            : new List<string>()
                                };

                                Administratives.Add(administrative);
                            }
                        }
                    }
                }

                else
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add("El archivo Excel que intentaste cargar no es válido. " +
                        "Por favor, revisa que el formato y los datos sean correctos y vuelve a intentarlo. " +
                        "Si el problema persiste, asegúrate de que el archivo no esté dañado o ponte en contacto con soporte.");
                }

                #endregion

                #region Insertar los datos

                string validatiionMessage = "Alguno(s) maestro(s) no se crearon, quizá(s) porque ya estaban registrado(s).";
                bool showValidatiionMessage = false;
               
                foreach (var administrative in Administratives)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(10, 100);



                    var rep1 = await _userService.RegisterUsserAsync(administrative, administrative.Roles, origin);

                    if (!rep1.result)
                    {
                        showValidatiionMessage = true;
                    }
                }

                generalResponse.messages.Add($"¡Profesores agregados con éxito! {(showValidatiionMessage ? validatiionMessage : "")} ");
                return generalResponse;

                #endregion

            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("Ha ocurrido un error en el sistema. Por favor, inténtelo de nuevo o contacte al administrador. " + ex.Message);
                return generalResponse;
            }
        }

        public async Task<GeneralResponse<string>> EducationalCenterImport(IFormFile excelFile)
        {
            GeneralResponse<string> generalResponse = new GeneralResponse<string>();
            SaveEducationalInstitutionViewModel model = new();

            try
            {

                #region Obtener los datos del excel
               
                var provinceDomViews = await _provinceDom.GetAllViewModel();
                var departmentViewModels = await _departmentService.GetAllViewModel();

                if (excelFile != null && excelFile.Length > 0)
                {
                    // Configuración necesaria para ExcelDataReader
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    // Leemos el archivo recibido
                    using (var stream = new MemoryStream())
                    {
                        await excelFile.CopyToAsync(stream);
                        stream.Position = 0; // Resetear la posición del stream al inicio

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var table = result.Tables[0];
                            Random random = new Random();

                            for (int i = 1; i < table.Rows.Count; i++)
                            {
                                var row = table.Rows[i];

                                int randomNumber = random.Next(10, 100);
                                var fecha_nac = DateTime.TryParse(row[4]?.ToString(), out DateTime fechaNacRep2) ? fechaNacRep2 : (DateTime?)null;

                                model = new SaveEducationalInstitutionViewModel
                                {
                                    // Datos estudiantes
                                    Name = row[0].ToString()!,
                                    Address = row[1]?.ToString( )?? "",
                                    IdProvinceDom = provinceDomViews.FirstOrDefault(pro => pro?.Name?.ToLower() == row[2].ToString().Trim().ToLower())?.Id,
                                    IdDepartment = departmentViewModels.FirstOrDefault(pro => pro?.Name?.ToLower() == row[3].ToString().Trim().ToLower())?.Id,
                                    Phone = FormatearTel(row[4]?.ToString() ?? ""),
                                    Mobile = FormatearTel(row[5]?.ToString() ?? ""),
                                    IdRector = "",
                                    IdCordinator = "",
                                    IdSecretary = "",
                                    AcademicResolution = row[9]?.ToString() ?? "",
                                    EducationalRegistry = row[10]?.ToString() ?? "",
                                    Website = row[11]?.ToString() ?? "",
                                };
                               
                            }
                        }
                    }
                }

                else
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add("El archivo Excel que intentaste cargar no es válido. " +
                        "Por favor, revisa que el formato y los datos sean correctos y vuelve a intentarlo. " +
                        "Si el problema persiste, asegúrate de que el archivo no esté dañado o ponte en contacto con soporte.");
                }

                #endregion

                #region Insertar los datos

                await _educationalInstitutionServices.Add(model);
                generalResponse.messages.Add($"¡Centro Educativo creado con éxito!");
                return generalResponse;

                #endregion

            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("Ha ocurrido un error en el sistema. Por favor, inténtelo de nuevo o contacte al administrador. " + ex.Message);
                return generalResponse;
            }
        }

        public async Task<GeneralResponse<string>> RoomImport(IFormFile excelFile)
        {
            GeneralResponse<string> generalResponse = new GeneralResponse<string>();
            RoomSaveViewModel model = new();

            try
            {

                #region Obtener los datos del excel

                var provinceDomViews = await _provinceDom.GetAllViewModel();
                var departmentViewModels = await _departmentService.GetAllViewModel();
                var users = await _userService.GetAllUser();


                if (excelFile != null && excelFile.Length > 0)
                {
                    // Configuración necesaria para ExcelDataReader
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    // Leemos el archivo recibido
                    using (var stream = new MemoryStream())
                    {
                        await excelFile.CopyToAsync(stream);
                        stream.Position = 0; // Resetear la posición del stream al inicio

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var table = result.Tables[0];
                            Random random = new Random();

                            for (int i = 1; i < table.Rows.Count; i++)
                            {
                                var row = table.Rows[i];

                                int randomNumber = random.Next(10, 100);
                                var fecha_nac = DateTime.TryParse(row[4]?.ToString(), out DateTime fechaNacRep2) ? fechaNacRep2 : (DateTime?)null;

                                var nameTeacher = row[2]?.ToString().Trim().Split(" ");

                                var targetTeacher = users.Data.FirstOrDefault(x => x.FirstName.Equals(nameTeacher[0].ToString())  && x.LastName.Contains(nameTeacher[1]));

                                model = new RoomSaveViewModel
                                {
                                    // Datos estudiantes
                                    Name = row[0].ToString()!,
                                    Description = row[1]?.ToString() ?? "",
                                    IdTeacherLead = targetTeacher?.Id ?? "",
                                    Location = row[3]?.ToString() ?? "",
                                    Capacity = int.Parse( row[4]?.ToString() ?? "0"),
                                    IdTypeRegisters = await GetTypeIdLevels( row[5]?.ToString()?? "Inicial"),
                                };

                            }
                        }
                    }
                }

                else
                {
                    generalResponse.result = false;
                    generalResponse.messages.Add("El archivo Excel que intentaste cargar no es válido. " +
                        "Por favor, revisa que el formato y los datos sean correctos y vuelve a intentarlo. " +
                        "Si el problema persiste, asegúrate de que el archivo no esté dañado o ponte en contacto con soporte.");
                }

                #endregion

                #region Insertar los datos

                var educInstitu = await _educationalInstitutionServices.GetAllViewModel();
                model.IdEducationalInsti = educInstitu.FirstOrDefault().Id;

                await _roomService.Add(model);
                generalResponse.messages.Add($"¡Centro Educativo creado con éxito!");
                return generalResponse;

                #endregion

            }
            catch (Exception ex)
            {
                generalResponse.result = false;
                generalResponse.messages.Add("Ha ocurrido un error en el sistema. Por favor, inténtelo de nuevo o contacte al administrador. " + ex.Message);
                return generalResponse;
            }
        }


        #region Private Methods
        private int GetAge(DateTime date)
        {
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - date.Year;

            // Verificar si ya ha pasado el cumpleaños este año
            if (hoy < date.AddYears(edad))
            {
                edad--;
            }

            return edad;
        }
        private string CleanUsername(string username)
        {
            string result = Regex.Replace(username.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            return result;
        }

        private static string FormatScientificNumber(string input)
        {
            // Intenta convertir la cadena a un número
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
            {
                // Devuelve el número como cadena con un formato específico
                return number.ToString("F5", CultureInfo.InvariantCulture); // Cambia "F5" para la cantidad de decimales deseada
            }
            return input; // Si no se puede convertir, devolver el valor original
        }
        private string FormatearCardId(string cedula)
        {
            // Validar que la cuenta tenga 10 dígitos
            if (cedula.Length != 11)
            {
                return "000-0000000-0";
            }

            // Formatear el número de cuenta
            return $"{cedula.Substring(0, 3)}-{cedula.Substring(3, 7)}-{cedula.Substring(10, 1)}";
        }
        private string FormatearTel(string tel)
        {
            // Validar que el teléfono tenga 10 dígitos
            if (tel.Length != 10)
            {
                return "999-999-9999";
            }

            // Formatear el número de teléfono
            return $"{tel.Substring(0, 3)}-{tel.Substring(3, 3)}-{tel.Substring(6, 4)}";
        }
        private async Task<string> GetTypeIdLevels(string levels)
        {
            var typeRegisterViewModels = await _typeRegisterService.GetAllViewModel();

            var targetLevels = levels.Split(',');
            var ids = new List<string>();

            foreach (var level in targetLevels)
            {
                var levelsSelect = typeRegisterViewModels.FirstOrDefault(x => x.Name.ToLower() == level.ToLower());
                if (levelsSelect != null)
                {
                    ids.Add(levelsSelect.Id.ToString());
                }
            }

            return string.Join(",", ids);
        }
        #endregion

    }
}

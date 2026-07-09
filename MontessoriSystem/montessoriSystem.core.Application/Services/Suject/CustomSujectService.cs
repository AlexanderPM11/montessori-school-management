using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using System.Security.Claims;

namespace MontessoriSystem.Core.Application.Services.Suject
{
    public class CustomSujectService: ICustomSujectService
    {
        private readonly ISujectService _sujectService;
        private readonly IUserService _userService;
        private readonly IFileServices _fileServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string userName;

        public CustomSujectService(IUserService userService,IFileServices fileServices, ISujectService sujectService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _fileServices = fileServices;
            _sujectService = sujectService;
            _httpContextAccessor = httpContextAccessor;

            userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }
        public async Task<GeneralResponse<List<SujectViewModel>>> GetAllSubject()
        {
            GeneralResponse<List<SujectViewModel>> response = new();

            try
            {
                var userCurrent = await _userService.GetUserByName(userName);
                if (!userCurrent.Roles.Contains(Roles.Profesor.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permiso para visualizar estos datos");
                    return response;
                }
                var sujectViewModels = await _sujectService.GetAllViewModel();
                foreach (var suject in sujectViewModels)
                {
                    if (!string.IsNullOrEmpty(suject.ImageUrl))
                    {
                        var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/Suject/{suject.ImageUrl}");
                        suject.ImageUrl = responseBase64.Data;
                    }
                }

                response.Data = sujectViewModels.ToList();
                response.messages.Add("Lista de asignaturas obtenida correctamente.");
                return response;

            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);

                return response;
            }

        }
        public async Task<GeneralResponse<int>> CreateUpdate(SujectSaveViewModel model)
        {
            GeneralResponse<int> response = new();

            try
            {
                var userCurrent = await _userService.GetUserByName(userName);
                if (!userCurrent.Roles.Contains(Roles.Profesor.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permiso para visualizar estos datos");
                    return response;
                }

                bool deleteImg = false;

                var exitRegister = await _sujectService.GetBy(subj => subj.Id == model.Id);

                if (model.File != null && model.File.Length > 0)
                {
                    var pathNewFolder = $"Files/FileUser/Suject";
                    GeneralResponse<string> reponse = await _fileServices.CreateOrUpdateFile(model.File, pathNewFolder);

                    model.ImageUrl = reponse.Data;
                    deleteImg = true;
                }
                var DBModel = exitRegister.FirstOrDefault();

                if (deleteImg && exitRegister.Count > 0)
                {
                    string rutaArchivo = $"FileUser/Suject/{DBModel.ImageUrl}";
                    await _fileServices.DeleteFile(rutaArchivo);
                }
                if (model.Id > 0)
                {
                    if (!deleteImg && DBModel != null)
                    {
                        model.ImageUrl = DBModel.ImageUrl;
                    }

                    await _sujectService.Update(model, model.Id);
                    response.Data = model.Id;
                    response.messages.Add("Información actualizada correctamente!");
                }
                else
                {
                    model.Code = GenerateCode(model.Name);
                    var subjectAdded = await _sujectService.Add(model);
                    response.messages.Add("Asignatura creada correctamente!");
                    model.Id = subjectAdded.Id;
                }

                response.Data = model.Id;                
                return response;

            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);

                return response;
            }

        }
        public async Task<GeneralResponse<int>> Delete(int id)
        {
            GeneralResponse<int> response = new();

            try
            {
                if (id > 0)
                {
                    var userCurrent = await _userService.GetUserByName(userName);
                    if (!userCurrent.Roles.Contains(Roles.Profesor.ToString()))
                    {
                        response.result = false;
                        response.messages.Add("No tiene permiso para visualizar estos datos");
                        return response;
                    }

                    var subject = await _sujectService.GetBy(subj => subj.Id == id);

                    if (subject.Count > 0)
                    {
                        string rutaArchivo = $"FileUser/Suject/{subject?.FirstOrDefault()?.ImageUrl}";
                        await _fileServices.DeleteFile(rutaArchivo);
                    }
                    await _sujectService.Delete(id);

                    response.Data = id;
                    response.messages.Add("Asignatura eliminada correctamente!");
                    return response;
                }

            }
            catch (Exception ex)
            {
                var error = $"Ocurrió un error: {ex.Message}";
                response.result = false;
                response.messages.Add(error);

                return response;
            }

            response.result = false;
            response.messages.Add("Asignatura no encontrada!");
            return response;

        }


        #region  Private Methods
        public static string GenerateCode(string name)
        {
            var words = name.Split(' ');
            var code = "";

            foreach (var word in words)
            {
                if (word.Length >= 3)
                {
                    code += word.Substring(0, 3).ToUpper();
                }
                else
                {
                    code += word.ToUpper();
                }
            }

            return code;
        }

        #endregion
    }
}

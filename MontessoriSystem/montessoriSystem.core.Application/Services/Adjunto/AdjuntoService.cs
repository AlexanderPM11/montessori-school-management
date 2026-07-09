using Microsoft.AspNetCore.Http;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;
using System.Security.Claims;

namespace MontessoriSystem.Core.Application.Services.Adjunto
{
    public class CustomAdjuntoService: ICustAdjuntoService
    {

        private readonly IAdjuntoServices _adjuntoServices;
        private readonly IFileServices _fileServices;
        private readonly ITipoAdjuntoService _tipoAdjuntoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;


        private string userName;
        public CustomAdjuntoService( IAdjuntoServices adjuntoServices, IFileServices fileServices, 
            ITipoAdjuntoService tipoAdjuntoService, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _adjuntoServices = adjuntoServices;
            _fileServices = fileServices;
            _tipoAdjuntoService = tipoAdjuntoService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;

            userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        public async Task<GeneralResponse<List<AdjuntoViewModel>>> GetAdjunto(int idStudent)
        {
            GeneralResponse<List<AdjuntoViewModel>> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                var adjuntoViewModel = await _adjuntoServices.GetBy(ad => ad.IdStudent == idStudent);

                foreach (var item in adjuntoViewModel)
                {
                    var responseBase64 = await _fileServices.GetImageAsBase64($"DocumentStudent/{item.Path}");
                    
                    if (responseBase64.result)
                    {
                        item.Base64 = responseBase64.Data;
                    }
                }
                response.messages.Add("Consulta exitosa");
                response.Data = adjuntoViewModel;


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
        
        public async Task<GeneralResponse<int>> CreateOrUpdate( SaveAdjuntoViewModel model)
        {
            GeneralResponse<int> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                if (model.Name.Length > 200)
                {
                    model.Name = model.Name.Substring(0, 200);
                }
                if (model.Description.Length > 1000)
                {
                    model.Description = model.Description.Substring(0, 500);
                }
                if (model.Id == 0)
                {
                    if (model.Archivo == null || model.Archivo.Length <= 0)
                    {
                        response.result = false;
                        response.messages.Add("Seleccione un archivo!");
                        return response;

                    }
                }
                // Verificar si el archivo es un PDF o una imagen
                if (model.Archivo != null)
                {
                    if (!EsArchivoPermitido(model.Archivo))
                    {
                        response.result = false;
                        response.messages.Add("El archivo debe ser un PDF o una imagen (jpg, jpeg, png, gif).");
                        return response;

                    }
                }

                GeneralResponse<string> reponse = new GeneralResponse<string>();
                var idTipoAdjunto = 0;
                if (model.Archivo != null && model.Archivo.Length > 0)
                {
                    var pathNewFolder = $"Files/DocumentStudent";
                    reponse = await _fileServices.CreateOrUpdateFile(model.Archivo, pathNewFolder);

                    var tipoAdjuntoViewModels = await _tipoAdjuntoService.GetAllViewModel();

                    var extension = Path.GetExtension(model.Archivo!.FileName).ToLowerInvariant();

                    if (extension == ".pdf")
                    {
                        var tipoAdjuntoViewModel = tipoAdjuntoViewModels.Where(tipAdj => tipAdj.Description == TipoAdjunto.PDF.ToString()).FirstOrDefault();
                        idTipoAdjunto = tipoAdjuntoViewModel.Id;
                    }
                    else
                    {
                        var tipoAdjuntoViewModel = tipoAdjuntoViewModels.Where(tipAdj => tipAdj.Description != TipoAdjunto.PDF.ToString()).FirstOrDefault();
                        idTipoAdjunto = tipoAdjuntoViewModel.Id;
                    }
                }

                model.IdTipoAdjunto = idTipoAdjunto == 0 ? model.IdTipoAdjunto : idTipoAdjunto;
                model.Path = reponse.Data == "" || reponse.Data == null ? model.Path : reponse.Data;
                model.bytesAdjunto = 0;

                if (model.Id != 0)
                {
                    if (model.Archivo != null && model.Archivo.Length > 0)
                    {
                        var studentSaveViewModel = await _adjuntoServices.GetByIdSaveViewModel(model.Id);
                        string rutaArchivo = $"DocumentStudent/{studentSaveViewModel.Path}";

                        await _fileServices.DeleteFile(rutaArchivo);
                    }

                    await _adjuntoServices.Update(model, model.Id);
                }
                else
                {
                  var result= await _adjuntoServices.Add(model);
                    response.Data = result.Id;
                }

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

        public async Task<GeneralResponse<int>> Delete(int idAdjunto)
        {
            GeneralResponse<int> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);

                bool hasPermission = currentUser.Roles.Any(role => role.Equals(Roles.Profesor.ToString())
                || role.Equals(Roles.Admin.ToString()));

                if (!hasPermission)
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }
                var adjunto = await _adjuntoServices.GetBy(adj => adj.Id.Equals(idAdjunto));

                if (adjunto != null) {

                    await _adjuntoServices.Delete(idAdjunto);

                    string rutaArchivo = $"DocumentStudent/{adjunto?.FirstOrDefault()?.Path}";
                    await _fileServices.DeleteFile(rutaArchivo);

                    response.messages.Add("Registro eliminado con exito!");
                    response.Data = idAdjunto;


                }
                else
                {
                    response.result = false;
                    response.messages.Add("No se encontró el registro.");
                }


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

        #region Private Methods

        private bool EsArchivoPermitido(IFormFile archivo)
        {
            // Obtener la extensión del archivo
            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            // Comprobar si la extensión es de un PDF o una imagen (puedes agregar más extensiones según tus necesidades)
            return extension == ".pdf" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }

        #endregion

    }
}

using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Adjunto;

namespace montessoriSystem.Controllers
{
    public class AdjuntoController : Controller
    {
        private readonly IAdjuntoServices _adjuntoServices;
        private readonly ITipoAdjuntoService _tipoAdjuntoService;
        private readonly IFileServices _fileServices;
        private readonly string _wwwrootPath;

        public AdjuntoController(IAdjuntoServices adjuntoServices, IFileServices fileServices, IWebHostEnvironment webHostEnvironment, ITipoAdjuntoService tipoAdjuntoService)
        {
            _adjuntoServices = adjuntoServices;
            _fileServices = fileServices;
            _wwwrootPath = webHostEnvironment.WebRootPath;
            _tipoAdjuntoService = tipoAdjuntoService;
        }

        public async Task<PartialViewResult> PartialViewAdjunto(int idStudent)
        {
            //idStudent =2;
            var adjuntoViewModel = await _adjuntoServices.GetBy(ad => ad.IdStudent == idStudent);

            return PartialView(adjuntoViewModel);
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? idAdjunto)
        {
            if (idAdjunto != null && idAdjunto != 0)
            {
                var studentSaveViewModel = await _adjuntoServices.GetByIdSaveViewModel((int)idAdjunto);
                try
                {
                    string rutaArchivo = $"FileUser/DocumentStudent/{studentSaveViewModel.Path}";
                    var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                    byte[] contenidoArchivo = System.IO.File.ReadAllBytes(filePath);

                    var archivo = new FormFile(new MemoryStream(contenidoArchivo), 0, contenidoArchivo.Length, "archivo", studentSaveViewModel.Path);
                    studentSaveViewModel.Archivo = archivo;
                }
                catch (Exception ex)
                { }

                return PartialView(studentSaveViewModel);
            }
            return PartialView(new SaveAdjuntoViewModel());
        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdateAdjunto(SaveAdjuntoViewModel model)
        {
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
                    return Json(new { data = "Seleccione un archivo!", result = false });
                }
            }
            // Verificar si el archivo es un PDF o una imagen
            if (model.Archivo != null)
            {
                if (!EsArchivoPermitido(model.Archivo))
                {
                    return Json(new { data = "El tipo de archivo cargado no es permitido!", result = false });
                }
            }

            GeneralResponse<string> reponse = new GeneralResponse<string>();
            var idTipoAdjunto = 0;
            if (model.Archivo != null && model.Archivo.Length > 0)
            {
                var pathNewFolder = $"{_wwwrootPath}/FileUser/DocumentStudent/";
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
                    string rutaArchivo = $"FileUser/DocumentStudent/{studentSaveViewModel.Path}";
                    var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                    await _fileServices.DeleteFile(filePath);
                }

                await _adjuntoServices.Update(model, model.Id);
            }
            else
            {
                var response = await _adjuntoServices.Add(model);
            }

            return Json(new { data = "Registro creado con exito!", result = true });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int idAdjunto)
        {
            try
            {
                var studentSaveViewModel = await _adjuntoServices.GetByIdSaveViewModel((int)idAdjunto);
                string rutaArchivo = $"FileUser/DocumentStudent/{studentSaveViewModel.Path}";
                var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                await _fileServices.DeleteFile(filePath);

                await _adjuntoServices.Delete(idAdjunto);
                return Json(new { data = "Registro eliminado con exito!", result = true }); ;
            }
            catch (Exception ex)
            {
                return Json(new { data = "Ocurrio un error!" + ex.Message, result = false }); ;
            }
        }

        private bool EsArchivoPermitido(IFormFile archivo)
        {
            // Obtener la extensión del archivo
            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            // Comprobar si la extensión es de un PDF o una imagen (puedes agregar más extensiones según tus necesidades)
            return extension == ".pdf" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }
    }
}
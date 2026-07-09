using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.Room;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using System.Net;

namespace montessoriSystem.Controllers
{
    public class SujectController : Controller
    {
        private readonly ISujectService _sujectService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileServices _fileServices;
        private readonly string _wwwrootPath;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IInitDataHelper _initDataHelper;


        public SujectController(ISujectService sujectService,
            IUserService userService, IMapper mapper, IFileServices fileServices, IWebHostEnvironment webHostEnvironment, IValidateInfoDataBase validateInfoDataBase, IInitDataHelper initDataHelper)
        {
            _sujectService = sujectService;
            _userService = userService;
            _mapper = mapper;
            _fileServices = fileServices;
            _wwwrootPath = webHostEnvironment.WebRootPath;
            _validateInfoDataBase = validateInfoDataBase;
            _initDataHelper = initDataHelper;
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? id)
        {
            if (id != null  && id > 0)
            {
                var suject = await _sujectService.GetByIdSaveViewModel((int)id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    return PartialView("");
                }

                var roomoViewModels = await _sujectService.GetBy(teachingPerio =>
                 teachingPerio.Id == id);

                if (roomoViewModels != null && roomoViewModels.Count > 0)
                {
                    var model = _mapper.Map<SujectSaveViewModel>(roomoViewModels.FirstOrDefault());

                    return PartialView(model);
                }
            }

            return PartialView(new SujectSaveViewModel());

        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdatePost(SujectSaveViewModel model, int idInstitu)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var userName = User.Identity.Name;
                    var userCurrent = await _userService.GetUserByName(userName);
                    
                    if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Json("");
                    }
                    

                    bool deleteImg = false;

                    var exitRegister = await _sujectService.GetBy(educatio => educatio.Id == model.Id &&
                    educatio.IdEducationalInsti == idInstitu);

                    if (model.File != null && model.File.Length > 0)
                    {
                        var pathNewFolder = $"{_wwwrootPath}/FileUser/Suject";
                        GeneralResponse<string> reponse = await _fileServices.CreateOrUpdateFile(model.File, pathNewFolder);

                        model.ImageUrl = reponse.Data;
                        deleteImg = true;
                    }
                    var DBModel = exitRegister.FirstOrDefault();

                    if (deleteImg && exitRegister.Count > 0)
                    {
                        string rutaArchivo = $"FileUser/Suject/{DBModel.ImageUrl}";
                        var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                        await _fileServices.DeleteFile(filePath);
                    }


                    model.IdEducationalInsti = idInstitu;
                    if (model.Id > 0)
                    {
                        if (!deleteImg && DBModel != null)
                        {
                            model.ImageUrl = DBModel.ImageUrl;
                        }

                        await _sujectService.Update(model, model.Id);
                    }
                    else
                    {
                        var teachingPeriods = await _sujectService.Add(model);
                    }

                    return Json(new { data = "Información actualizada correctamente!", result = true });
                }

                return Json(new { data = "Datos sin validar!", result = false });
            }
            catch (Exception ex)
            {
                return Json(new { data = "Ocurrio un error!.. " + ex.Message, result = false });
            }


        }

        [HttpGet]
        public async Task<PartialViewResult> Sujects(int? idInstitu)
        {
            ViewBag.idInstitu = idInstitu;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetSujectsJson(int? idInstitu)
        {
            try
            {
                if (idInstitu != null && idInstitu > 0)
                {
                    var userName = User.Identity.Name;
                    var userCurrent = await _userService.GetUserByName(userName);

                    if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Json("");
                    }
                }  
                var sujectViewModels = await _sujectService.GetBy(teachingPerio =>
                teachingPerio.IdEducationalInsti == idInstitu);


                if (sujectViewModels != null && sujectViewModels.Count > 0)
                {
                    var model = _mapper.Map<List<SujectSaveViewModel>>(sujectViewModels);

                    return Json(model);
                }

                else
                {
                    return Json(new List<SujectSaveViewModel>());
                }

            }


            catch (Exception ex)
            {
                return Json("");
            }

        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            return Json(
               new
               {
                   data = "Accion no permitida por el momento!",
                   result = false
               }
            );
            if (id > 0)
            {
                var suject = await _sujectService.GetByIdSaveViewModel((int)id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(suject.IdEducationalInsti??0, userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("");
                }

                var  sujectViewModels = await _sujectService.GetBy(teachingPerio =>
               teachingPerio.Id == id);

                if (sujectViewModels.Count > 0)
                {
                    string rutaArchivo = $"FileUser/Suject/{sujectViewModels.FirstOrDefault().ImageUrl}";
                    var filePath = Path.Combine(_wwwrootPath, rutaArchivo);
                    await _fileServices.DeleteFile(filePath);
                }
                await _sujectService.Delete(id);
                return Json(
                new
                {
                    data = "Asignatura eliminada correctamente!",
                    result = true
                }
             );
            }

            return Json(
                new
                {
                    data = "Asignatura no encontrada!",
                    result = false
                }
             );
        }
    
}
}

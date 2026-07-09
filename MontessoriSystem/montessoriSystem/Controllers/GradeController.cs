using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using MontessoriSystem.Core.Application.ViewModels.Suject;
using System.Net;

namespace montessoriSystem.Controllers
{
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidateInfoDataBase _validateInfoDataBase;

        public GradeController(IGradeService gradeService, IUserService userService, IMapper mapper, IValidateInfoDataBase validateInfoDataBase)
        {
           
            _userService = userService;
            _mapper = mapper;
            _gradeService = gradeService;
            _validateInfoDataBase = validateInfoDataBase;
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? id)
        {
            if (id != null && id > 0)
            {
                var grade = await _gradeService.GetByIdSaveViewModel((int)id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    return PartialView("");
                }
                var gradeViewModels = await _gradeService.GetBy(grade =>
                 grade.Id == id);

                if (gradeViewModels != null && gradeViewModels.Count > 0)
                {
                    var model = _mapper.Map<GradeSaveViewModel>(gradeViewModels.FirstOrDefault());

                    return PartialView(model);
                }
            }

            return PartialView(new GradeSaveViewModel ());

        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdatePost(GradeSaveViewModel model, int idInstitu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.Name;
                    var userCurrent = await _userService.GetUserByName(userName);

                    if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                    {
                        return Json(new { message = "" });
                    }


                    model.IdEducationalInsti = idInstitu;
                    if (model.Id > 0)
                    {                       
                        await _gradeService.Update(model, model.Id);
                    }
                    else
                    {
                        var grade = await _gradeService.Add(model);
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
        public async Task<PartialViewResult> Grades(int? idInstitu)
        {
            ViewBag.idInstitu = idInstitu;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetGradesJson(int? idInstitu)
        {
            try
            {
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    return Json(new { message = "" });
                }
                var gradeViewModels = await _gradeService.GetAllViewModel();


                if (gradeViewModels != null && gradeViewModels.Count > 0)
                {
                    var model = _mapper.Map<List<GradeSaveViewModel>>(gradeViewModels);

                    return Json(model);
                }

                else
                {
                    return Json(new List<GradeSaveViewModel>());
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
                   data = "Opcion deshabilitada por el momento",
                   result = false
               }
            );

            if (id > 0)
            {
                var grade = await _gradeService.GetByIdSaveViewModel(id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    return Json(new { message = "" });
                }

                await _gradeService.Delete(id);
                return Json(
                new
                {
                    data = "Grado eliminado correctamente!",
                    result = true
                }
             );
            }

            return Json(
                new
                {
                    data = "Grado no encontrado!",
                    result = false
                }
             );
        }
    }
}

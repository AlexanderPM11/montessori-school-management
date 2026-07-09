using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Application.ViewModels.TeachingPeriods;
using MontessoriSystem.Core.Application.ViewModels.TipoAdjunto;
using System.Net;

namespace montessoriSystem.Controllers
{
    public class TeachingPeriodController : Controller
    {
        private readonly ITeachingPeriodsService _teachingPeriods;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidateInfoDataBase _validateInfoDataBase;

        public TeachingPeriodController(ITeachingPeriodsService teachingPeriods,
            IUserService userService, IMapper mapper, IValidateInfoDataBase validateInfoDataBase)
        {
            _teachingPeriods = teachingPeriods;
            _userService = userService;
            _mapper = mapper;
            _validateInfoDataBase = validateInfoDataBase;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? id)
        {           
            if(id != null && id > 0)
            {
                var teachinPeriod = await _teachingPeriods.GetByIdSaveViewModel((int)id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return PartialView("");
                }

                var teachingPeriods = await _teachingPeriods.GetBy(teachingPerio =>
                 teachingPerio.Id == id);

                if (teachingPeriods != null && teachingPeriods.Count > 0)
                {
                    var model = _mapper.Map<TeachingPeriodsSaveViewModel>(teachingPeriods.FirstOrDefault());

                    return PartialView(model);
                }
            }

            return PartialView(new TeachingPeriodsSaveViewModel());

        }
        
        [HttpPost]
        public async Task<JsonResult> CreateUpdatePost( TeachingPeriodsSaveViewModel model, int idInstitu)
        {
            try
            {
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);
                
                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("");
                }

                if (ModelState.IsValid)
                {
                    if(!DateValidator(model.DateInit, model.DateFinish)){
                        return Json(
                            new { data = "Fechas inválidas. ¡Asegúrese de colocar una fecha lógica!", 
                            result = false }
                         );
                    }
                    model.IdEducationalInsti = idInstitu;
                    if(model.Id > 0)
                    {                        
                        await _teachingPeriods.Update(model, model.Id);
                    }
                    else
                    {
                        var teachingPeriods = await _teachingPeriods.Add(model);
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
        public async Task<PartialViewResult> TeachingPeriods(int? idInstitu)
        {
            ViewBag.idInstitu = idInstitu;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetTeachingPeriodsJson(int? idInstitu)
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

                var teachingPeriods = await _teachingPeriods.GetBy(teachingPerio =>
                teachingPerio.IdEducationalInsti == idInstitu);

               
                if (teachingPeriods != null && teachingPeriods.Count > 0)
                {
                    var model = _mapper.Map<List<TeachingPeriodsSaveViewModel>>(teachingPeriods);

                    return Json(model);
                }
            
                else
                {
                    return Json(new List<TeachingPeriodsSaveViewModel>());
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
            if (id >  0)
            {
                var teachingPerio = await _teachingPeriods.GetByIdSaveViewModel((int)id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (!userCurrent.Roles.Contains(Roles.Admin.ToString()))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("");
                }

                await _teachingPeriods.Delete(id);
                return Json(
                new
                {
                    data = "Periodo eliminado correctamente!",
                    result = true
                }
             );
            }

            return Json(
                new
                {
                    data = "Periodo no encontrado!",
                    result = false
                }
             );
        }

        #region private
        public bool DateValidator(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion




    }
}

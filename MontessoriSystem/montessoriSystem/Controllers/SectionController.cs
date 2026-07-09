using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.ViewModels.Section;
using MontessoriSystem.Core.Application.ViewModels.Student;
using Newtonsoft.Json;

namespace montessoriSystem.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionServices _sectionServices;
        private readonly IInitDataHelper _initDataHelper;

        public SectionController( ISectionServices sectionServices, IInitDataHelper initDataHelper)
        {
            _sectionServices = sectionServices;
            _initDataHelper = initDataHelper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return RedirectToAction(action, controller);
            }
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetDatosSeccionesJson()
        {
            try
            {
                var DataFilterViewModel = await _sectionServices.GetAllViewModel();
                var jsonResult = JsonConvert.SerializeObject(DataFilterViewModel, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return Json(DataFilterViewModel);
            }
            catch (Exception ex)
            {

                return Json("");
            }

        }
        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? idSection)
        {
            if (idSection != null && idSection != 0)
            {
                var studentSaveViewModel = await _sectionServices.GetByIdSaveViewModel((int)idSection);
                return PartialView(studentSaveViewModel);
            }
            return PartialView(new SectionSaveViewModel());

        }
        [HttpPost]
        public async Task<PartialViewResult> CreateUpdateSection(SectionSaveViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    await _sectionServices.Update(model, model.Id);
                }
                else
                {
                    var studentSaveViewModel = await _sectionServices.Add(model);
                }

            }
            else
            {
                return PartialView("CreateUpdate", model);
            }

            return PartialView("Index");

        }
        [HttpPost]
        public async Task<PartialViewResult> ActiveInactiveSection(int idSection)
        {
            await _sectionServices.ActiveInactiveSection(idSection);
            return PartialView("Index");
        }

    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers.InitData;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Department;
using System.Collections.Generic;
using System.Net;

namespace montessoriSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IInitDataHelper _initDataHelper;


        public DepartmentController(
            IUserService userService, IMapper mapper, IValidateInfoDataBase validateInfoDataBase, IDepartmentService departmentService, IInitDataHelper initDataHelper)
        {

            _userService = userService;
            _mapper = mapper;
            _validateInfoDataBase = validateInfoDataBase;
            _departmentService = departmentService;
            _initDataHelper = initDataHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            (string action, string controller) = await _initDataHelper.ValidatedInitData();

            if (action != null)
            {
                return PartialView(action, controller);
            }
            return View();
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? id)
        {
            if (id != null && id > 0)
            {
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsIdUserValidAsync(userCurrent.Id, userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return PartialView( "" );
                }
                var departmentViewModels = await _departmentService.GetBy(department =>
                 department.Id == id);

                if (departmentViewModels != null && departmentViewModels.Count > 0)
                {
                    var model = _mapper.Map<DepartmentSaveViewModel>(departmentViewModels.FirstOrDefault());

                    return PartialView(model);
                }
            }

            return PartialView(new DepartmentSaveViewModel());

        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdatePost(DepartmentSaveViewModel model)
        {
            try
            {
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                if (model.Id > 0)
                {
                    var modelOld = await _departmentService.GetByIdSaveViewModel(model.Id);
                    model.IdUserCreator = modelOld.IdUserCreator;

                    await _departmentService.Update(model, model.Id);
                }
                else
                {
                    model.IdUserCreator = userCurrent.Id;
                    var department = await _departmentService.Add(model);
                }

                return Json(new { data = "Información actualizada correctamente!", result = true });
            }
            catch (Exception ex)
            {
                return Json(new { data = "Ocurrio un error!.. " + ex.Message, result = false });
            }


        }

        [HttpGet]
        public async Task<PartialViewResult> departments()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetdepartmentsJson()
        {
            try
            {
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsIdUserValidAsync(userCurrent.Id , userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { message = "" });
                }
                var departmentViewModels = new List<DepartmentViewModel>();
                if (userCurrent.Roles.Contains(Roles.SuperAdmin.ToString())) {
                    departmentViewModels = await _departmentService.GetBy(department =>
                        department.IdUserCreator == userCurrent.Id);
                }
                else
                {
                    departmentViewModels = await _departmentService.GetBy(department =>
                        department.IdUserCreator == userCurrent.IdUserCreator);
                }
               


                if (departmentViewModels != null && departmentViewModels.Count > 0)
                {
                    var model = _mapper.Map<List<DepartmentSaveViewModel>>(departmentViewModels);

                    return Json(model);
                }

                else
                {
                    return Json(new List<DepartmentSaveViewModel>());
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

            if (id > 0)
            {

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsIdUserValidAsync(userCurrent.Id, userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { message = "" });
                }

                await _departmentService.Delete(id);
                return Json(
                new
                {
                    data = "Departamento eliminado correctamente!",
                    result = true
                }
             );
            }

            return Json(
                new
                {
                    data = "Departamento no encontrado!",
                    result = false
                }
             );
        }
    }
}

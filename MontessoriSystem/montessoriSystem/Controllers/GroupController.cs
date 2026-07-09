using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.ViewModels.Grade;
using MontessoriSystem.Core.Application.ViewModels.Group;
using MontessoriSystem.Core.Application.ViewModels.GroupGrade;
using MontessoriSystem.Core.Domain.Entities;
using System.Net;

namespace montessoriSystem.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGradeService _gradeService;
        private readonly IGroupService _groupService;
        private readonly IGroupGradeService _groupGradeService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidateInfoDataBase _validateInfoDataBase;
        private readonly IRoomService _roomService;
        public GroupController(IGradeService gradeService, 
            IUserService userService, IMapper mapper, IValidateInfoDataBase validateInfoDataBase,
            IGroupService groupService, IGroupGradeService groupGradeService, IRoomService roomService)
        {
            _userService = userService;
            _mapper = mapper;
            _gradeService = gradeService;
            _validateInfoDataBase = validateInfoDataBase;
            _groupService = groupService;
            _groupGradeService = groupGradeService;
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateUpdate(int? id,int idInstitu, int IdRoom)
        {
            var room = await _roomService.GetByIdSaveViewModel(IdRoom);
            if (room == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return PartialView("");
            }
            ViewBag.IdsGrade= await GetGrade(room.IdEducationalInsti ?? 0);
            if (id != null && id > 0)
            {
                var group = await _groupService.GetByIdSaveViewModel((int)id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(room.IdEducationalInsti ?? 0,
                    userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return PartialView();
                }

                var groupViewModels = await _groupGradeService.GetAllWithIncludeViewModel(
                    new List<string> { "GroupCenter" }, grp => grp.GroupId == id);

               

                if (groupViewModels != null && groupViewModels.Count > 0)
                {
                    var model = _mapper.Map<GroupSaveViewModel>(groupViewModels.FirstOrDefault().GroupCenter);
                    foreach (var item in groupViewModels)
                    {
                        model.IdsGrade.Add(item.GradeId);
                    }

                    return PartialView(model);
                }
            }

            return PartialView(new GroupSaveViewModel());

        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdatePost(GroupSaveViewModel model, int IdRoom)
        {
            try
            {
                var room = await _roomService.GetByIdSaveViewModel(IdRoom);
                if (room == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("");
                }
                ModelState.Remove(nameof(model.IdsGrade));
                if (ModelState.IsValid)
                {
                    var userName = User.Identity.Name;
                    var userCurrent = await _userService.GetUserByName(userName);

                    bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(room.IdEducationalInsti ?? 0, userCurrent);

                    if (!isAuthorized)
                    {
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Json(new { message = "" });
                    }


                    model.IdRoom = IdRoom;
                    if (model.Id > 0)
                    {
                       await DeleteGroupGrades(model.Id);
                       await _groupService.Update(model, model.Id);

                        foreach (var grade in model.IdsGrade)
                        {
                            var groupGrade = new GroupGradeSaveViewModel
                            {
                                GroupId = model.Id,
                                GradeId = grade
                            };

                            var groupGradeData = await _groupGradeService.Add(groupGrade);
                        }
                        //await _groupService.Update(model, model.Id);
                    }
                    else
                    {
                        var group = await _groupService.Add(model);
                        foreach (var grade in model.IdsGrade)
                        {
                            var groupGrade = new GroupGradeSaveViewModel
                            {
                                GroupId = group.Id,
                                GradeId = grade
                            };

                            var groupGradeData = await _groupGradeService.Add(groupGrade);
                        }
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
        public async Task<PartialViewResult> GroupCenter(int? IdRoom)
        {
            ViewBag.IdRoom = IdRoom;
            return PartialView();
        }

        [HttpGet]
        public async Task<JsonResult> GetGroupsJson(int? idInstitu, int IdRoom)
        {
            try
            {
                var room = await _roomService.GetByIdSaveViewModel(IdRoom);
                if (room == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("");
                }
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(room.IdEducationalInsti??0, userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { message = "" });
                }

                var groupViewModels = await _groupService.GetBy(grp =>
                grp.IdRoom == IdRoom);

                List<GroupSaveViewModel> groupSaveViewModels = new List<GroupSaveViewModel>();
                var grades = await _gradeService.GetBy(gra => gra.IdEducationalInsti == idInstitu);

                foreach (var item in groupViewModels)
                {
                    var groupGrade = await _groupGradeService.GetAllWithIncludeViewModel(
                    new List<string> { "GroupCenter" }, grp => grp.GroupId == item.Id);
                    var model = _mapper.Map<GroupSaveViewModel>(groupGrade.FirstOrDefault().GroupCenter);                   
                    
                    foreach (var grd in groupGrade)
                    {
                        var gradesModel = grades.Where(g => g.Id == grd.GradeId).FirstOrDefault();
                        model.Grades.Add(gradesModel.Name);
                    }
                    groupSaveViewModels.Add(model);

                }               

                if (groupSaveViewModels.Count > 0)
                {
                    return Json(groupSaveViewModels);
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
        [HttpGet]
        public async Task<PartialViewResult> SeeGroup(int? idInstitu, int idGroup, int IdRoom)
        {

            try
            {
                var room = await _roomService.GetByIdSaveViewModel(IdRoom);
                if (room == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return PartialView("");
                }
                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(room.IdEducationalInsti ?? 0, userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return PartialView(new { message = "" });
                }

                var groupViewModels = await _groupService.GetBy(grp =>
                grp.IdRoom == IdRoom && grp.Id == idGroup);

                List<GroupSaveViewModel> groupSaveViewModels = new List<GroupSaveViewModel>();
                var grades = await _gradeService.GetBy(gra => gra.IdEducationalInsti == idInstitu);

                foreach (var item in groupViewModels)
                {
                    var groupGrade = await _groupGradeService.GetAllWithIncludeViewModel(
                    new List<string> { "GroupCenter" }, grp => grp.GroupId == item.Id);
                    var model = _mapper.Map<GroupSaveViewModel>(groupGrade.FirstOrDefault().GroupCenter);

                    foreach (var grd in groupGrade)
                    {
                        var gradesModel = grades.Where(g => g.Id == grd.GradeId).FirstOrDefault();
                        model.GradeViewModels.Add(gradesModel);
                    }
                    groupSaveViewModels.Add(model);

                }

                if (groupSaveViewModels.Count > 0)
                {
                    return PartialView(groupSaveViewModels);
                }
                else
                {
                    return PartialView(new List<GroupSaveViewModel>());
                }

            }


            catch (Exception ex)
            {
                return PartialView("");
            }


        }
        
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {

            if (id > 0)
            {
                var group = await _groupService.GetByIdSaveViewModel(id);

                var room = await _roomService.GetByIdSaveViewModel(group.IdRoom);
                if (room == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json("");
                }
                var targetgrade = await _groupService.GetBy(grp =>
                grp.IdRoom == room.IdEducationalInsti && grp.Id == id);

                var userName = User.Identity.Name;
                var userCurrent = await _userService.GetUserByName(userName);

                bool isAuthorized = await _validateInfoDataBase.IsInstitutionIdValidAsync(room.IdEducationalInsti ?? 0, userCurrent);

                if (!isAuthorized)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Json(new { message = "" });
                }

                await _groupService.Delete(id);
                return Json(
                new
                {
                    data = "Grupo eliminado correctamente!",
                    result = true
                }
             );
            }

            return Json(
                new
                {
                    data = "Grupo no encontrado!",
                    result = false
                }
             );
        }


        #region Private Method

        private async Task<List<ClassSelected<int>>> GetGrade(int idInstitu)
        {

            var grades = await _gradeService.GetBy(gra=>gra.IdEducationalInsti == idInstitu);            

            List<ClassSelected<int>> selectedGrades = new List<ClassSelected<int>>();

            foreach (var grade in grades)
            {
                ClassSelected<int> classSelected = new ClassSelected<int>
                {
                    Id = grade.Id,
                    text = grade.Name,
                };

                selectedGrades.Add(classSelected);
            }
            return selectedGrades;
        }
        private async Task<bool> DeleteGroupGrades(int idGroup)
        {

            var groupGrades = await _groupGradeService.GetBy(gr=>gr.GroupId == idGroup);
            foreach (var gGra in groupGrades)
            {
               await _groupGradeService.Delete(gGra.Id);
            }
            return true;
        }


        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.Department;
using MontessoriSystem.Core.Application.ViewModels.EducationalInstitution;
using MontessoriSystem.Core.Application.ViewModels.InstitutionalCenterUsers;
using MontessoriSystem.Core.Application.ViewModels.Student;
using MontessoriSystem.Core.Application.ViewModels.User;
using MontessoriSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Core.Application.Services.CustomInstitutionService
{
    public class CustomInstitutionService : ICustomInstitutionService
    {
        private readonly IEducationalInstitutionService _educationalInstitutionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly IInstitutionalCenterUsersService _institutionalCenterUsersService;
        private readonly IFileServices _fileServices;


        private string userName;
        public CustomInstitutionService(IEducationalInstitutionService educationalInstitutionServices, 
            IHttpContextAccessor httpContextAccessor, IUserService userService, IDepartmentService departmentService,
            IInstitutionalCenterUsersService institutionalCenterUsersService, IFileServices fileServices)
        {
            _educationalInstitutionServices = educationalInstitutionServices;
            _httpContextAccessor = httpContextAccessor;
            userName = _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            _userService = userService;
            _departmentService = departmentService;
            _institutionalCenterUsersService = institutionalCenterUsersService;
            _fileServices = fileServices;
        }

        public async Task<GeneralResponse<EducationalInstitutionViewModel>> GetInstituCenter()
        {
            GeneralResponse<EducationalInstitutionViewModel> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);
                if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }
                var allUser = await _userService.GetAllUser();

                var educationalInstitutionViewModel = await _educationalInstitutionServices.GetAllViewModel();
                response.Data = educationalInstitutionViewModel.FirstOrDefault();

                var rector = allUser.Data.FirstOrDefault(u => u.Id == response?.Data?.IdRector);
                if (rector != null)
                {
                    response.Data.NameRector = $"{rector.FirstName} {rector.LastName}";
                }

                var corditator = allUser.Data.FirstOrDefault(u => u.Id == response?.Data?.IdCordinator);
                if (corditator != null)
                {
                    response.Data.NameCordinator = $"{corditator.FirstName} {corditator.LastName}";
                }

                var secretary = allUser.Data.FirstOrDefault(u => u.Id == response?.Data?.IdSecretary);
                if (secretary != null)
                {
                    response.Data.NameSecretary = $"{secretary.FirstName} {secretary.LastName}";
                }


                if (response?.Data?.UrlLogo != null)
                {
                    var responseBase64 = await _fileServices.GetImageAsBase64($"FileUser/EducantionalInstitution/{response.Data.UrlLogo}");
                    response.Data.UrlLogo = responseBase64.Data;
                }

                if (educationalInstitutionViewModel != null && educationalInstitutionViewModel.Count > 0)
                {
                    var departmentViewModels = new List<DepartmentViewModel>();

                    if (currentUser.Roles.Contains(Roles.SuperAdmin.ToString()))
                    {
                        departmentViewModels = await _departmentService.GetBy(department =>
                            department.IdUserCreator == currentUser.Id);
                    }
                    else
                    {
                        departmentViewModels = await _departmentService.GetBy(department =>
                            department.IdUserCreator == currentUser.IdUserCreator);
                    }
                    
                }               

                return response;
            }
            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                response.result = false;

                return response;
            }

        }

        public async Task<GeneralResponse<string>> CreateUpdate(SaveEducationalInstitutionViewModel model)
        {
            GeneralResponse<string> response = new();

            try
            {
                var currentUser = await _userService.GetUserByName(userName);
                if (!currentUser.Roles.Contains(Roles.Admin.ToString()))
                {
                    response.result = false;
                    response.messages.Add("No tiene permisos para realizar esta acción");

                    return response;
                }

                bool deleteImg = false;
                var exitRegister = new List<EducationalInstitutionViewModel>();

                exitRegister = await _educationalInstitutionServices.GetAllViewModel();
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                if (model.File != null && model.File.Length > 0)
                {
                    var pathNewFolder = $"FileUser/EducantionalInstitution";
                    var filePath = Path.Combine(uploadsPath, pathNewFolder);

                    GeneralResponse<string> responseFile = await _fileServices.CreateOrUpdateFile(model.File, filePath);
                    model.UrlLogo = responseFile.Data;
                    deleteImg = true;
                }

                if (exitRegister != null && exitRegister.Count > 0)
                {
                    var DBModel = exitRegister.FirstOrDefault();

                    if (!string.IsNullOrEmpty(DBModel.UrlLogo) && deleteImg)
                    {
                        var modelDB = await _educationalInstitutionServices.GetByIdSaveViewModel(model.Id);

                        string rutaArchivo = $"FileUser/EducantionalInstitution/{modelDB.UrlLogo}";
                        var filePath = Path.Combine(uploadsPath, rutaArchivo);

                        await _fileServices.DeleteFile(filePath);

                    }
                    if (!deleteImg)
                    {
                        model.UrlLogo = DBModel.UrlLogo;
                    }

                    model.LastModifiedBy = currentUser.UserName;

                    model.IsMainSchool = DBModel!.IsMainSchool;
                    model.IdUser = DBModel!.IdUser;
                    model.UserAssignmentId = DBModel!.UserAssignmentId;

                    await _educationalInstitutionServices.Update(model, DBModel.Id);

                    response.Data = "Información actualizada correctamente!";
                    return response;
                }

                else
                {
                    model.IdUser = currentUser.Id;

                    model.CreatedBy = currentUser.UserName;
                    model.IsMainSchool = true;

                    var centerAdded = await _educationalInstitutionServices.Add(model);
                    InstitutionalCenterUsersSaveViewModel InsCenterUsersSaveViewModel = new InstitutionalCenterUsersSaveViewModel { UserId = currentUser.Id!, CenterId = centerAdded.Id };

                    await _institutionalCenterUsersService.Add(InsCenterUsersSaveViewModel);

                    response.Data = "Información actualizada correctamente!";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                response.result = false;

                return response;
            }
            

        }


    }
}

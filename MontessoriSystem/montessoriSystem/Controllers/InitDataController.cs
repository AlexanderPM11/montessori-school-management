using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MontessoriSystem.Core.Application.Enums;
using MontessoriSystem.Core.Application.Interface.Helpers;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Services;
using MontessoriSystem.Core.Application.Services.User;
using MontessoriSystem.Core.Application.ViewModels.InitData;

public class InitDataController : Controller
{
    private readonly IInitDataService _initDataService;
    private readonly string _wwwrootPath;
    private readonly IExcelManager _excelManager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IInitDataHelper _initDataHelper;


    public InitDataController(IInitDataService initDataService, IWebHostEnvironment webHostEnvironment, 
        IExcelManager excelManager, IMapper mapper, IUserService userService, IInitDataHelper initDataHelper)
    {
        _initDataService = initDataService;
        _wwwrootPath = webHostEnvironment.WebRootPath;
        _excelManager = excelManager;
        _mapper = mapper;
        _userService = userService;
        _initDataHelper = initDataHelper;
    }

    public async Task<IActionResult> Index()
    {
        (string action,string controller) = await _initDataHelper.ValidatedInitData();
        
        if(action != null)
        {
            return RedirectToAction(action, controller);
        }

        return RedirectToAction("Index", "Dashboard");
      
    }


    #region Manejo EducationalCenter By Excel

    [HttpGet]
    public IActionResult FillEducationalCenter()
    {
        return View();
    }

    [HttpGet]
    public IActionResult DownloadTemplateExcelEducationalCenter()
    {
        var filePath = Path.Combine(_wwwrootPath, "Templates", "Plantilla_CentroEducativo.xlsx");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("La plantilla de Excel no se encontró.");
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Plantilla_CentroEducativo.xlsx");
    }    

    [HttpPost]
    public async Task<JsonResult> CreateEducationalCenterExcel(IFormFile excelFile)
    {

        var response = await _excelManager.EducationalCenterImport(excelFile);

        await SetInitData(InitDataEnum.EducationalCenter);

        return Json(new { data = response.messages.FirstOrDefault(), result = response.result });

    }

    #endregion

    #region Manejo Teacher By Excel
    
    [HttpGet]
    public async Task<IActionResult> FillTeachers()
    {
        return View();
    }

    [HttpGet]
    public IActionResult DownloadTeacherTemplateExcel()
    {
        var filePath = Path.Combine(_wwwrootPath, "Templates", "PlantillaTeacher.xlsx");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("La plantilla de Excel no se encontró.");
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PlantillaTeacher.xlsx");
    }

    [HttpPost]
    public async Task<JsonResult> CreateTeacherExcel(IFormFile excelFile, int IdInstitu)
    {
        var origin = Request.Headers["origin"];

        var response = await _excelManager.TeacherImport(excelFile, origin, IdInstitu);
        await SetInitData(InitDataEnum.Teachers);

        return Json(new { data = response.messages.FirstOrDefault(), result = response.result });

    }

    #endregion

    #region Manejo Student By Excel

    [HttpGet]
    public IActionResult DownloadTemplateExcelStudent()
    {
        var filePath = Path.Combine(_wwwrootPath, "Templates", "PlantillaAlumnoRepresentantes.xlsx");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("La plantilla de Excel no se encontró.");
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PlantillaAlumnoRepresentantes.xlsx");
    }

    [HttpGet]
    public async Task<IActionResult> FillStudentsAndFather()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> CreateStudentExcel(IFormFile excelFile, int IdInstitu)
    {
        var userName = User.Identity.Name;
        var currentUser = await _userService.GetUserByName(userName);
        if (IdInstitu == 0)
        {
            IdInstitu = currentUser.InstitutionId ?? 0;
        }

        var origin = Request.Headers["origin"];

        var response = await _excelManager.StudentAndParentsImport(excelFile);
        await SetInitData(InitDataEnum.StudentsAndFather);

        return Json(new { data = response.messages.FirstOrDefault(), result = response.result });

    }

    #endregion    
    
    #region Manejo Room By Excel

    [HttpGet]
    public IActionResult DownloadTemplateExcelRoom()
    {
        var filePath = Path.Combine(_wwwrootPath, "Templates", "Plantilla_Salones.xlsx");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("La plantilla de Excel no se encontró.");
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Plantilla_Salones.xlsx");
    }

    [HttpGet]
    public IActionResult FillRoom()
    {
        // Código para mostrar la vista de llenar datos de "Room"
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> CreateRoomExcel(IFormFile excelFile)
    {
        
        var response = await _excelManager.RoomImport(excelFile);
        await SetInitData(InitDataEnum.Room);

        return Json(new { data = response.messages.FirstOrDefault(), result = response.result });

    }

    #endregion    


    #region Private Metods

    private async Task<bool> SetInitData(InitDataEnum  initDataEnum)
    {
        var getInitData = await _initDataService.GetAllViewModel();
        var filterInitData = getInitData.FirstOrDefault(x => x.Description == initDataEnum.ToString());

        var targetResult = _mapper.Map<InitDataSaveViewModel>(filterInitData);

        if (targetResult != null)
        {
            targetResult.Ready = true;
           await _initDataService.Update(targetResult, targetResult.Id);
        }
        return true;
    }
    
    #endregion


}

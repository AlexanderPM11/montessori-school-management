using SmartCampus.Extentions;
using MontessoriSystem.Infrastructure.Identity;
using MontessoriSystem.Core.Application;
using MontessoriSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Identity;
using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Infrastructure.Identity.Contexts;
using MontessoriSystem.Infrastructure.Identity.Entities;
using MontessoriSystem.Infrastructure.Identity.Seeds;
using MontessoriSystem.Infrastructure.Persistence.Contexts;
using MontessoriSystem.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddPersistenceInfrastructureSegurity(builder.Configuration);
builder.Services.AddCoreApplication(builder.Configuration);

builder.Services.AddControllersWithViews();
builder.Services.AddApiVersioningExtension();
builder.Services.AddSuaggerConfiguration();

builder.Services.AddIdentityInfrastructureWebApi(builder.Configuration);

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddHttpContextAccessor();


// Configuración de CORS: Permitir cualquier origen

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

// Habilita el registro de CORS
app.UseCors("CorsPolicy");
app.UseSwaggerDocumentation();
app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.MapControllers();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    RotativaConfiguration.Setup(app.Environment.ContentRootPath, "Rotativa");
}
else
{
    RotativaConfiguration.Setup("/usr/local/bin", string.Empty);
}


#region Seeds
try
{
    var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    #region Appling Automatic Migration

    var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

    // Asegúrate de que las migraciones se apliquen en el inicio de la aplicación
    await applicationContext.Database.MigrateAsync();
    await identityContext.Database.MigrateAsync();

    #endregion

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    var initDataService = scope.ServiceProvider.GetRequiredService<IInitDataService>();
    var gradeService = scope.ServiceProvider.GetRequiredService<IGradeService>();
    var indicatorsService = scope.ServiceProvider.GetRequiredService<IAchievementIndicatorsService>();
    var sujectService = scope.ServiceProvider.GetRequiredService<ISujectService>();
    var civilStatusService = scope.ServiceProvider.GetRequiredService<ICivilStatusService>();
    var provinceService = scope.ServiceProvider.GetRequiredService<IProvinceDomService>();
    var nationalityService = scope.ServiceProvider.GetRequiredService<INacionalityService>();
    var specializationService = scope.ServiceProvider.GetRequiredService<ISpecializationService>();
    var titlesService = scope.ServiceProvider.GetRequiredService<ITitlesAchievedsService>();
    var educationalLevelService = scope.ServiceProvider.GetRequiredService<IEducationalLevelService>();
    var relationshipService = scope.ServiceProvider.GetRequiredService<IRelationshipPersonService>();
    var professionsService = scope.ServiceProvider.GetRequiredService<IProfessionsService>();
    var typeRegisterService = scope.ServiceProvider.GetRequiredService<ITypeRegisterService>();
    var tipoAdjuntoService = scope.ServiceProvider.GetRequiredService<ITipoAdjuntoService>();

    var existInitDaata = await initDataService.GetAllViewModel();
    var existGrades = await gradeService.GetAllViewModel();
    var achievementIndicators = await indicatorsService.GetAllViewModel();
    var sujects = await sujectService.GetAllViewModel();
    var civilStatus = await civilStatusService.GetAllViewModel();
    var existProvinces = await provinceService.GetAllViewModel();
    var existNationalities = await nationalityService.GetAllViewModel();
    var existSpecializations = await specializationService.GetAllViewModel();
    var existTitles = await titlesService.GetAllViewModel();
    var existEducationalLevels = await educationalLevelService.GetAllViewModel();
    var existRelationships = await relationshipService.GetAllViewModel();
    var existProfessions = await professionsService.GetAllViewModel();
    var existTypeRegisters = await typeRegisterService.GetAllViewModel();
    var existTipoAdjuntos = await tipoAdjuntoService.GetAllViewModel();

    if (existInitDaata.Count == 0)
    {
        await InitDataSeed.SetDataInit(initDataService);
    }
    if (existGrades.Count == 0)
    {
        await GradeSeed.SetGrade(gradeService);
    }
    if (achievementIndicators.Count == 0)
    {
        await AchievementIndicatorsSeed.SetAchievementIndicators(indicatorsService);
    }
    if (sujects.Count == 0)
    {
        await AchievementIndicatorsSeed.SetMultipleSubjects(sujectService);
    }
    if (civilStatus.Count == 0)
    {
        await CivilStatusSeed.SetCivilStatus(civilStatusService);
    }
    if (existProvinces.Count == 0) await MasterDataSeed.SeedProvinces(provinceService);
    if (existNationalities.Count == 0) await MasterDataSeed.SeedNationalities(nationalityService);
    if (existSpecializations.Count == 0) await MasterDataSeed.SeedSpecializations(specializationService);
    if (existTitles.Count == 0) await MasterDataSeed.SeedTitlesAchieved(titlesService);
    if (existEducationalLevels.Count == 0) await MasterDataSeed.SeedEducationalLevels(educationalLevelService);
    if (existRelationships.Count == 0) await MasterDataSeed.SeedRelationshipPersons(relationshipService);
    if (existProfessions.Count == 0) await MasterDataSeed.SeedProfessions(professionsService);
    if (existTypeRegisters.Count == 0) await MasterDataSeed.SeedTypeRegister(typeRegisterService);
    if (existTipoAdjuntos.Count == 0) await MasterDataSeed.SeedTipoAdjunto(tipoAdjuntoService);

    await DefaultRoles.SeedAsync(roleManager);
    await DefaultBasicUser.SeedAsync(userManager);
    await AdminUser.SeedAsync(userManager);
    await SuperAdminUser.SeedAsync(userManager);
    await SuperAdminUser2.SeedAsync(userManager);
    await SuperAdminUser3.SeedAsync(userManager);
    await TeacherDefault.SeedAsync(userManager);
    await SuperAdminFrontend.SeedAsync(userManager);

}
catch (Exception ex)
{
    Console.WriteLine($"Error al aplicar las migraciones: {ex.Message}");
    throw ex;
}
#endregion




app.Run();

using Microsoft.AspNetCore.Mvc;

namespace SmartCampus.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Touride.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetUser()
        {
            var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.Equals("firstName", StringComparison.OrdinalIgnoreCase))?.Value;

            return Ok(userName);
        }
    }
}
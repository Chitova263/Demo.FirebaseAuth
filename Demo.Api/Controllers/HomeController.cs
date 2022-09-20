using Demo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public HomeController(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }
        
        [HttpGet]
        public IActionResult GetResource()
        {
            var user = _identityService.GetApplicationUser();
            return Ok(user);
        }
    
    }
}

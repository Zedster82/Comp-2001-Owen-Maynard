using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {

        // GET: api/Logout
        [HttpGet]
        public string Get()
        {
            return "Succefully logged out";
        }

        
    }
}

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
            //Clear all data from logged in user
            Login.isLoggedIn = false;
            Login.username = string.Empty;
            Login.password = string.Empty;
            Login.email = string.Empty;
            Login.accountType = string.Empty;
            Login.accountID = null;
            return "Successfully logged out";
        }

        
    }
}

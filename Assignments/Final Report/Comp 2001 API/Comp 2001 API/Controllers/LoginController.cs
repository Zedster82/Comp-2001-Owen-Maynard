using Microsoft.AspNetCore.Mvc;



namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // GET api/Login/5
        [HttpGet("{email},{password}")]
        public string Get(string email,string password)
        {
            return $"{email} {password}";




        }

    }
}

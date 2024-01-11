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



        // GET api/Login/username,password
        [HttpGet("{email},{password}")]
        public Task<string> Get(string email,string password)
        {

            return Login.LoginAuth(email, password);

        }

    }
}

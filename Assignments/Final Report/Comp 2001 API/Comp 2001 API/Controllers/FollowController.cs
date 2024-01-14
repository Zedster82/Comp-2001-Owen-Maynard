using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public FollowController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // POST api/FollowController/5
        [HttpPost]
        public void Post(int followingID)
        {
            
        }

        // DELETE api/<FollowController>/5
        [HttpDelete("Unfollow{id}")]
        public void Delete(int id)
        {
            
        }
    }
}

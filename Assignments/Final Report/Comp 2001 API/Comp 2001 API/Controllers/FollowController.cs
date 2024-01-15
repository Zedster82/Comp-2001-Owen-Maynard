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
        [HttpPost("Follow{id}")]
        public ContentResult Post(int id)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return Content("You are not logged in");
            }


            //Check if login is expired
            if (Login.loginExpired())
            {
                return Content("Login Expired");
            }



            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Follow_User] @user_id , @following_id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user_id", Login.accountID);
                    command.Parameters.AddWithValue("@following_id", id);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            return Content($"User {Login.accountID} has successfully followed user {id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message, "application/json");
                    }
                }
            }
        }

        // DELETE api/<FollowController>/5
        [HttpDelete("Unfollow{id}")]
        public ContentResult Delete(int id)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return Content("You are not logged in");
            }


            //Check if login is expired
            if (Login.loginExpired())
            {
                return Content("Login Expired");
            }



            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Unfollow_User] @user_id , @following_id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user_id", Login.accountID);
                    command.Parameters.AddWithValue("@following_id", id);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            return Content($"User {Login.accountID} has successfully un-followed user {id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message, "application/json");
                    }
                }
            }
        }
    }
}

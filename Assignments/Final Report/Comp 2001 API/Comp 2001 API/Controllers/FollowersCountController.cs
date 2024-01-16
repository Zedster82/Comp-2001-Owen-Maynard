using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersCountController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public FollowersCountController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /*[HttpGet("Logged in")]
        public ContentResult Get()
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


            //Get number of followers from currently logged in user
            string connectionString = Configuration.GetConnectionString("Default");

            int id;

            try
            {
                id = (int)Login.accountID;
            }
            catch (Exception ex)
            {
                return Content("You do not have a user id");
            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"EXEC CW2.[Followers_Count] {id}";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Read all data, using a data table to convert it
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);


                            string count = dataTable.Rows[0]["Column1"].ToString();
                            return Content(count);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                }
            }
        }*/


        // GET api/<FollowCountController>/5
        [HttpGet("{id}")]
        public ContentResult Get(int id)
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


            //Get a list of all users
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"EXEC CW2.[Followers_Count] {id}";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Read all data, using a data table to convert it
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);


                            string count = dataTable.Rows[0]["Column1"].ToString();
                            return Content(count);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                }
            }
        }

        
    }
}

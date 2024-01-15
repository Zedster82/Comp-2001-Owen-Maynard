using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteActivitiesController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public FavouriteActivitiesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }




        // GET: api/<FavouriteActivitiesController>
        [HttpGet]
        public ContentResult Get()
        {
            //Get a list of a single user favourite activities
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

            //Check if the user is an admin
            if (Login.accountType != "admin")
            {
                return Content("You do not have permission to perfom this action");
            }

            //Get a list of all users favourite activities
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Favourite_Activity_List_All]";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Read all data, using a data table to convert it
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);

                            string jsonConverted = JsonConvert.SerializeObject(dataTable);
                            return Content(jsonConverted, "application/json");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message, "application/json");
                    }
                }
            }
        }

        // GET api/<FavouriteActivitiesController>/5
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

            //Get a list of a specific users favourite activities
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Favourite_Activity_List_ID] @id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Read all data, using a data table to convert it
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);

                            string jsonConverted = JsonConvert.SerializeObject(dataTable);
                            return Content(jsonConverted, "application/json");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message, "application/json");
                    }
                }
            }
        }

        // POST api/<FavouriteActivitiesController>
        [HttpPost("Favourite_Activity")]
        public ContentResult Post(int activity_id)
        {
            //Favourite an activity
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


            //Get a list of a specific users favourite activities
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Favourite_Activity] @user_id, @activity_id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user_id", Login.accountID);
                    command.Parameters.AddWithValue("@activity_id", activity_id);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Read all data, using a data table to convert it
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);

                            string jsonConverted = JsonConvert.SerializeObject(dataTable);
                            return Content(jsonConverted, "application/json");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message, "application/json");
                    }
                }
            }
        }

        

        // DELETE api/<FavouriteActivitiesController>/5
        [HttpDelete("Un_Favourite_Activity{id}")]
        public ContentResult Delete(int activity_id)
        {
            //Unfavourite an activity

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


            //Get a list of a specific users favourite activities
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Un_Favourite_Activity] @user_id, @activity_id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user_id", Login.accountID);
                    command.Parameters.AddWithValue("@activity_id", activity_id);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //Read all data, using a data table to convert it
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);

                            string jsonConverted = JsonConvert.SerializeObject(dataTable);
                            return Content(jsonConverted, "application/json");
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

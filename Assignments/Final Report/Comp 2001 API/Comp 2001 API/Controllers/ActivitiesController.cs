using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public ActivitiesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }






        // GET: api/<ActivitiesController>
        [HttpGet]
        public ContentResult Get()
        {
            //Get all Activities
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

                string sql = "SELECT * FROM CW2.[Activity]";


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

        // GET api/<ActivitiesController>/5
        [HttpGet("{id}")]
        public ContentResult Get(int id)
        {
            //Get Activity at value
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

                string sql = $"SELECT * FROM CW2.[Activity] WHERE activity_id = @id";


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

        // POST api/<ActivitiesController>
        [HttpPost("CreateActivity")]
        public ContentResult Post(string activityName)
        {
            
            //Create activity
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

            //Check if user is an admin
            if (Login.accountType != "admin")
            {
                return Content("You do not have permission to perfom this action");
            }

            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Add_Activity] @activityName";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@activityName", activityName);
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

        // PUT api/<ActivitiesController>/5
        [HttpPut("Edit_Activity/{id},{newActivityName}")]
        public ContentResult Put(int id, string newActivityName)
        {
            //Edit Activity
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

            //Check if user is an admin
            if (Login.accountType != "admin")
            {
                return Content("You do not have permission to perfom this action");
            }

            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Activity_Edit] @id, @activityName";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@activityName", newActivityName);
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

        // DELETE api/<ActivitiesController>/5
        [HttpDelete("Delete_Activity/{id}")]
        public ContentResult Delete(int id)
        {
            //Delete Activity
            
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

            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Delete_Activity] @id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message, "application/json");
                    }
                }
            }
            return Content("Succesful deletion");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveUsersController : ControllerBase
    {


        public IConfiguration Configuration { get; }

        public ArchiveUsersController(IConfiguration configuration)
        {
            Configuration = configuration;
        }





        // GET: api/<ArchiveUsersController>
        [HttpGet]
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

            //Get a list of all archive users
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM CW2.[Archive_User]";


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

        // GET api/<ArchiveUsersController>/5
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


            //Get a singular users data
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"SELECT * FROM CW2.[Archive_User] WHERE user_id='{id}'";


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
                            connection.Close();
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

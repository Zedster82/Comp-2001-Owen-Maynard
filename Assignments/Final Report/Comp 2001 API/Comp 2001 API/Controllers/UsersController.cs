using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public UsersController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string sqlRequest()
        {
            
        }


        // GET: api/Users
        [HttpGet]
        public string Get()
        {

            //Get a list of all users
            

            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM CW1.[User]";


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
                            return jsonConverted;
                        }
                    }
                    catch(Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //Get a singular users data
            return "value";
        }

        // POST api/Users
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //Create a new user
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //Edit a current users data
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //Delete a user
        }
    }
}

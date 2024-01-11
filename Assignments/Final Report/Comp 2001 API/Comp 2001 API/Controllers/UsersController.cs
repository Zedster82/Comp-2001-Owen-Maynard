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

        


        // GET: api/Users
        [HttpGet]
        public string Get()
        {

            //Get a list of all users
            

            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM CW2.[User]";


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

            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"SELECT * FROM CW2.[User] WHERE user_id='{id}'";


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
                            return jsonConverted;
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
        }

        // POST api/Users/username,email,password
        [HttpPost("{username},{email},{password}")]
        public string Post(string username, string email, string password)
        {
            //Create a new user
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"EXEC CW2.[Add_User] '{username}', '{email}', '{password}', 'user'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            
                            connection.Close();
                            return $"user {username} with email {email} added";
                        }
                    }
                    catch (Exception ex)
                    {
                        return "User creation failed, try again";
                    }
                }
            }
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //Edit a current users data
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //Delete a user

            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"EXEC CW2.[Archive_User_Procedure] {id}";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            return $"user {id} deleted";
                        }
                    }
                    catch(Exception ex)
                    {
                        return ex.Message;
                    }
                    
                }
            }
        }
    }
}

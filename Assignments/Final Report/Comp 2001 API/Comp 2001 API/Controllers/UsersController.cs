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
        public ContentResult Get()
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return Content("You are not logged in");
            }


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
                            return Content(jsonConverted, "application/json");
                        }
                    }
                    catch(Exception ex)
                    {
                        return Content(ex.Message, "application/json");
                    }
                }
            }


        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public ContentResult Get(int id)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return Content("You are not logged in");
            }
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

        // POST api/Users/username,email,password
        [HttpPost("{username},{email},{password}")]
        public string Post(string username, string email, string password)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return "You are not logged in";
            }

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
        [HttpPut("{id},{username},{email},{password},{isAdmin}")]
        public string Put(int id, string username, string email, string password, bool isAdmin)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return "You are not logged in";
            }



            //Edit a current users data

            string usertype = "user";
            //Check to make sure that the user is being edited to admin and that the currently logged in user is an admin.
            if (isAdmin && (Login.accountType == "admin"))
            {
                usertype = "admin";
            }


            string[] sqlArray = 
           {$"EXEC CW2.[Edit_Username] {id}, \"{username}\"",
            $"EXEC CW2.[Edit_Email] {id}, \"{email}\"",
            $"EXEC CW2.[Edit_Password] {id}, \"{password}\"",
            $"EXEC CW2.[Edit_Account_Type] {id}, \"{usertype}\""};


            string connectionString = Configuration.GetConnectionString("Default");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (string sql in sqlArray)
                {
                    


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {


                                
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }
                    }
                }
                connection.Close();
                
            }
            return "User succesfully edited";

        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn && Login.accountType == "admin")
            {
                return "You are not logged in, or you do not have permissions.";
            }
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

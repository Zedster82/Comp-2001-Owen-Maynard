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

                string sql = "SELECT * FROM CW2.[Main_View]";


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

                string sql = $"SELECT * FROM CW2.[Main_View] WHERE [User ID]='{id}'";


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
        [HttpPost("CreateUser/{username},{email},{password}")]
        public ContentResult Post(string username, string email, string password)
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




            //Hash the password

            string[] hashresult = Login.hashPassword(password);

            
            string connectionString = Configuration.GetConnectionString("Default");

            //Create a new user
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "EXEC CW2.[Add_User] @Username, @Email, @Hashed_password, @Salt, 'user'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Hashed_password", hashresult[0]);
                    command.Parameters.AddWithValue("@Salt", hashresult[1]);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            
                            connection.Close();
                            return Content($"user {username} with email {email} added");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content("User creation failed, try again");
                    }
                }
            }
        }

        // PUT api/Users/5
        [HttpPut("EditUser/{id},{newUsername},{newEmail},{newPassword},{newAccountType}")]
        public string Put(int id, string newUsername, string newEmail, string newPassword, bool newAccountType)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return "You are not logged in";
            }


            //Check if login is expired
            if (Login.loginExpired())
            {
                return "Login Expired";
            }


            //Edit a current users data

            string accountTypeString = "user";
            //Check to make sure that the user is being edited to admin and that the currently logged in user is an admin.
            if (newAccountType && (Login.accountType == "admin"))
            {
                accountTypeString = "admin";
            }
            else
            {
                return "You do not have permission to do this";
            }



            //Generate the new hashed password
            string[] hashresult = Login.hashPassword(newPassword);




            string[] sqlArray = 
           {$"EXEC CW2.[Edit_Username] {id}, \"{newUsername}\"",
            $"EXEC CW2.[Edit_Email] {id}, \"{newEmail}\"",
            $"EXEC CW2.[Edit_Password] {id}, \"{hashresult[0]}\" , '{hashresult[1]}'",
            $"EXEC CW2.[Edit_Account_Type] {id}, \"{accountTypeString}\""};


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
        [HttpDelete("DeleteUser/{id}")]
        public string Delete(int id)
        {
            //Check if a user is logged in
            if (!Login.isLoggedIn)
            {
                return "You are not logged in";
            }

            //Check if login is expired
            if (Login.loginExpired())
            {
                return "Login Expired";
            }

            //Check if the user is an admin
            if (Login.accountType != "admin")
            {
                return "You do not have permission to perfom this action";
            }


            //Delete a user
            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"EXEC CW2.[Archive_User_Procedure] @id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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

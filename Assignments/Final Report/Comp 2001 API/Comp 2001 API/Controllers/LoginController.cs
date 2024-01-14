using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;



namespace Comp_2001_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        public IConfiguration Configuration { get; }

        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // GET api/Login/username,password
        [HttpGet("{email},{password}")]
        public string Get(string email,string password)
        {

            Login.LoginAuth(email, password);

            if (!Login.isLoggedIn)
            {
                return "Incorrect Login";
            }



            string connectionString = Configuration.GetConnectionString("Default");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"SELECT * FROM CW2.[User] WHERE email=@Email";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            //Read all data, using a data table to convert it
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);

                            //string jsonConverted = JsonConvert.SerializeObject(dataTable);
                            
                            Login.accountID = Convert.ToInt32(dataTable.Rows[0]["user_id"].ToString());
                            Login.username = dataTable.Rows[0]["username"].ToString();
                            Login.email = dataTable.Rows[0]["email"].ToString();
                            Login.password = dataTable.Rows[0]["password"].ToString();
                            Login.accountType = dataTable.Rows[0]["account_type"].ToString();


                            

                            return $"Successfully logged in as user: {Login.username} with email of: {Login.email}";
                            
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }





            return "failed";

        }

    }
}

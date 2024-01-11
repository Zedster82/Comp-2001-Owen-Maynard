using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Comp_2001_API
{
    public class Login
    {



        


        public string? email;

        public string? password;

        public bool isLoggedIn;

        public string? accountType;

        public int accountID;


        public static  async Task<string> LoginAuth(string email, string password)
        {


            //string jsonData = @"""{@""email@"": @""" + email + @""",@""password@"": @""" + password + @"""}";
            string jsonData = "{\"email\": \"" + email + "\",\"password\": \"" + password + "\"}";

            //string jsonData = "{ \"email\": \"grace@plymouth.ac.uk\", \"password\": \"ISAD123!\" }";

            string url = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/users";

            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using var response = client.Send(request);

            //response.EnsureSuccessStatusCode();

            var responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseText);

            

            return responseText;
        }

        

        
    }
}

using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace Comp_2001_API
{
    public class Login
    {




        public static string? username;

        public static string? email;

        public static bool isLoggedIn = false;

        public static string? accountType;

        public static int? accountID;

        public static DateTime sessionExpireTime;

        public static bool loginExpired()
        {
            if(DateTime.Compare(sessionExpireTime, DateTime.Now) <= 0)
            {
                return true;
            }
            else
            {
                sessionExpireTime = DateTime.Now.AddMinutes(10);
                return false;
            }
        } 
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


            if (responseText.Contains("True"))
            {
                isLoggedIn = true;
                sessionExpireTime = DateTime.Now.AddMinutes(10);
            }


            return responseText;
        }
        
        public static string[] hashPassword (string password, string salt = "")
        {
            
            if (salt == "")
            {
                salt = CreateSalt();
            }
            return [GenerateSHA256Hash(GenerateSHA256Hash(password, salt),salt), salt];
        }

        const int SALT_SIZE = 10;
        private static string CreateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[SALT_SIZE];
            rng.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }


        private static string GenerateSHA256Hash(string input, string salt = "")
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            if (salt == ""){
                bytes = Encoding.UTF8.GetBytes(input);
            }
            
            
            var hashManager = new SHA256Managed();
            byte[] hash = hashManager.ComputeHash(bytes);

            return ByteArrayToHexString(hash);
        }


        private static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
                sb.AppendFormat("{0:x2}", b);

            return sb.ToString();
        }
    }
}

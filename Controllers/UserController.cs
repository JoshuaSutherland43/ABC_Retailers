using CLDV_POE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace CLDV_POE.Controllers
{
    public class UserController : Controller
    {
        // Declairing variables

        //HttpClient will allow us to use triggers
        private readonly HttpClient _httpClient;

        //String to connect to the URL
        private static string connectionString = "Server=tcp:st10255930.database.windows.net,1433;Initial Catalog=ST10255930_CLDV_POE_P1;Persist Security Info=False;User ID=joshua;Password=Lesley*1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //String that connects to Function
        private string FunctionUrl = "https://functionst10255930.azurewebsites.net/api/InsertUserTest?";

        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Displaying Users
        public IActionResult Index()
        {
            List<User> users = new List<User>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "SELECT * FROM Users";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            User_Id = Convert.ToInt32(reader["User_Id"]),
                            User_Name = reader["User_Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString()
                        });
                    }
                    
                }
                con.Close();
            }

            return View(users);
        }

        // Displaying Add User View
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }


        //Adding user method
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            // Create form data content
            var formContent = new FormUrlEncodedContent(new[]
            {
                  //Lock, A. (2023)
                  new KeyValuePair<string, string>("User_Name", user.User_Name),
                  new KeyValuePair<string, string>("Email", user.Email),
                  new KeyValuePair<string, string>("Password", user.Password)
            });

            // Send the form data to the Azure Function
            //(IEvangelist. 2023)
            var response = await _httpClient.PostAsync(FunctionUrl, formContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to add user.");
            return View(user);
        }

        //Lock, A. (2023)
        //Reading JSON and binary data from multipart/form-data sections in ASP.NET CORE, Andrew Lock | .NET Escapades.
        //Available at: https://andrewlock.net/reading-json-and-binary-data-from-multipart-form-data-sections-in-aspnetcore/
        //(Accessed: 12 November 2024).

        //IEvangelist (2023)
        //Make HTTP requests with the httpclient - .NET, Make HTTP requests with the HttpClient - .NET | Microsoft Learn.
        //Available at: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
        //(Accessed: 12 November 2024). 


        //Delete Method
        [HttpPost]
        public IActionResult Delete(int userId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "DELETE FROM Users WHERE User_Id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return RedirectToAction("Index");
        }
    }
}

//=====================================================
// Referencing
//=====================================================

//Rudman, G. (2024)
//BCA2 CLDV Part 2 Workshop, YouTube.
//Available at: https://www.youtube.com/watch?v=I_tiFJ-nlfE&list=LL&index=1&t=13s
//(Accessed: 18 October 2024). 
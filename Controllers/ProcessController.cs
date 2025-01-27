
using CLDV_POE.Models;
using CLDV_POE.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CLDV_POE.Controllers
{
    public class ProcessController : Controller
    {
        //Obtaining Azure SQL connection string
        private static string connectionString = "Server=tcp:st10255930.database.windows.net,1433;Initial Catalog=ST10255930_CLDV_POE_P1;Persist Security Info=False;User ID=joshua;Password=Lesley*1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //Displaying process list
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Process> processes = new List<Process>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                string sql = "SELECT * FROM Process";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        processes.Add(new Process
                        {
                            Process_Id = Convert.ToInt32(reader["Process_Id"]),
                            User_ID = Convert.ToInt32(reader["User_ID"]),
                            Product_ID = Convert.ToInt32(reader["Product_ID"]),
                            Process_Date = Convert.ToDateTime(reader["Process_Date"]),
                            Process_Location = reader["Process_Location"].ToString(),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
                await con.CloseAsync();
            }
            return View(processes);
        }

        //Get method for register View
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var users = await GetAllUsersAsync();
            var products = await GetAllProductsAsync();

            if (users.Count == 0)
            {
                ModelState.AddModelError("", "No Users found. Please add a user first.");
                return View();
            }
            if (products.Count == 0)
            {
                ModelState.AddModelError("", "No Products found. Please add a product first.");
                return View();
            }

            ViewData["User"] = users;
            ViewData["Product"] = products;
            return View();
        }

        //Post method to Register Process
        [HttpPost]
        public IActionResult Register(Process process)
        {
            if (ModelState.IsValid)
            {
                process.Status = "Pending";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "INSERT INTO Process (User_ID, Product_ID, Process_Date, Process_Location, Status) VALUES (@User_ID, @Product_ID, @Process_Date, @Process_Location, @Status)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@User_ID", process.User_ID);
                        cmd.Parameters.AddWithValue("@Product_ID", process.Product_ID);
                        cmd.Parameters.AddWithValue("@Process_Date", process.Process_Date);
                        cmd.Parameters.AddWithValue("@Process_Location", process.Process_Location);
                        cmd.Parameters.AddWithValue("@Status", process.Status);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", $"Failed to register process. Error: {ex.Message}");
                            ViewData["User"] = GetAllUsersAsync().Result;
                            ViewData["Product"] = GetAllProductsAsync().Result;
                            return View(process);
                        }
                    }
                    con.Close();
                }
                return RedirectToAction("Index");
            }

            ViewData["User"] = GetAllUsersAsync().Result;
            ViewData["Product"] = GetAllProductsAsync().Result;
            return View(process);
        }

        //Delete a process method
        [HttpPost]
        public IActionResult DeleteProcess(int processId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "DELETE FROM Process WHERE Process_Id = @ProcessId";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ProcessId", processId);
                    cmd.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Process deleted successfully.";
            return RedirectToAction("Index");
        }

        private async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                string sql = "SELECT * FROM Users";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
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
                await con.CloseAsync();
            }
            return users;
        }

        private async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                string sql = "SELECT * FROM Product";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Product_Id = Convert.ToInt32(reader["Product_Id"]),
                            Product_Name = reader["Product_Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            Price = reader["Price"].ToString()
                        });
                    }
                }
                await con.CloseAsync();
            }
            return products;
        }

        [HttpPost]
        public IActionResult UpdateProcess(int processId, string status)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "UPDATE Process SET Status = @updatedStatus WHERE Process_Id = @process_Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@updatedStatus", status);
                    cmd.Parameters.AddWithValue("@process_Id", processId);
                    cmd.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Process status updated successfully.";
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
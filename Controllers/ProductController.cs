using Azure.Storage.Blobs;
using CLDV_POE.Models;
using CLDV_POE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CLDV_POE.Controllers
{
    public class ProductController : Controller
    {
        // Declairing variables 

        private readonly BlobService _blobService;

        //HttpClient will allow us to use triggers
        private readonly HttpClient _httpClient;

        //String that connects to Function
        private string FunctionUrl = "https://functionst10255930.azurewebsites.net/api/InsertProductTest?";

        //String to connect to the URL
        private static string connectionString = "Server=tcp:st10255930.database.windows.net,1433;Initial Catalog=ST10255930_CLDV_POE_P1;Persist Security Info=False;User ID=joshua;Password=Lesley*1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public ProductController(BlobService blobService, HttpClient httpClient)
        {
            _blobService = blobService;
            _httpClient = httpClient;
        }



        // Display Add Product View
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        // Displaying all products
        [HttpGet]
        public IActionResult Index()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "SELECT * FROM Product";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
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
                con.Close();
            }

            if (products.Count == 0)
            {
                ViewBag.Message = "No products available to display.";
            }

            return View(products);
        }

        // POST: Adds a new product to the database
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile imageFile)
        {
            // Create Multipart form content, We do not use a URL encoded data as we are aslo sending an image over, not just text.
            //(Lock. 2023)
            using (var content = new MultipartFormDataContent())
            {
                // Adding product data to the new form:
                //(Lock. 2023)
                content.Add(new StringContent(product.Product_Name), "Product_Name");
                content.Add(new StringContent(product.Description ?? ""), "Description");
                content.Add(new StringContent(product.Price.ToString()), "Price");

                // Add the image file if it exists
                if (imageFile != null && imageFile.Length > 0)
                {
                    //(Yiyi.2020)
                    var imageContent = new StreamContent(imageFile.OpenReadStream());
                    //(Yiyi.2020)
                    imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);

                    //Adding the image if it exists to the form
                    //(Lock. 2023)
                    content.Add(imageContent, "File", imageFile.FileName);
                }

                // Send request to Azure Function
                //(IEvangelist. 2023)
                var response = await _httpClient.PostAsync(FunctionUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to add product.");
            }

            return View(product);
        }

        //Lock, A. (2023)
        //Reading JSON and binary data from multipart/form-data sections in ASP.NET CORE, Andrew Lock | .NET Escapades.
        //Available at: https://andrewlock.net/reading-json-and-binary-data-from-multipart-form-data-sections-in-aspnetcore/
        //(Accessed: 12 November 2024). 

        //Yiyi You(2020)
        //Post form-data with image file using C#, Stack Overflow.
        //Available at: https://stackoverflow.com/questions/63394137/post-form-data-with-image-file-using-c-sharp
        //(Accessed: 12 November 2024). 

        //IEvangelist (2023)
        //Make HTTP requests with the httpclient - .NET, Make HTTP requests with the HttpClient - .NET | Microsoft Learn.
        //Available at: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
        //(Accessed: 12 November 2024). 



        // Delete a product method
        [HttpPost]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        // Retrieve the product to get the image URL
            string imageUrl = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                string sql = "SELECT ImageUrl FROM Product WHERE Product_Id = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", productId);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    imageUrl = reader["ImageUrl"].ToString();
                }
                await con.CloseAsync();
            }

            // Delete the image from Blob Storage if the URL is valid
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    var imageUri = new Uri(imageUrl, UriKind.Absolute);
                    await _blobService.DeleteBlobAsync(imageUri.AbsolutePath.TrimStart('/'));
                }
                catch (UriFormatException)
                {
                    ModelState.AddModelError("", "The image URL format is invalid.");
                }
            }

            // Delete product from database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                string sql = "DELETE FROM Product WHERE Product_Id = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", productId);
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
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
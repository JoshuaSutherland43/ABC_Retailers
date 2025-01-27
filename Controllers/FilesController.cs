using CLDV_POE.Models;
using CLDV_POE.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CLDV_POE.Controllers
{
    public class FilesController : Controller
    {
        private readonly AzureFileShareService _fileShareService;
        private string FunctionURL = "https://functionst10255930.azurewebsites.net/api/FileShare?code=G99E9nJ11UMzmX8ol8od9VjKNMiqzJuPYenfjRe2o65wAzFuIs4ZIg%3D%3D";
        public FilesController(AzureFileShareService fileShareService)
        {
            _fileShareService = fileShareService;
        }

        [HttpGet]
        public async Task<IActionResult> Log()
        {
            List<FileModel> files;
            try
            {
                files = await _fileShareService.ListFilesAsync("uploads");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Failed to load files: {ex.Message}";
                files = new List<FileModel>();
            }
            return View(files);
        }

        public async Task<IActionResult> Index()
        {
            List<FileModel> files;
            try
            {
                files = await _fileShareService.ListFilesAsync("uploads");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Failed to load files: {ex.Message}";
                files = new List<FileModel>();
            }
            return View(files);
        }

        //Upload file Method
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to upload.");
                return View();
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var formContent = new MultipartFormDataContent())
                    {
                        // Add file content
                        //(Yiyi.2020)
                        var fileContent = new StreamContent(file.OpenReadStream());

                        //(Yiyi.2020)
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        formContent.Add(fileContent, "file", file.FileName);

                        // Add additional form data
                        //(Lock. 2023)
                        formContent.Add(new StringContent("fileshare"), "shareName");
                        formContent.Add(new StringContent("uploads"), "directoryName");

                        // Send request to Azure Function
                        //(IEvangelist. 2023)
                        var response = await httpClient.PostAsync(FunctionURL, formContent);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Message"] = $"File '{file.FileName}' uploaded successfully.";
                        }
                        else
                        {
                            TempData["Message"] = "File upload failed.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"File upload failed: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
        //Yiyi You(2020)
        //Post form-data with image file using C#, Stack Overflow.
        //Available at: https://stackoverflow.com/questions/63394137/post-form-data-with-image-file-using-c-sharp
        //(Accessed: 12 November 2024). 

        //Lock, A. (2023)
        //Reading JSON and binary data from multipart/form-data sections in ASP.NET CORE, Andrew Lock | .NET Escapades.
        //Available at: https://andrewlock.net/reading-json-and-binary-data-from-multipart-form-data-sections-in-aspnetcore/
        //(Accessed: 12 November 2024).

        //IEvangelist (2023)
        //Make HTTP requests with the httpclient - .NET, Make HTTP requests with the HttpClient - .NET | Microsoft Learn.
        //Available at: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
        //(Accessed: 12 November 2024). 



        //Download File Method
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be null or empty");
            }

            try
            {
                var fileStream = await _fileShareService.DownloadFileAsync("uploads", fileName);
                if (fileStream == null)
                {
                    return NotFound($"File '{fileName}' not found");
                }
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception e)
            {
                return BadRequest($"Error downloading file: {e.Message}");
            }
        }

        //Delete File Method
        [HttpPost]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be null or empty");
            }

            try
            {
                // Perform deletion
                await _fileShareService.DeleteFileAsync("uploads", fileName);
                TempData["Message"] = $"File '{fileName}' deleted successfully";
            }
            catch (Exception e)
            {
                TempData["Message"] = $"Error deleting file: {e.Message}";
            }

            return RedirectToAction("Log");
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
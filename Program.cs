using Microsoft.EntityFrameworkCore;
using CLDV_POE.Models;
using CLDV_POE.Services;

namespace CLDV_POE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient();

            // Register blobservice with configuration
            builder.Services.AddSingleton(new BlobService(configuration.GetConnectionString("AzureStorage2")));

            // Register tablestorage with configuration
            builder.Services.AddSingleton(new TableStorageServices(configuration.GetConnectionString("AzureStorage")));

            builder.Services.AddSingleton<QueueService>(sp =>
            {
                var connectionString = configuration.GetConnectionString("AzureStorage");
                return new QueueService(connectionString, "processes");
            });

            // Register fileShareServices with configuration
            builder.Services.AddSingleton<AzureFileShareService>(sp =>
            {
                var connectionString = configuration.GetConnectionString("AzureStorage2");
                return new AzureFileShareService(connectionString, "fileshare");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

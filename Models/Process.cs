using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace CLDV_POE.Models
{
    public class Process : ITableEntity
    {
        [Key]
        public int Process_Id { get; set; } // Ensure property exists

        public string? Status { get; set; }

        //ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        [Required(ErrorMessage = "Please select a user.")]
        public int User_ID { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public int Product_ID { get; set; }

        [Required(ErrorMessage = "Please select a Date.")]
        public DateTime Process_Date { get; set; }

        [Required(ErrorMessage = "Please enter a location.")]
        public string? Process_Location { get; set; }


    }
}

using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace CLDV_POE.Models
{
    public class User : ITableEntity
    {
        [Key]
        public int User_Id { get; set; }
        public string? User_Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        //ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}

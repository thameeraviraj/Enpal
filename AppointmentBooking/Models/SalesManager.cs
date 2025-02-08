using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentBooking.Models
{
    [Table("sales_managers")]
    public class SalesManager
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } 
        
        [Column("languages")]
        public string[] Languages { get; set; } = Array.Empty<string>();
        
        [Column("products")]
        public string[] Products { get; set; } = Array.Empty<string>();
        
        [Column("customer_ratings")]
        public string[] CustomerRatings { get; set; } = Array.Empty<string>();
    }
}

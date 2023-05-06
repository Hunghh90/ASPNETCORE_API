using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCOREAPI.Entities
{
    [Table("products")]
    public class Product
    {

        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string name { get; set; }

        [Required]
        public double price { get; set; }

        [StringLength(150)]
        public string thumbnail { get; set; }
    }
}


using System.ComponentModel.DataAnnotations;

namespace eCommerce.Data.Models
    {
    public class Review
        {
        [Key]
        public int ReviewId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        }
    }

using SpringBootCloneApp.Models;
using SpringBootCloneApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SpringBootCloneApp.Controllers.RequestModels
{
    public class OrderRequest
    {
        [Required]
        public long? Id { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Lat { get; set; }
        [Required]
        public string? Lng { get; set; }


        [Required]
        public string? Name { get; set; } = null!;

        [Required]
        public OrderStatus? Status { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double TotalPrice { get; set; }

        [Required]
        public long? ClientId { get; set; }

        [Required]
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace SpringBootCloneApp.Models.DTOs
{
    public class OrderItemDto
    {

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public long Id { get; set; }

        public string ItemName { get; set; }


    }
}

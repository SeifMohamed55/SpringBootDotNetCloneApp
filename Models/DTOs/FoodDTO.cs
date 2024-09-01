using System.ComponentModel.DataAnnotations;

namespace SpringBootCloneApp.Models.DTOs
{
    public class FoodDTO
    {
        public FoodDTO()
        {
            
        }

        public FoodDTO(Food food)
        {
            Id = food.Id;
            Name = food.Name;
            CookTime = food.CookTime;
            Hidden = food.Hidden;
            Price = food.Price;
            FoodTags = food.FoodTags.Select(x => x.Tag).ToList();
            ImageUrl = food.ImageUrl;
            FoodOrigins = food.FoodOrigins.Select(x => x.Origin).ToList();
        }
        public long? Id { get; set; }

        [Required]
        public string CookTime { get; set; } = null!;

        public bool Hidden { get; set; } = false;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, double.MaxValue)]
        public double? Price { get; set; }

        public virtual ICollection<string> FoodTags { get; set; } = new List<string>();
        public virtual ICollection<string> FoodOrigins { get; set; } = new List<string>();

    }
}

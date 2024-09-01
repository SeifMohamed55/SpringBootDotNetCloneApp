using System.ComponentModel.DataAnnotations;

namespace SpringBootCloneApp.Controllers.RequestModels
{
    public class EmailRequest
    {
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The email address is not valid.")]
        public string? To { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}

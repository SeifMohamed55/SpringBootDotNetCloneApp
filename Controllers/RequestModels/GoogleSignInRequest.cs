using System.ComponentModel.DataAnnotations;

namespace SpringBootCloneApp.Controllers.RequestModels
{
    public class GoogleSignInRequest
    {
        [Required]
        public required string TokenId { get; set; }
    }
}

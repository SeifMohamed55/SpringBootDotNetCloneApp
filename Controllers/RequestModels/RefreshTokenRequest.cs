using System.ComponentModel.DataAnnotations;

namespace SpringBootCloneApp.Controllers.RequestModels
{
    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; } = null!;

    }
}

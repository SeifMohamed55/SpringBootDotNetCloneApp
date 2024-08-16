using System.ComponentModel.DataAnnotations;

namespace EFCorePostgres.Controllers.RequestModels
{
    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; } = null!;

    }
}

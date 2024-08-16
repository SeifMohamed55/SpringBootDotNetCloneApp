using System.ComponentModel.DataAnnotations;

namespace EFCorePostgres.Controllers.RequestModels
{
    public class GoogleSignInRequest
    {
        [Required]
        public required string TokenId { get; set; }
    }
}

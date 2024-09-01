using System.ComponentModel.DataAnnotations;

namespace SpringBootCloneApp.Controllers.RequestModels
{
    public class PasswordRequestModel
    {
        [Required]
        public long? Id { get;}

        [Required]
        public string OldPassword { get;}

        [Required] 
        public string NewPassword { get;}


    }
}

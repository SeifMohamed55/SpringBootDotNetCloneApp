﻿using System.ComponentModel.DataAnnotations;

namespace EFCorePostgres.Controllers.RequestModels
{
    public class LoginCustomRequest
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The email address is not valid.")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}

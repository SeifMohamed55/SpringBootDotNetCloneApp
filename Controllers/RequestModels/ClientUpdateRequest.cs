﻿namespace SpringBootCloneApp.Controllers.RequestModels
{
    public class ClientUpdateRequest
    {
        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

    }
}

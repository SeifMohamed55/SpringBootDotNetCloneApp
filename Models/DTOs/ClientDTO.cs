using Microsoft.AspNetCore.Identity;
using EFCorePostgres.Models;

namespace EFCorePostgres.Models.DTOs
{
    public class ClientDTO
    {
        public ClientDTO()  {  }

        public ClientDTO(Client client)
        {
            this.Id = client.Id;
            this.Address = client.Address;
            this.Banned = client.Banned;
            this.Email = client.Email;
            this.FirstName = client.FirstName;
            this.LastName = client.LastName;
        }

        public long Id { get; set; }

        public string Address { get; set; } = null!;

        public bool Banned { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public override string ToString()
        {
            return $"ClientId: {Id}, Address: {Address}, Banned: {Banned}\nEmail: {Email}, Name:{FirstName + LastName}\n";
        }
    }
}

using Microsoft.AspNetCore.Identity;
using SpringBootCloneApp.Models;

namespace SpringBootCloneApp.Models.DTOs
{
    public class ClientDTO
    {
        public ClientDTO()  {  }

        public ClientDTO(Client client)
        {
            this.Id = client.Id;
            this.Address = client.Address;
            this.Email = client.Email;
            this.FirstName = client.FirstName;
            this.LastName = client.LastName;
        }

        public long Id { get; set; }

        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;


        public override string ToString()
        {
            return $"ClientId: {Id}, Address: {Address}\nEmail: {Email}, Name:{FirstName + LastName}\n";
        }
    }
}

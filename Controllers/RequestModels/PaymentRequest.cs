using SpringBootCloneApp.Models;
using SpringBootCloneApp.Models.DTOs;

namespace SpringBootCloneApp.Controllers.RequestModels
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class PaymentRequest
    {
        [JsonPropertyName("total_price")]
        public decimal TotalPrice { get; set; }


        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItemDTO> Items { get; set; }

        [JsonPropertyName("billing_data")]
        public BillingData BillingData { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

    }

    public class OrderItemDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class BillingData
    {
        [JsonPropertyName("apartment")]
        public string Apartment { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("building")]
        public string Building { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("floor")]
        public string Floor { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }

    public class Customer
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

    }

}

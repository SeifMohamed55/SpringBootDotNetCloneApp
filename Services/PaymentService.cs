using Newtonsoft.Json;
using SpringBootCloneApp.Controllers.ResponseModels;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using SpringBootCloneApp.Controllers.RequestModels;
using NuGet.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Connections;
using System.Security.Policy;
using SpringBootCloneApp.Models;
using System.Security.Cryptography;
using System.Net;
using static System.Net.WebRequestMethods;

namespace SpringBootCloneApp.Services
{
    public interface IPaymentService
    {
        Task<IntentionResponse> CreateIntention(PaymentRequest request);
        bool IsHmacCorrect(TransactionProcessedCallBackResponse response, string hmac);
        string CreateLink(IntentionResponse response);
    }

    public class PaymentService : IPaymentService
    {
        private readonly string _publicKey;
        private readonly string _secretKey;
        private readonly string _walletsIntegrationId;
        private readonly string _cardsIntegrationId;
        private readonly string _profileHMACSecret;
        private readonly string _paymentLink;

        public PaymentService(IConfiguration configuration)
        {
            _publicKey = configuration.GetSection("Authentication")["Paymob:PublicKey"] ?? throw new Exception();
            _secretKey = configuration.GetSection("Authentication")["Paymob:SecretKey"] ?? throw new Exception();
            _cardsIntegrationId = configuration.GetSection("Authentication")["Paymob:CardsIntegrationId"] ?? throw new Exception();
            _walletsIntegrationId = configuration.GetSection("Authentication")["Paymob:WalletsIntegrationId"] ?? throw new Exception();
            _profileHMACSecret = configuration.GetSection("Authentication")["Paymob:HMAC"] ?? throw new Exception();
            _paymentLink = "https://accept.paymob.com/unifiedcheckout/?publicKey=egy_pk_test_RsbXPHclwbLwyfZWeiICf6Rgr24hzQwm&clientSecret=";

        }

        

        public async Task<IntentionResponse> CreateIntention(PaymentRequest request)
        {
            request.Items.ForEach(x => x.Amount = x.Amount * 100);

            var requestData = new
            {
                amount = request.TotalPrice * 100,
                currency = "EGP",
                payment_methods = new[] { request.PaymentMethod },
                items = request.Items, // Add item details if needed
                billing_data = request.BillingData,
                //customer = request.Customer
            };


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _secretKey);

                try
                {
                    var response = await client.PostAsJsonAsync("https://accept.paymob.com/v1/intention/", requestData);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.

                    var responseData = await response.Content.ReadAsStringAsync();

                    return await response.Content.ReadFromJsonAsync<IntentionResponse>() 
                                                        ?? throw new HttpRequestException("Response is null");
                }
                catch (HttpRequestException)
                {
                    throw;
                }
            }

        }

        public string CreateLink(IntentionResponse response)
        {
            return _paymentLink + response.client_secret;
        }


        public bool IsHmacCorrect(TransactionProcessedCallBackResponse response, string hmac)
        {
            var hmacResponseData = MakeHmacString(response);
            var calculatedHmac = CalculateHMAC(hmacResponseData);
            return calculatedHmac == hmac;

        }


        private string CalculateHMAC(string data)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_profileHMACSecret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private string MakeHmacString(TransactionProcessedCallBackResponse response)
        {
            var transaction = response.Obj;
            return $"{transaction.AmountCents}" +
                $"{transaction.CreatedAt}" +
                $"{transaction.Currency}" +
                $"{transaction.ErrorOccurred}" +
                $"{transaction.HasParentTransaction}{transaction.TransactionId}{transaction.IntegrationId}" +
                $"{transaction.Is3dSecure}" +
                $"{transaction.IsAuth}" +
                $"{transaction.IsCapture}" +
                $"{transaction.IsRefunded}" +
                $"{transaction.IsStandAlonePayment}" +
                $"{transaction.IsVoided}" +
                $"{transaction.Order.Id}" +
                $"{transaction.Owner}{transaction.Pending}" +
                $"{transaction.SourceData.Pan}{transaction.SourceData.SubType}{transaction.SourceData.Type}" +
                $"{transaction.Success}";
        }

    }
}


// https://accept.paymob.com/unifiedcheckout/?publicKey=egy_pk_test_RsbXPHclwbLwyfZWeiICf6Rgr24hzQwm&clientSecret=

// https://accept.paymobsolutions.com/api/acceptance/post_pay


/*  public async Task<TokenResponse?> AuthenticatePaymob()
        {
            var request = new
            {
                api_key = _apiKey
            };

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TokenResponse>();
        }

        public async Task<OrderResponse?> RegisterOrder(string token, PaymentRequest request)
        {
            var orderRequest = new
            {
                auth_token = token,
                delivery_needed = "false",
                amount_cents = request.Amount * 100,
                currency = "EGP",
                items = new object[] { request.Cart }  // Add items if needed
            };

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", orderRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<OrderResponse>();
        }

        public async Task<PaymentKeyResponse?> GetPaymentKey(string token, string orderId, PaymentRequest request)
        {
            var paymentKeyRequest = new
            {
                auth_token = token,
                amount_cents = request.Amount * 100,
                expiration = 3600,
                order_id = orderId,
                billing_data = new
                {
                    email = request.Email,
                    first_name = request.FirstName,
                    last_name = request.LastName,
                    phone_number = request.PhoneNumber,
                },
                currency = "EGP",
                integration_id = _integrationId
            };

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payment_keys", paymentKeyRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PaymentKeyResponse>();
        }


        public Uri PaymentUriPage(string paymentKey)
        {
            var iframURL = $"https://accept.paymob.com/api/acceptance/iframes/863458?payment_token={paymentKey}";

            return new Uri(iframURL);
        }

        public async Task<RefundResponse?> RefundTransaction(string token, RefundRequest request)
        {
            var refundRequest = new
            {
                auth_token = token,
                transaction_id = request.TransactionId,
                amount_cents = request.AmountCents,
                refund_reason = request.RefundReason
            };

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/void_refund/refund", refundRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RefundResponse>();
        }*/
using FgssrApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;

namespace FgssrApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public PaymentController()
        {
                
        }



        [HttpPost]
        [Route("GetPaymentSecret")]
        public async Task<IActionResult> GetPaymentSecret([FromBody] PaymentRequestDto prd)
        {
            var url = "https://accept.paymob.com/v1/intention/";
            var secretKey = "add your secret";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
            var paymentRequestData = new PaymentRequestDto
            {
                amount = prd.amount,
                currency = prd.currency,
                payment_methods = prd.payment_methods,
                items = prd.items,
                billing_data = prd.billing_data,
                customer = prd.customer,
            };
            var content = new StringContent(JsonSerializer.Serialize(paymentRequestData), null, "application/json");
            var response = await client.PostAsync(url , content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }


        [HttpPost]
        [Route("GetPaymentAuthToken")]
        public async Task<IActionResult> GetPaymentAuthToken()
        {
            var url = "https://accept.paymob.com/api/auth/tokens";
            var apiKey = "add your api key ";
            using var client = new HttpClient();

            var paymentRequestData = new 
            {
                api_key = apiKey,
                expiration = 99999999
            };
            var content = new StringContent(JsonSerializer.Serialize(paymentRequestData), null, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }


        [HttpPost]
        [Route("PaymentByLink")]
        public async Task<IActionResult> PaymentByLink([FromBody] PaymentRequestDto prd)
        {
            var url = "https://accept.paymob.com/api/ecommerce/payment-links";
            var loginToken = prd.token;
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);
            var paymentRequestData = new PaymentRequestDto
            {
                payment_methods = prd.payment_methods,
                email = prd.email,
                phone_number = prd.phone_number,
                full_name = prd.full_name,
                amount_cents = prd.amount_cents,
            };
            var content = new StringContent(JsonSerializer.Serialize(paymentRequestData), null, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }


        [HttpPost]
        [Route("GetTransactionById")]
        public async Task<IActionResult> GetTransactionById([FromBody] PaymentRequestDto prd)
        {
            var url = $"https://accept.paymob.com/api/acceptance/transactions/{prd.transaction_id}";
            var loginToken = prd.token;
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }


        [HttpPost]
        [Route("RefundTransactionById")]
        public async Task<IActionResult> RefundTransactionById([FromBody] PaymentRequestDto prd)
        {
            var url = "https://accept.paymob.com/api/acceptance/void_refund/refund";
            var secretKey = "add your secret";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", secretKey);
            var paymentRequestData = new PaymentRequestDto
            {
                transaction_id = prd.transaction_id,
                amount_cents = prd.amount_cents,
            };
            var content = new StringContent(JsonSerializer.Serialize(paymentRequestData), null, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }


        [HttpPost]
        [Route("CalculateProcessHmacAndCompare")]
        public async Task<IActionResult> CalculateProcessHmacAndCompare([FromBody] CallBackDataDto data , [FromQuery] string hmac)
        {

            string myHmacKey = "F95A2A2D09297C3D4BF91DFE2A5F3EDC";

            var concatenatedData = ConcatenateHmacData(data);
            var calculatedHmac = CalculateHMAC(myHmacKey,concatenatedData);
            if (calculatedHmac.Equals(hmac, StringComparison.OrdinalIgnoreCase))
            {

                return Ok(new
                {
                    message = "HMAC is Valid",
                    hmac = calculatedHmac
                });
            }
            else
            {

                return Unauthorized("Invalid HMAC.");
            }
            
        }

        [HttpPost]
        [Route("CalculateResponseHmacAndCompare")]
        public async Task<IActionResult> CalculateResponseHmacAndCompare([FromQuery] CallBackDataDto data, [FromQuery] string hmac)
        {

            string myHmacKey = "add your hmac";

            var concatenatedData = ConcatenateHmacData(data);
            var calculatedHmac = CalculateHMAC(myHmacKey, concatenatedData);
            if (calculatedHmac.Equals(hmac, StringComparison.OrdinalIgnoreCase))
            {

                return Ok(new
                {
                    message = "HMAC is Valid",
                    hmac = calculatedHmac
                });
            }
            else
            {

                return Unauthorized("Invalid HMAC.");
            }

        }




        private string ConcatenateHmacData(CallBackDataDto data)
        {
            return data.obj.amount_cents +                      
                   data.obj.created_at +
                   data.obj.currency +
                   data.obj.error_occured.ToString().ToLower() +
                   data.obj.has_parent_transaction.ToString().ToLower() + // bool > to string > to lowercase
                   data.obj.id +
                   data.obj.integration_id +
                   data.obj.is_3d_secure.ToString().ToLower() +
                   data.obj.is_auth.ToString().ToLower() +
                   data.obj.is_capture.ToString().ToLower() +
                   data.obj.is_refunded.ToString().ToLower() +
                   data.obj.is_standalone_payment.ToString().ToLower() +
                   data.obj.is_voided.ToString().ToLower() +
                   data.obj.order.id +
                   data.obj.owner +
                   data.obj.pending.ToString().ToLower() +
                   data.obj.source_data.pan +
                   data.obj.source_data.sub_type +
                   data.obj.source_data.type +
                   data.obj.success.ToString().ToLower();
        }

        private string CalculateHMAC(string hmacKey , string concatenatedData)
        {
            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(hmacKey)))
            {
                var hash = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(concatenatedData));
                return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
            }
        }




        //compare hmacs for response and process callbacks


    }
}

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Infrastructure;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public OrderingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _remoteServiceBaseUrl = $"{configuration["PurchaseUrl"]}/o/api/v1/orders";
        }

        public OrderDTO MapUserInfoIntoOrder(ClaimsPrincipal user, OrderDTO order)
        {
            var expirationSplit = user.GetExpiration().Split('/');

            order.City = user.GetCity();
            order.Street = user.GetStreet();
            order.State = user.GetState();
            order.Country = user.GetCountry();
            order.ZipCode = user.GetZipCode();

            order.CardNumber = user.GetCardNumber();
            order.CardHolderName = user.GetCardHolderName();
            order.CardExpiration = new DateTime(int.Parse("20" + expirationSplit[1]), int.Parse(expirationSplit[0]), 1);
            order.CardSecurityNumber = user.GetSecurityNumber();

            return order;
        }

        public BasketCheckoutDTO MapOrderToBasket(OrderDTO order)
        {
            order.CardExpirationApiFormat();

            return new BasketCheckoutDTO
            {
                City = order.City,
                Street = order.Street,
                State = order.State,
                Country = order.Country,
                ZipCode = order.ZipCode,
                CardNumber = order.CardNumber,
                CardHolderName = order.CardHolderName,
                CardExpiration = order.CardExpiration,
                CardSecurityNumber = order.CardSecurityNumber,
                CardTypeId = 1,
                Buyer = order.Buyer,
                RequestId = order.RequestId
            };
        }

        public async Task<List<OrderDTO>> GetMyOrders(string userId)
        {
            var uri = API.Order.GetAllMyOrders(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var response = JsonConvert.DeserializeObject<List<OrderDTO>>(responseString);

            return response;
        }

        public async Task CancelOrder(string orderId)
        {
            var uri = API.Order.CancelOrder(_remoteServiceBaseUrl);

            var order = new { OrderNumber = int.Parse(orderId) };

            var orderContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, orderContent);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling order, try later.");
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<OrderDTO> GetOrder(string userId, string orderId)
        {
            var uri = API.Order.GetOrder(_remoteServiceBaseUrl, orderId);
            
            var responseString = await _httpClient.GetStringAsync(uri);

            var response = JsonConvert.DeserializeObject<OrderDTO>(responseString);

            return response;
        }
    }
}

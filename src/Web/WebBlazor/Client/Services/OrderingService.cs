using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Security.Claims;
using WebBlazor.Client.Extensions;
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

        public OrderDTO MapUserInfoIntoOrder(/*ApplicationUser*/ClaimsPrincipal user, OrderDTO order)
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
    }
}

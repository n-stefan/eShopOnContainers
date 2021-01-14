using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebBlazor.Client.Infrastructure;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BasketService> _logger;
        private readonly IEventService _eventService;
        private readonly string _basketByPassUrl;
        private readonly string _purchaseUrl;

        public BasketService(HttpClient httpClient, IConfiguration configuration, ILogger<BasketService> logger, IEventService eventService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _eventService = eventService;

            _basketByPassUrl = $"{configuration["PurchaseUrl"]}/b/api/v1/basket";
            _purchaseUrl = $"{configuration["PurchaseUrl"]}/api/v1";
        }

        public async Task<BasketDTO> GetBasket(/*ApplicationUser user*/string userId)
        {
            var uri = API.Basket.GetBasket(_basketByPassUrl, /*user.Id*/userId);
            _logger.LogDebug("[GetBasket] -> Calling {Uri} to get the basket", uri);

            var response = await _httpClient.GetAsync(uri);
            _logger.LogDebug("[GetBasket] -> response code {StatusCode}", response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            
            return string.IsNullOrEmpty(responseString) ?
                new BasketDTO { BuyerId = /*user.Id*/userId } :
                JsonConvert.DeserializeObject<BasketDTO>(responseString);
        }

        public async Task AddItemToBasket(/*ApplicationUser user*/string userId, int productId)
        {
            var uri = API.Purchase.AddItemToBasket(_purchaseUrl);

            var newItem = new
            {
                CatalogItemId = productId,
                BasketId = /*user.Id*/userId,
                Quantity = 1
            };

            var basketContent = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, basketContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _eventService.OnBasketItemAdded(userId);
            }
        }

        public async Task<BasketDTO> SetQuantities(/*ApplicationUser user*/string userId, Dictionary<string, int> quantities)
        {
            var uri = API.Purchase.UpdateBasketItem(_purchaseUrl);

            var basketUpdate = new
            {
                BasketId = /*user.Id*/userId,
                Updates = quantities.Select(kvp => new
                {
                    BasketItemId = kvp.Key,
                    NewQty = kvp.Value
                }).ToArray()
            };

            var basketContent = new StringContent(JsonConvert.SerializeObject(basketUpdate), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, basketContent);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BasketDTO>(jsonResponse);
        }

        public async Task<BasketDTO> UpdateBasket(BasketDTO basket)
        {
            var uri = API.Basket.UpdateBasket(_basketByPassUrl);

            var basketContent = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, basketContent);

            response.EnsureSuccessStatusCode();

            return basket;
        }

        public async Task<OrderDTO> GetOrderDraft(string basketId)
        {
            var uri = API.Purchase.GetOrderDraft(_purchaseUrl, basketId);

            var responseString = await _httpClient.GetStringAsync(uri);

            var response = JsonConvert.DeserializeObject<OrderDTO>(responseString);

            return response;
        }

        public async Task Checkout(BasketCheckoutDTO basket)
        {
            var uri = API.Basket.CheckoutBasket(_basketByPassUrl);

            var basketContent = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");

            _logger.LogInformation("Uri checkout {uri}", uri);

            var response = await _httpClient.PostAsync(uri, basketContent);

            response.EnsureSuccessStatusCode();
        }
    }
}

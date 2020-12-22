using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly EventService _eventService;
        private readonly string _basketByPassUrl;
        private readonly string _purchaseUrl;

        public BasketService(HttpClient httpClient, IConfiguration configuration, ILogger<BasketService> logger, EventService eventService)
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
    }
}

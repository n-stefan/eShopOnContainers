using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebBlazor.Client.Infrastructure;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public CatalogService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _remoteServiceBaseUrl = $"{configuration["PurchaseUrl"]}/c/api/v1/catalog/";
        }

        public async Task<CatalogDTO> GetCatalogItems(int page, int take, int? brand, int? type)
        {
            var uri = API.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, page, take, brand, type);

            var catalog = await _httpClient.GetFromJsonAsync<CatalogDTO>(uri);

            return catalog;
        }

        public async Task<IEnumerable<BrandDTO>> GetBrands()
        {
            var uri = API.Catalog.GetAllBrands(_remoteServiceBaseUrl);

            var brands = await _httpClient.GetFromJsonAsync<List<BrandDTO>>(uri);

            var items = new List<BrandDTO>
            {
                new() { Id = null, Brand = "All", Selected = true }
            };

            items.AddRange(brands);

            return items;
        }

        public async Task<IEnumerable<TypeDTO>> GetTypes()
        {
            var uri = API.Catalog.GetAllTypes(_remoteServiceBaseUrl);

            var types = await _httpClient.GetFromJsonAsync<List<TypeDTO>>(uri);

            var items = new List<TypeDTO>
            {
                new() { Id = null, Type = "All", Selected = true }
            };

            items.AddRange(types);

            return items;
        }
    }
}

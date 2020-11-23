using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            var uri = GetAllCatalogItems(_remoteServiceBaseUrl, page, take, brand, type);

            var catalog = await _httpClient.GetFromJsonAsync<CatalogDTO>(uri);

            return catalog;

            //var responseString = await _httpClient.GetStringAsync(uri);

            //var catalog = JsonConvert.DeserializeObject<Catalog>(responseString);
        }

        public async Task<IEnumerable<BrandDTO>> GetBrands()
        {
            var uri = $"{_remoteServiceBaseUrl}catalogBrands";

            var brands = await _httpClient.GetFromJsonAsync<List<BrandDTO>>(uri);

            var items = new List<BrandDTO>
            {
                new BrandDTO { Id = null, Brand = "All", Selected = true }
            };

            items.AddRange(brands);

            return items;
        }

        public async Task<IEnumerable<TypeDTO>> GetTypes()
        {
            var uri = $"{_remoteServiceBaseUrl}catalogTypes";

            var types = await _httpClient.GetFromJsonAsync<List<TypeDTO>>(uri);

            var items = new List<TypeDTO>
            {
                new TypeDTO { Id = null, Type = "All", Selected = true }
            };

            items.AddRange(types);

            return items;
        }

        private static string GetAllCatalogItems(string baseUri, int page, int take, int? brand, int? type)
        {
            string filterQs;

            if (type.HasValue)
            {
                var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
                filterQs = $"/type/{type.Value}/brand/{brandQs}";
            }
            else if (brand.HasValue)
            {
                var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
                filterQs = $"/type/all/brand/{brandQs}";
            }
            else
            {
                filterQs = string.Empty;
            }

            return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
        }
    }
}

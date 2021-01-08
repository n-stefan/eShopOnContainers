using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebBlazor.Client.Infrastructure;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public LocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _remoteServiceBaseUrl = $"{configuration["MarketingUrl"]}/l/api/v1/locations/";
        }

        public async Task CreateOrUpdateUserLocation(LocationDTO location)
        {
            var uri = API.Locations.CreateOrUpdateUserLocation(_remoteServiceBaseUrl);

            var response = await _httpClient.PostAsJsonAsync(uri, location);

            response.EnsureSuccessStatusCode();
        }
    }
}

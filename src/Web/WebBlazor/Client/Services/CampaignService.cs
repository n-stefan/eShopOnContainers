using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebBlazor.Client.Infrastructure;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public CampaignService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _remoteServiceBaseUrl = $"{configuration["MarketingUrl"]}/m/api/v1/campaigns/";
        }

        public async Task<CampaignDTO> GetCampaigns(int pageSize, int pageIndex)
        {
            var uri = API.Marketing.GetAllCampaigns(_remoteServiceBaseUrl, pageSize, pageIndex);

            var campaign = await _httpClient.GetFromJsonAsync<CampaignDTO>(uri);

            return campaign;
        }

        public async Task<CampaignItemDTO> GetCampaignById(int id)
        {
            var uri = API.Marketing.GetCampaignById(_remoteServiceBaseUrl, id);

            var campaignItem = await _httpClient.GetFromJsonAsync<CampaignItemDTO>(uri);

            return campaignItem;
        }
    }
}

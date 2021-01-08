using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    interface ICampaignService
    {
        Task<CampaignDTO> GetCampaigns(int pageSize, int pageIndex);

        Task<CampaignItemDTO> GetCampaignById(int id);
    }
}

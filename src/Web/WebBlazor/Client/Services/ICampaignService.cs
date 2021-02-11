using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public interface ICampaignService
    {
        Task<CampaignDTO> GetCampaigns(int pageSize, int pageIndex);

        Task<CampaignItemDTO> GetCampaignById(int id);
    }
}

using System.Collections.Generic;

namespace WebBlazor.Client.Services.ModelDTOs
{
    public class CampaignDTO
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public List<CampaignItemDTO> Data { get; set; }
    }
}

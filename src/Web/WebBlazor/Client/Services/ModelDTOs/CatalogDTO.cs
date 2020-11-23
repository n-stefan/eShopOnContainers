using System.Collections.Generic;

namespace WebBlazor.Client.Services.ModelDTOs
{
    public class CatalogDTO
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public List<CatalogItemDTO> Data { get; set; }
    }
}

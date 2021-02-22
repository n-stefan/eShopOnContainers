using System.Collections.Generic;

namespace WebBlazor.Client.Services.ModelDTOs
{
    public record CatalogDTO
    {
        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public int Count { get; init; }

        public List<CatalogItemDTO> Data { get; init; }
    }
}

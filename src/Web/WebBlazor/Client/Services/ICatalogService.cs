using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    interface ICatalogService
    {
        Task<CatalogDTO> GetCatalogItems(int page, int take, int? brand, int? type);

        Task<IEnumerable<BrandDTO>> GetBrands();

        Task<IEnumerable<TypeDTO>> GetTypes();
    }
}

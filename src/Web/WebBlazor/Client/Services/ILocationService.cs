using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public interface ILocationService
    {
        Task CreateOrUpdateUserLocation(LocationDTO location);
    }
}

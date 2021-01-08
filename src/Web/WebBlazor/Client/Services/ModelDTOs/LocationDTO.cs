using System.ComponentModel.DataAnnotations;
using WebBlazor.Client.Services.ModelDTOs.Annotations;

namespace WebBlazor.Client.Services.ModelDTOs
{
    public class LocationDTO
    {
        [LongitudeCoordinate, Required]
        public double Longitude { get; set; } = -122.315752;

        [LatitudeCoordinate, Required]
        public double Latitude { get; set; } = 47.604610;
    }
}

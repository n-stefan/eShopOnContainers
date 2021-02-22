
namespace WebBlazor.Client.Services.ModelDTOs
{
    public record BrandDTO
    {
        public int? Id { get; init; }

        public string Brand { get; init; }

        public bool Selected { get; init; }
    }
}

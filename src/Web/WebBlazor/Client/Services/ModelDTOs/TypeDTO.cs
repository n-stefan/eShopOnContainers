
namespace WebBlazor.Client.Services.ModelDTOs
{
    public record TypeDTO
    {
        public int? Id { get; init; }

        public string Type { get; init; }

        public bool Selected { get; init; }
    }
}

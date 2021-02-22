
namespace WebBlazor.Client.Services.ModelDTOs
{
    public record BasketItemDTO
    {
        public string Id { get; init; }

        public string ProductId { get; init; }

        public string ProductName { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal OldUnitPrice { get; init; }

        public int Quantity { get; set; }

        public string PictureUrl { get; init; }
    }
}

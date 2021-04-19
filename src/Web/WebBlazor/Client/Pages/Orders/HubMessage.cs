
namespace WebBlazor.Client.Pages.Orders
{
    public record HubMessage
    {
        public int OrderId { get; init; }

        public string Status { get; init; }
    }
}

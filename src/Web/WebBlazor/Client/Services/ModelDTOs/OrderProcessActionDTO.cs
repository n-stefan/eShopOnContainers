
namespace WebBlazor.Client.Services.ModelDTOs
{
    public record OrderProcessActionDTO
    {
        public string Code { get; }

        public string Name { get; }

        public static OrderProcessActionDTO Ship = new(nameof(Ship).ToLowerInvariant(), "Ship");

        protected OrderProcessActionDTO()
        {
        }

        public OrderProcessActionDTO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}

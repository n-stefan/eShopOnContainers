using System;
using System.Collections.Generic;
using System.Linq;

namespace WebBlazor.Client.Services.ModelDTOs
{
    public record BasketDTO
    {
        public List<BasketItemDTO> Items { get; init; } = new();

        public string BuyerId { get; init; }

        public decimal Total() =>
            Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
    }
}

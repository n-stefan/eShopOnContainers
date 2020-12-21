using System;
using System.Collections.Generic;
using System.Linq;

namespace WebBlazor.Client.Services.ModelDTOs
{
    public class BasketDTO
    {
        public List<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>();

        public string BuyerId { get; set; }

        public decimal Total() =>
            Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
    }
}

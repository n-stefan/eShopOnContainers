
namespace WebBlazor.Client.Infrastructure
{
    public class API
    {
        public static class Catalog
        {
            public static string GetAllCatalogItems(string baseUri, int page, int take, int? brand, int? type)
            {
                string filterQs;

                if (type.HasValue)
                {
                    var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
                    filterQs = $"/type/{type.Value}/brand/{brandQs}";
                }
                else if (brand.HasValue)
                {
                    var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
                    filterQs = $"/type/all/brand/{brandQs}";
                }
                else
                {
                    filterQs = string.Empty;
                }

                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }

            public static string GetAllBrands(string baseUri) =>
                $"{baseUri}catalogBrands";

            public static string GetAllTypes(string baseUri) =>
                $"{baseUri}catalogTypes";
        }

        public static class Basket
        {
            public static string GetBasket(string baseUri, string basketId) =>
                $"{baseUri}/{basketId}";

            public static string UpdateBasket(string baseUri) =>
                baseUri;

            public static string CheckoutBasket(string baseUri) =>
                $"{baseUri}/checkout";
        }

        public static class Purchase
        {
            public static string AddItemToBasket(string baseUri) =>
                $"{baseUri}/basket/items";

            public static string UpdateBasketItem(string baseUri) =>
                $"{baseUri}/basket/items";

            public static string GetOrderDraft(string baseUri, string basketId) =>
                $"{baseUri}/order/draft/{basketId}";
        }

        public static class Marketing
        {
            public static string GetAllCampaigns(string baseUri, int take, int page) =>
                $"{baseUri}user?pageSize={take}&pageIndex={page}";

            public static string GetCampaignById(string baseUri, int id) =>
                $"{baseUri}{id}";
        }

        public static class Locations
        {
            public static string CreateOrUpdateUserLocation(string baseUri) =>
                baseUri;
        }

        public static class Order
        {
            public static string GetAllMyOrders(string baseUri) =>
                baseUri;

            public static string CancelOrder(string baseUri) =>
                $"{baseUri}/cancel";
        }
    }
}

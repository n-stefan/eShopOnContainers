
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
        }
    }
}

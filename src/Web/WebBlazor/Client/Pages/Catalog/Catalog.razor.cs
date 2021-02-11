using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;
using WebBlazor.Client.Services.ModelDTOs;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Pages.Catalog
{
    public partial class Catalog
    {
        private bool errorReceived;
        private List<BrandDTO> brands = new List<BrandDTO>();
        private int? brandSelected;
        private List<TypeDTO> types = new List<TypeDTO>();
        private int? typeSelected;
        private CatalogDTO catalog;
        private PagerInfo paginationInfo = new PagerInfo();
        private bool authenticated;
        private string userId;
        
        [Inject]
        private ICatalogService catalogService { get; set; }
        [Inject]
        private IBasketService basketService { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await authenticationStateTask).User;
            authenticated = user.Identity.IsAuthenticated;
            if (authenticated)
                userId = user.GetSub();
            await LoadData();
        }

        private async Task LoadData()
        {
            var brandData = await catalogService.GetBrands();
            brands = brandData.ToList();
            await GetCatalog(10, 0);
            var typeData = await catalogService.GetTypes();
            types = typeData.ToList();
        }

        private async Task GetCatalog(int pageSize, int pageIndex, int? brand = null, int? type = null)
        {
            errorReceived = false;
            try
            {
                catalog = await catalogService.GetCatalogItems(pageIndex, pageSize, brand, type);
                paginationInfo.ActualPage = catalog.PageIndex;
                paginationInfo.ItemsPage = catalog.PageSize;
                paginationInfo.TotalItems = catalog.Count;
                paginationInfo.TotalPages = (int)Math.Ceiling((decimal)catalog.Count / catalog.PageSize);
                paginationInfo.Items = catalog.Data.Count;
            }
            catch (Exception)
            {
                errorReceived = true;
                throw;
            }
        }

        private void OnBrandFilterChanged(ChangeEventArgs e)
        {
            var value = e.Value.ToString();
            if (value == "All")
                brandSelected = null;
            else
                brandSelected = int.Parse(value);
        }

        private void OnTypeFilterChanged(ChangeEventArgs e)
        {
            var value = e.Value.ToString();
            if (value == "All")
                typeSelected = null;
            else
                typeSelected = int.Parse(value);
        }

        private async Task OnFilterApplied()
        {
            await GetCatalog(paginationInfo.ItemsPage, 0/*paginationInfo.ActualPage*/, brandSelected, typeSelected);
        }

        private async Task OnPageChanged(int value)
        {
            paginationInfo.ActualPage = value;
            await GetCatalog(paginationInfo.ItemsPage, value);
        }

        private async Task AddToCart(CatalogItemDTO item)
        {
            await basketService.AddItemToBasket(userId, item.Id);
        }
    }
}

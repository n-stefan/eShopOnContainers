﻿using Microsoft.AspNetCore.Components;
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
        private List<BrandDTO> brands = new();
        private int? brandSelected;
        private List<TypeDTO> types = new();
        private int? typeSelected;
        private CatalogDTO catalog;
        private readonly PagerInfo paginationInfo = new();
        private bool authenticated;
        private string userId;
        
        [Inject]
        private ICatalogService CatalogService { get; set; }

        [Inject]
        private IBasketService BasketService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthenticationStateTask).User;
            authenticated = user.Identity.IsAuthenticated;
            if (authenticated)
                userId = user.GetSub();
            await LoadData();
        }

        private async Task LoadData()
        {
            var brandData = await CatalogService.GetBrands();
            brands = brandData.ToList();
            await GetCatalog(9, 0);
            var typeData = await CatalogService.GetTypes();
            types = typeData.ToList();
        }

        private async Task GetCatalog(int pageSize, int pageIndex, int? brand = null, int? type = null)
        {
            errorReceived = false;
            try
            {
                catalog = await CatalogService.GetCatalogItems(pageIndex, pageSize, brand, type);
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
            paginationInfo.ActualPage = 0;
            await GetCatalog(paginationInfo.ItemsPage, paginationInfo.ActualPage, brandSelected, typeSelected);
        }

        private async Task OnPageChanged(int value)
        {
            paginationInfo.ActualPage = value;
            await GetCatalog(paginationInfo.ItemsPage, value);
        }

        private async Task AddToCart(CatalogItemDTO item) =>
            await BasketService.AddItemToBasket(userId, item.Id);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlazor.Client.Services;
using WebBlazor.Client.Services.ModelDTOs;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Pages.Campaigns
{
    [Authorize]
    public partial class Campaigns
    {
        private bool errorReceived;
        private bool isCampaignDetailFunctionEnabled;
        private CampaignDTO campaigns;
        private LocationDTO location = new LocationDTO();
        private PagerInfo paginationInfo = new PagerInfo();
        private List<HeaderInfo> header = new List<HeaderInfo> { new HeaderInfo { Url = "/catalog", Text = "Back to catalog" } };
        
        [Inject]
        private ICampaignService campaignService { get; set; }
        [Inject]
        private ILocationService locationService { get; set; }
        [Inject]
        private IConfiguration configuration { get; set; }
        [Inject]
        private NavigationManager navigation { get; set; }
        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetCampaigns(9, 0);
            isCampaignDetailFunctionEnabled = configuration["ActivateCampaignDetailFunction"] == bool.TrueString;
        }

        private async Task OnPageChanged(int value)
        {
            paginationInfo.ActualPage = value;
            await GetCampaigns(paginationInfo.ItemsPage, value);
        }

        private async Task GetCampaigns(int pageSize, int pageIndex)
        {
            errorReceived = false;
            try
            {
                campaigns = await campaignService.GetCampaigns(pageSize, pageIndex);
                paginationInfo.ActualPage = campaigns.PageIndex;
                paginationInfo.ItemsPage = campaigns.PageSize;
                paginationInfo.TotalItems = campaigns.Count;
                paginationInfo.TotalPages = (int)Math.Ceiling((decimal)campaigns.Count / campaigns.PageSize);
                paginationInfo.Items = campaigns.Data.Count;
            }
            catch (Exception)
            {
                errorReceived = true;
                throw;
            }
        }

        private async Task OnNavigateToDetails(string uri)
        {
            if (!string.IsNullOrWhiteSpace(uri))
                await jsRuntime.InvokeVoidAsync("openBlankWindow", uri);
        }

        private void OnNavigateToDetails(int id)
        {
            if (id > 0)
                navigation.NavigateTo($"/campaigns/{id}");
        }

        private async Task UpdateUserLocation()
        {
            await locationService.CreateOrUpdateUserLocation(location);
        }
    }
}

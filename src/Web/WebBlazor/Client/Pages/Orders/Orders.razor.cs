using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;
using WebBlazor.Client.Services.ModelDTOs;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Pages.Orders
{
    [Authorize]
    public partial class Orders : IAsyncDisposable
    {
        private bool errorReceived;
        private string userId;
        private HubConnection hubConnection;
        private List<OrderDTO> orders = new();
        private List<HeaderInfo> header = new()
        {
            new HeaderInfo { Url = "catalog", Text = "Back to catalog" },
            new HeaderInfo { Text = "/" },
            new HeaderInfo { Url = "ordersmanagement", Text = "Orders Management" }
        };
        
        [Inject]
        private IOrderingService OrderingService { get; set; }

        [Inject]
        private IConfiguration Configuration { get; set; }

        [Inject]
        private IAccessTokenProvider TokenProvider { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userId = (await AuthenticationStateTask).User.GetSub();
            await GetOrders();
            await InitSignalR();
        }

        private async Task InitSignalR()
        {
            var tokenResult = await TokenProvider.RequestAccessToken(new AccessTokenRequestOptions
            {
                Scopes = new[] { "orders.signalrhub" }
            });
            if (!tokenResult.TryGetToken(out var token))
            {
                return;
            }
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Configuration["SignalrHubUrl"]}/hub/notificationhub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token.Value);
                })
                .WithAutomaticReconnect()
                .Build();
            hubConnection.On<HubMessage>("UpdatedOrderState", async _ =>
            {
                await GetOrders();
                StateHasChanged();
            });
            await hubConnection.StartAsync();
        }

        private async Task GetOrders()
        {
            try
            {
                orders = await OrderingService.GetMyOrders(userId);
                errorReceived = false;
            }
            catch (Exception)
            {
                errorReceived = true;
                throw;
            }
        }

        private async Task CancelOrder(string orderNumber)
        {
            await OrderingService.CancelOrder(orderNumber);
        }

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }
    }
}

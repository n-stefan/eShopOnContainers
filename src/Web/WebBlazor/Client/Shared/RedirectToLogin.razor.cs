using Microsoft.AspNetCore.Components;
using System;

namespace WebBlazor.Client.Shared
{
    public partial class RedirectToLogin : ComponentBase
    {
        [Inject]
        private NavigationManager navigation { get; set; }

        protected override void OnInitialized()
        {
            navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(navigation.Uri)}");
        }
    }
}

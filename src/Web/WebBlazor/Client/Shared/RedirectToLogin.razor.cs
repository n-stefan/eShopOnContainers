using Microsoft.AspNetCore.Components;
using System;

namespace WebBlazor.Client.Shared
{
    public partial class RedirectToLogin : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
        }
    }
}

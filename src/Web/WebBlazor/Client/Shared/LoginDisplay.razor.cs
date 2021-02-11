using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Threading.Tasks;

namespace WebBlazor.Client.Shared
{
    public partial class LoginDisplay
    {
        [Inject]
        private NavigationManager navigation { get; set; }
        [Inject]
        private SignOutSessionStateManager signOutManager { get; set; }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await signOutManager.SetSignOutState();
            navigation.NavigateTo("authentication/logout");
        }
    }
}

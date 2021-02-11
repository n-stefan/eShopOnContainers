using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Shared
{
    public partial class Pager
    {
        private bool nextDisabled = true;
        private bool previousDisabled = true;

        [Parameter]
        public PagerInfo Model { get; set; }

        [Parameter]
        public EventCallback<int> Changed { get; set; }

        protected override void OnParametersSet()
        {
            if (Model != null)
            {
                //Model.Items = (Model.ItemsPage > Model.Items) ? Model.Items : Model.ItemsPage;

                previousDisabled = (Model.ActualPage == 0);
                nextDisabled = (Model.ActualPage + 1 >= Model.TotalPages);
            }
        }

        private async Task OnPreviousClicked()
        {
            await Changed.InvokeAsync(Model.ActualPage - 1);
        }

        private async Task OnNextClicked()
        {
            await Changed.InvokeAsync(Model.ActualPage + 1);
        }
    }
}

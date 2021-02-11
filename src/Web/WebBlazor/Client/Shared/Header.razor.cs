using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Shared
{
    public partial class Header
    {
        [Parameter]
        public IEnumerable<HeaderInfo> Model { get; set; }
    }
}

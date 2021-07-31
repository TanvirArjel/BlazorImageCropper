using Microsoft.AspNetCore.Components;

namespace BlazorImageCropper.Shared
{
    public partial class ImageComponent
    {
        [Parameter]
        public string Src { get; set; }

        [Parameter]
        public string Width { get; set; }

        [Parameter]
        public string Height { get; set; }

        [Parameter]
        public string AltText { get; set; }

        [Parameter]
        public string CssClass { get; set; }
    }
}

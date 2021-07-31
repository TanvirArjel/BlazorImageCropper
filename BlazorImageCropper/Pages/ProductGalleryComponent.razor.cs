using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorImageCropper.Models;
using BlazorImageCropper.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorImageCropper.Pages
{
    public partial class ProductGalleryComponent
    {
        [Inject]
        private ProductService ProductService { get; set; }
        private List<Product> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetListAsync();
        }
    }
}

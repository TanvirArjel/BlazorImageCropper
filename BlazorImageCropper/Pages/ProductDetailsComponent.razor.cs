using System;
using System.Threading.Tasks;
using BlazorImageCropper.Models;
using BlazorImageCropper.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorImageCropper.Pages
{
    public partial class ProductDetailsComponent
    {
        [Inject]
        private ProductService ProductService { get; set; }

        [Parameter]
        public Guid ProductId { get; set; }

        private Product Product { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Product = await ProductService.GetByIdAsync(ProductId);
        }
    }
}

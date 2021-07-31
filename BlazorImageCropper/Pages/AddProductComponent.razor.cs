using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using BlazorImageCropper.Models;
using BlazorImageCropper.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorImageCropper.Pages
{
    public partial class AddProductComponent
    {
        [Inject]
        private ProductService ProductService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private EditContext EditContext { get; set; }

        private CreateProductModel InputModel { get; set; } = new CreateProductModel();

        private string ImageBase64String { get; set; }
        private string PreviewImagePath { get; set; }

        protected override void OnInitialized()
        {
            EditContext = new EditContext(InputModel);
            EditContext.AddDataAnnotationsValidation();
        }

        private async Task HandleValidSubmit()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = InputModel.Name,
                ImagePath = PreviewImagePath
            };

            await ProductService.AddAsync(product);

            NavigationManager.NavigateTo("product-gallery");
        }

        private async Task HandleInputFileChange(InputFileChangeEventArgs e)
        {
            await UpdatePreviewASync(e.File);
            InputModel.Image = e.File;
            EditContext.Validate();
        }

        private async Task UpdatePreviewASync(IBrowserFile browserFile)
        {
            Stream inputFileStream = browserFile.OpenReadStream();
            using MemoryStream memoryStream = new MemoryStream();
            await inputFileStream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            ImageBase64String = Convert.ToBase64String(imageBytes);
            PreviewImagePath = $"data:image/png;base64,{ImageBase64String}";
        }

        private int Width { get; set; }

        private int Height { get; set; }

        private async Task ResizeImageAsync()
        {
            if (Width > 0 && Height > 0)
            {
                IBrowserFile image = InputModel.Image;
                if (image != null)
                {
                    image = await image.RequestImageFileAsync("image/jpg", Width, Height);
                    await UpdatePreviewASync(image);
                }
            }
        }

        private class CreateProductModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public IBrowserFile Image { get; set; }
        }
    }
}

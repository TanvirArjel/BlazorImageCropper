using System;
using System.IO;
using System.Threading.Tasks;
using Blazor.Cropper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorImageCropper.Pages
{
    public partial class ImageCropperComponent
    {
        private Cropper cropper;

        private IBrowserFile file;
        private string PreviewImagePath { get; set; }
        private string ImageBase64String { get; set; }

        private readonly string prompt = "Image cropped! Parsing to base64...";
        private bool parsing = false;

        private bool ShowCroper { get; set; } = false;

        private bool IsAspectRatioEnabled { get; set; }

        private double AspectRatio { get; set; } = 1d;

        private double ratio = 1;


        private double AspectWidth { get; set; } = 1;

        private void OnAspectWidthChanged(ChangeEventArgs eventArgs)
        {
            AspectWidth = double.Parse((string)eventArgs.Value);

            AspectRatio = AspectHeight / AspectWidth;
        }

        private double AspectHeight { get; set; } = 1;

        private void OnAspectHeightChanged(ChangeEventArgs eventArgs)
        {
            AspectHeight = double.Parse((string)eventArgs.Value);

            AspectRatio = AspectHeight / AspectWidth;
        }

        private void OnRatioChange(ChangeEventArgs args)
        {
            ratio = int.Parse(args.Value.ToString()) / 100.0;
        }

        private void OnInputFileChange(InputFileChangeEventArgs args)
        {
            PreviewImagePath = null;
            file = args.File;
            ShowCroper = true;
        }

        private double CropCurrentWidth { get; set; }
        private double CropCurrentHeight { get; set; }
        private void HandleCropSizeChanged((double, double) cropSize)
        {
            CropCurrentWidth = cropSize.Item1;
            CropCurrentHeight = cropSize.Item2;
        }

        private async Task DoneCrop()
        {
            ImageCroppedResult args = await cropper.GetCropedResult();
            ShowCroper = false;
            parsing = true;
            base.StateHasChanged();
            await Task.Delay(10);// a hack, otherwise prompt won't show
            await JSRuntime.InvokeVoidAsync("console.log", "converted!");

            string base64String = await args.GetBase64Async();
            PreviewImagePath = $"data:image/png;base64,{base64String}";
            args.Dispose();
            parsing = false;
        }

        private async Task CancelCropAsync()
        {
            ShowCroper = false;
            await UpdatePreviewASync(file);
        }

        private readonly int MaxAllowedFileSize = 10 * 1024 * 1024;

        private async Task UpdatePreviewASync(IBrowserFile browserFile)
        {
            Stream inputFileStream = browserFile.OpenReadStream(MaxAllowedFileSize);
            using MemoryStream memoryStream = new MemoryStream();
            await inputFileStream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            ImageBase64String = Convert.ToBase64String(imageBytes);
            PreviewImagePath = $"data:image/png;base64,{ImageBase64String}";
        }
    }
}

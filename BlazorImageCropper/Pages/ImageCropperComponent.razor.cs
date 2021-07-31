﻿using System;
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

        private bool _aspectRatioEnalbed = false;
        private bool IsAspectRatioEnabled
        {
            get => _aspectRatioEnalbed;
            set
            {
                if (value == true)
                {
                    AspectRatio = CropHeight / CropHeight;
                }

                _aspectRatioEnalbed = value;
            }
        }

        private double _aspectRatio = 1d;
        private double AspectRatio
        {
            get => _aspectRatio;
            set
            {
                CropHeight = value * CropWidth;
                _aspectRatio = value;
            }
        }

        private double ratio = 1;

        private double cropWidth = 200;
        private double CropWidth
        {
            get => cropWidth;
            set
            {
                if (IsAspectRatioEnabled)
                {
                    CropHeight = value * AspectRatio;
                }

                cropWidth = value;
            }
        }

        private double CropHeight { get; set; } = 200;

        private double _aspectWidth = 1;
        private double AspectWidth
        {
            get => _aspectWidth;
            set
            {
                if (IsAspectRatioEnabled)
                {
                    AspectRatio = AspectHeight / value;
                }

                _aspectWidth = value;
            }
        }

        private double _aspectHeight = 1;
        private double AspectHeight
        {
            get => _aspectHeight;
            set
            {
                if (IsAspectRatioEnabled)
                {
                    AspectRatio = value / AspectWidth;
                }

                _aspectHeight = value;
            }
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

        private async Task DoneCrop()
        {
            ImageCroppedResult args = await cropper.GetCropedResult();
            ShowCroper = false;
            parsing = true;
            base.StateHasChanged();
            await Task.Delay(10);// a hack, otherwise prompt won't show
            await JSRuntime.InvokeVoidAsync("console.log", "converted!");
            PreviewImagePath = await args.GetBase64Async();
            args.Dispose();
            parsing = false;
        }

        private async Task CancelCropAsync()
        {
            ShowCroper = false;
            await UpdatePreviewASync(file);
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
    }
}
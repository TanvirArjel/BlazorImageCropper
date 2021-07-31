using System;

namespace BlazorImageCropper.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}

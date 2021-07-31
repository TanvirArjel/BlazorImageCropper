using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorImageCropper.Models;

namespace BlazorImageCropper.Services
{
    public class ProductService
    {
        private readonly ILocalStorageService _localStorage;
        public ProductService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;

        }

        private readonly string ProductListKey = "LocalProducts";

        public async Task<List<Product>> GetListAsync()
        {
            List<Product> Products = await _localStorage.GetItemAsync<List<Product>>(ProductListKey);
            return Products;
        }

        public async Task<Product> GetByIdAsync(Guid productId)
        {
            List<Product> Products = await _localStorage.GetItemAsync<List<Product>>(ProductListKey);
            return Products.Where(p => p.Id == productId).FirstOrDefault();
        }

        public async Task AddAsync(Product product)
        {
            List<Product> Products = await _localStorage.GetItemAsync<List<Product>>(ProductListKey);

            Products ??= new List<Product>() { product };
            Products.Add(product);

            await _localStorage.SetItemAsync(ProductListKey, Products);
        }
    }
}

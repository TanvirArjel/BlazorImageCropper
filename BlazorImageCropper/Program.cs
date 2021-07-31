using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorImageCropper.Models;
using BlazorImageCropper.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorImageCropper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<ProductService>();
            builder.Services.AddBlazoredLocalStorage();

            ILocalStorageService localStorageService = builder.Services.BuildServiceProvider().GetService<ILocalStorageService>();

            List<Product> Products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Swimming Constume",
                    ImagePath = "images/fun-swimming-products.jpg"
                },
                new Product()
                {
                    Id =  Guid.NewGuid(),
                    Name = "Floaty Fun",
                    ImagePath = "images/pool-floaty-fun.jpg"
                },
                new Product()
                {
                    Id =  Guid.NewGuid(),
                    Name = "Red T-shirt",
                    ImagePath = "images/red-t-shirt.jpg"
                },
            };

            await localStorageService.SetItemAsync("LocalProducts", Products);

            await builder.Build().RunAsync();
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Data
{
    public static class MockupDb
    {
        public static void PrepDb(IApplicationBuilder app)
        {
            using(var serviceScoped = app.ApplicationServices.CreateScope())
            {
                PopulateDb(serviceScoped.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void PopulateDb(AppDbContext context)
        {
            context.Carts.Add(new Cart() { Id = new Guid("f019002f-d22c-4cbe-abbb-68d3a278b83c") });

            Product vase = new Product() { Name = "vase", Price = 1.2, Id = new Guid("f019002f-d22c-4cbe-abbb-68d3a278b111") };
            Product mug = new Product() { Name = "mug", Price = 1, Id = new Guid("f019002f-d22c-4cbe-abbb-68d3a278b832") };
            Product napkins = new Product() { Name = "napkins", Price = 0.45, Id = new Guid("f019002f-d22c-4cbe-abbb-68d3a278b84c") };

            context.Products.AddRange(vase, mug, napkins);

            context.SaveChanges();

            context.Discounts.AddRange(
                new Discount() { ProductId = napkins.Id, Quantity = 3, Percentage = 0.33},
                new Discount() { ProductId = mug.Id, Quantity = 2, Percentage = 0.25}
                //new Discount() { ProductId = mug.Id, Quantity = 1, Percentage = 1}
                );

            context.SaveChanges();
        }
    }
}

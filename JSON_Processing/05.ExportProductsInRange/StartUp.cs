using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();

            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            var inputJson = File.ReadAllText("./../../../Datasets/categories-products.json");

            Console.WriteLine(GetProductsInRange(db));
        }

        public static string GetProductsInRange(ProductShopContext context)
        {


            var products = context.Products.Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x=>x.price)
                .ToList();

            var jsonProduct = JsonConvert.SerializeObject(products,Formatting.Indented);
            
           

            return jsonProduct;

        }
    }
}
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

            Console.WriteLine(GetCategoriesByProductsCount(db));
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {


            var categories = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(y => new
                {
                    category = y.Name,
                    productsCount = y.CategoryProducts.Count,
                    averagePrice=y.CategoryProducts
                    .Average(p=>p.Product.Price)
                    .ToString("f2"),
                    totalRevenue=y.CategoryProducts
                    .Sum(p=>p.Product.Price)
                    .ToString("f2")

                })
                .ToList();

            var jsonProduct = JsonConvert.SerializeObject(categories, Formatting.Indented);
          
            return jsonProduct;

        }
    }
}

//Get all categories. Order them in descending order by the category’s products count. 
//For each category select its name, the number of products,
//the average price of those products (rounded to second digit after the decimal separator)
//and the total revenue (total price sum and rounded to second digit after the decimal separator) 
//of those products (regardless if they have a buyer or not).
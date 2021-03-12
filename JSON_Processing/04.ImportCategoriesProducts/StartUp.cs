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

            Console.WriteLine(ImportCategoryProducts(db, inputJson));
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {


            var categoriesProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoriesProducts);
            
            
           
           
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";

        }
    }
}
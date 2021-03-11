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

            var inputJson = File.ReadAllText("./../../../Datasets/categories.json");

            Console.WriteLine(ImportCategories(db, inputJson));
        }

         public static string ImportCategories(ProductShopContext context, string inputJson)
        {

            
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(x=>x.Name!=null).ToList();

            context.Categories.AddRange(categories);
           
           
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";

        }
    }
}
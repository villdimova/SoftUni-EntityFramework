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

            Console.WriteLine(GetUsersWithProducts(db));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {

            var allUsers = context.Users
                  .ToList()
                  .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
                  .Select(u => new
                  {
                      firstName = u.FirstName,
                      lastName = u.LastName,
                      age = u.Age,
                      soldProducts = new
                      {
                          count = u.ProductsSold.Where(z => z.Buyer != null).Count(),
                          products = u.ProductsSold.Where(z => z.Buyer != null).Select(p => new
                          {
                              name = p.Name,
                              price = p.Price

                          })

                      }
                  }).OrderByDescending(x => x.soldProducts.count)
                  .ToList();

            var resultObject = new
            {
                usersCount = allUsers.Count(),
                users = allUsers,
            };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore

            };


            var resultJson = JsonConvert.SerializeObject(resultObject, jsonSettings);

            return resultJson;
        }
    }
}

//Get all users who have at least 1 sold product with a buyer. 
//Order them in descending order by the number of sold products with a buyer. 
//Select only their first and last name, age and for each product - name and price. 
//Ignore all null values.
//Export the results to JSON. 
//Follow the format below to better understand how to structure your data. 

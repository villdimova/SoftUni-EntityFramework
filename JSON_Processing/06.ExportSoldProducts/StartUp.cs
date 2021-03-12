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

            Console.WriteLine(GetSoldProducts(db));
        }

        public static string GetSoldProducts(ProductShopContext context)
        {


            var soldProducts = context.Users.Where(x => x.ProductsSold.Any(y=>y.Buyer!=null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Where(y=>y.Buyer!=null).Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    })

                })
                .OrderBy(x => x.lastName).ThenBy(x => x.lastName)
                .ToList();

            var jsonProduct = JsonConvert.SerializeObject(soldProducts,Formatting.Indented);
          
            return jsonProduct;

        }
    }
}
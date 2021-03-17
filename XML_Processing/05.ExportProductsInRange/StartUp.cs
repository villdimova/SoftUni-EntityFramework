using ProductShop.Data;
using ProductShop.Dtos;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();

            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            System.Console.WriteLine(GetProductsInRange(db));


        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(x => new ExportProductDto
                {
                    Name = x.Name,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName,
                    Price = x.Price
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToList();


            var xmlSerializer = new XmlSerializer(typeof(List<ExportProductDto>)
                                                , new XmlRootAttribute("Products"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer= new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, products,namespaces);
            }

            return sb.ToString().Trim();

            
        }

        //Get all products in a specified price range between 500 and 1000 (inclusive).
        //    Order them by price(from lowest to highest). 
        //    Select only the product name, price and the full name of the buyer.Take top 10 records.
    }
}
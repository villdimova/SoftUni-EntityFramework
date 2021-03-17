using ProductShop.Data;
using ProductShop.Dtos;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var xml = File.ReadAllText("./Datasets/products.xml");

            System.Console.WriteLine(ImportProducts(db, xml));


        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {

            var xmlSerializer = new XmlSerializer(typeof(ProductInputModel[])
                                                   , new XmlRootAttribute("Products"));

            var productDtos = (ProductInputModel[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var products = new List<Product>();

            foreach (var productDto in productDtos)
            {
                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId

                };

                products.Add(product);
            }
           

            context.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

    }
}
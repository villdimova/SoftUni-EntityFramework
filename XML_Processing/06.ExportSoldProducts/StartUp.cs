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

            System.Console.WriteLine(GetSoldProducts(db));


        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var users = context.Users
                .Where(x => x.ProductsSold.Count >= 1)
                .Select(y => new ExportUsersSoldProductsDto()
                {
                    FirstName = y.FirstName,
                    LastName = y.LastName,
                    SoldProducts = y.ProductsSold.Where(p => p.Buyer != null)
                                    .Select(p => new UserProductDto
                                    {
                                        Name = p.Name,
                                        Price = p.Price

                                    }).ToArray()
                }).OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                   .Take(5)
                   .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUsersSoldProductsDto[])
                                                , new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer= new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, users, namespaces);
            }

            return sb.ToString().Trim();

            
        }

        
    }
}

//Get all users who have at least 1 sold item. 
//    Order them by last name, then by first name. Select the person's
//    first and last name. For each of the sold products, select the product's name and price. 
//    Take top 5 records. 
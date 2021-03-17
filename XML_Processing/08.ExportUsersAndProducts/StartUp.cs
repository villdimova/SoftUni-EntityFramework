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

            System.Console.WriteLine(GetUsersWithProducts(db));


        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var usersProductsInfo = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .ToArray()
                .Select(x => new ExportUserDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new ExportSoldProductsDto
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold.Select(p => new ExportProductsDto
                        {
                            Name = p.Name,
                            Price = p.Price

                        }).OrderByDescending(z=>z.Price)
                        .ToArray()
                    }

                })
                .OrderByDescending(x=>x.SoldProducts.Count)
                .Take(10)
                .ToArray();

            var result = new ExportUsersCountDto
            {
                Count = context.Users.Where(x=>x.ProductsSold.Count>=1).Count(),
                Users= usersProductsInfo

            };
           
            var xmlSerializer = new XmlSerializer(typeof(ExportUsersCountDto)
                                                , new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer= new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, result, namespaces);
            }

            return sb.ToString().Trim();

            
        }

        
    }
}

//Select users who have at least 1 sold product. 
//Order them by the number of sold products (from highest to lowest). 
//Select only their first and last name, age, count of sold products and 
 //for each product - name and price sorted by price (descending). Take top 10 records.
//Follow the format below to better understand how to structure your data. 


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

            System.Console.WriteLine(GetCategoriesByProductsCount(db));


        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories
                .Select(x => new ExportCategoriesByProductsDto
                {
                    Name = x.Name,
                    AveragePrice = x.CategoryProducts.Average(y => y.Product.Price),
                    Count = x.CategoryProducts.Count(),
                    TotalRevenue = x.CategoryProducts.Sum(y => y.Product.Price)
                }).OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToList();
           
            var xmlSerializer = new XmlSerializer(typeof(List<ExportCategoriesByProductsDto>)
                                                , new XmlRootAttribute("Categories"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer= new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, categories, namespaces);
            }

            return sb.ToString().Trim();

            
        }

        
    }
}

//Get all categories. For each category select its name, the number of products, 
//the average price of those products and the total revenue (total price sum) 
 //of those products (regardless if they have a buyer or not). 
//Order them by the number of products (descending) then by total revenue.
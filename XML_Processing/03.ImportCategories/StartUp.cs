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

            var xml = File.ReadAllText("./Datasets/categories.xml");

            System.Console.WriteLine(ImportCategories(db, xml));


        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {

            var xmlSerialyzer = new XmlSerializer(typeof(CategoryInputModel[])
                                                , new XmlRootAttribute("Categories"));
            var categoryDtos = (CategoryInputModel[])xmlSerialyzer
                                                .Deserialize(new StringReader(inputXml));
            var categories = new List<Category>();

            foreach (var categoryDto in categoryDtos.Where(c=>c.Name!=null))
            {
                var category = new Category()
                {
                    Name = categoryDto.Name
                };

                categories.Add(category);
            }

          
            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

    }
}
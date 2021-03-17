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

            var xml = File.ReadAllText("./Datasets/categories-products.xml");

            System.Console.WriteLine(ImportCategoryProducts(db, xml));


        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {

            var xmlSerializer = new XmlSerializer(typeof(CategoryProductInputModel[]),
                                                 new XmlRootAttribute("CategoryProducts"));
            var categoryProductsDtos = (CategoryProductInputModel[])xmlSerializer.Deserialize
                                        (new StringReader(inputXml));
            var categoriesProducts = new List<CategoryProduct>();

            foreach (var cpDto in categoryProductsDtos)
            {
                var categoryProduct = new CategoryProduct()
                {
                    CategoryId = cpDto.CategoryId,
                    ProductId = cpDto.ProductId

                };

                categoriesProducts.Add(categoryProduct);

            }



            context.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

    }
}
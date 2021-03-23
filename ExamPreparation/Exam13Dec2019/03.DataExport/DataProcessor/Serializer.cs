namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                 .Select(a => new 
              
                 {
                     AuthorName = a.FirstName + " " + a.LastName,
                     Books = a.AuthorsBooks.OrderByDescending(x=>x.Book.Price)
                     .Select(b => new 
                     {
                         BookName = b.Book.Name,
                         BookPrice = b.Book.Price.ToString("f2")
                     }).ToList()
                 }).ToList().OrderByDescending(x => x.Books.Count()).ThenBy(x => x.AuthorName).ToList();

            var json = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return json;

        }

        //Select all authors along with their books. Select their name in format first name + ' ' + last name. 
        //    For each book select its name and price formatted to the second digit after the decimal point. 
        //    Order the books by price in descending order. 
        //    Finally sort all authors by book count descending and then by author full name.

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var sb = new StringBuilder();

            var books = context.Books.Where(b => b.PublishedOn < date && b.Genre==Genre.Science)
                 .ToArray()
                 .OrderByDescending(b=>b.Pages)
                 .ThenByDescending(n=>n.PublishedOn)
                  .Take(10)
                 .Select(b => new ExportOldestBooks
                
                 {
 
                     Name = b.Name,
                     Date = b.PublishedOn.ToString("d",CultureInfo.InvariantCulture),
                     Pages = b.Pages,

                 }).ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<ExportOldestBooks>)
                                                , new XmlRootAttribute("Books"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, books, namespaces);
            }

            return sb.ToString().Trim();


        }
    }
}

//Export top 10 oldest books that are published before the given date and are of type science. 
   // For each book select its name, date (in format "d") and pages.
    //Sort them by pages in descending order and then by date in descending order.
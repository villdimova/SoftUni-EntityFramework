namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            // DbInitializer.ResetDatabase(db);


            Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));

        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {

            var stringDate = DateTime.ParseExact(date, "dd-MM-yyyy",CultureInfo.InvariantCulture);

            var books = context.Books.Where(b => b.ReleaseDate < stringDate)
                .Select(b => new
                {
                    title = b.Title,
                    editionType = b.EditionType,
                    price = b.Price,
                    releaseDate = b.ReleaseDate

                })
                .OrderByDescending(b => b.releaseDate)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.title} - {b.editionType} - ${b.price:f2}");
            }

            return sb.ToString().TrimEnd();


        }

        //Return the title, edition type and price of all books that are released before a given date.
        //The date will be a string in format dd-MM-yyyy.
        //Return all of the rows in a single string, ordered by release date descending.


    }
}

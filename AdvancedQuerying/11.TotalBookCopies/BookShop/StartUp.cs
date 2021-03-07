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

            Console.WriteLine(CountCopiesByAuthor(db));
            

        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {

            var books = context.Authors
                .Select(x => new
                {
                    authorName = x.FirstName + " " + x.LastName,
                    Count = x.Books.Select(x => x.Copies).Sum()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

               

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.authorName} - {b.Count}");
            }

            return sb.ToString().TrimEnd();
           
           

        }

        //Return the total number of book copies for each author. 
        //Order the results descending by total book copies.
       // Return all results in a single string, each on a new line.






    }
}

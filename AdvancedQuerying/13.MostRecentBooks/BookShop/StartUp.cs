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

            Console.WriteLine(GetMostRecentBooks(db));


        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    books = x.CategoryBooks.Select(b => new
                    {
                        name = b.Book.Title,
                        releaseDate = b.Book.ReleaseDate.Value

                    })
                    .OrderByDescending(x => x.releaseDate)
                    .Take(3)
                    .ToList()

                })
                .OrderBy(x => x.category).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var c in categories)
            {
                sb.AppendLine($"--{c.category}");

                foreach (var b in c.books)
                {
                    sb.AppendLine($"{b.name} ({b.releaseDate.Year})");
                }
            }

            return sb.ToString().TrimEnd();



        }

        //Get the most recent books by categories.The categories should be ordered by name alphabetically.
        // Only take the top 3 most recent books from each category - ordered by release date (descending). 
        // Select and print the category name, and for each book – its title and release year.





    }
}

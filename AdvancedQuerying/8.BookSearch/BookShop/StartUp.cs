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


            Console.WriteLine(GetBookTitlesContaining(db, "sK"));

        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {

            input = input.ToLower();

            var books = context.Books.Where(b => b.Title.ToLower().Contains(input))
                .OrderBy(t => t.Title)
                .Select(t => t.Title)
                .ToList();



            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {

                sb.AppendLine(b);
            }

            return sb.ToString().TrimEnd();


        }

        //Return the titles of book, which contain a given string. Ignore casing.
        //Return all titles in a single string, each on a new row, ordered alphabetically.




    }
}

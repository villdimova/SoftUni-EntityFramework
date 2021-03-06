namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            // DbInitializer.ResetDatabase(db);


            Console.WriteLine(GetBooksByCategory(db, "Horror mysTery drma"));

        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {

            var listCategories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower()).ToList();



            var books = context.Books
                
                .Where(b => b.BookCategories
                .Any(c => listCategories.Contains(c.Category.Name.ToLower())))
                .Select(b => new
                {
                    Title = b.Title
                })
                .OrderBy(b => b.Title);
               

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine(b.Title);
            }

            return sb.ToString().TrimEnd();


        }

        //
        //Return in a single string the titles of books by a given list of categories. 
        //The list of categories will be given in a single line separated with one or more spaces. 
        //Ignore casing. Order by title alphabetically.

    }
}

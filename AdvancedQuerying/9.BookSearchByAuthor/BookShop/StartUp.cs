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


            Console.WriteLine(GetBooksByAuthor(db, "R"));

        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {

            input = input.ToLower();

            var books = context.Books
                .Select(b => new
                {
                    title = b.Title,
                    authorFirstName= b.Author.FirstName,
                    authorLastname= b.Author.LastName,
                    bookId= b.BookId
                })
                .Where(b => b.authorLastname.ToLower().StartsWith(input))
                .OrderBy(b => b.bookId)
                .ToList();



            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                if (b.authorFirstName == null)
                {
                    sb.AppendLine($"{b.title} ({b.authorLastname})");
                }
                else
                {
                    sb.AppendLine($"{b.title} ({b.authorFirstName} {b.authorLastname})");
                }
                
            }

            return sb.ToString().TrimEnd();


        }

    // Return all titles of books and their authors’ names for books, 
    // which are written by authors whose last names start with the given string.
    //Return a single string with each title on a new row.Ignore casing.Order by book id ascending.





    }
}

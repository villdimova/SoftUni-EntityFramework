namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            // DbInitializer.ResetDatabase(db);


            Console.WriteLine(GetBooksNotReleasedIn(db,2000));

        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine(b);
            }

            return sb.ToString().TrimEnd();


        }

        //
        //Return in a single string all titles of books that are NOT released on a given year. 
        //Order them by book id ascending.
        
    }
}

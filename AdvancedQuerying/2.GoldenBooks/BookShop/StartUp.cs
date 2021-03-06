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

            
            Console.WriteLine(GetGoldenBooks(db)); 

        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var books = context.Books
            .Where(b => b.AgeRestriction== ageRestriction)
            .Select(b => new
            {
                Name = b.Title

            })
            .OrderBy(b => b.Name)
            .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine(b.Name);
            }


            return sb.ToString().TrimEnd();


        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => (b.EditionType == EditionType.Gold) && b.Copies < 5000)
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

        //Return in a single string titles of the golden edition books that have less than 5000 copies, 
        //    each on a new line.Order them by book id ascending.
    }
}

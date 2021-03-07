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

            Console.WriteLine(CountBooks(db,12));
            

        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {

            var books = context.Books.Where(b => b.Title.Length > lengthCheck).ToList();

            return books.Count;


           
           

        }

        // Return the number of books, which have a title longer than the number given as an input.





    }
}

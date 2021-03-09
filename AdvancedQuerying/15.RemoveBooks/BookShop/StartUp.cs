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
             DbInitializer.ResetDatabase(db);

           var result= RemoveBooks(db);

            Console.WriteLine(result);

        }

        public static int RemoveBooks(BookShopContext context)
        {

            var removedBooks = context.Books.Where(x => x.Copies < 4200);

            var count = removedBooks.Count();


            context.Books.RemoveRange(removedBooks);

            context.SaveChanges();



            return count;



        }

        //Remove all books, which have less than 4200 copies. 
        //Return an int - the number of books that were deleted from the database.




    }
}

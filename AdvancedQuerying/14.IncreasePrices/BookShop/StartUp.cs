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

         IncreasePrices(db);


        }

        public static void IncreasePrices(BookShopContext context)
        {

            var books = context.Books.ToList();

            foreach (var b in books)
            {
                if (b.ReleaseDate.Value.Year<2010)
                {
                    b.Price += 5;
                }
            }

            context.SaveChanges();
            

        }

        //Increase the prices of all books released before 2010 by 5.




    }
}

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

            Console.WriteLine(GetTotalProfitByCategory(db));
            

        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {

            var books = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    totalPrice = x.CategoryBooks.Select(y => y.Book.Copies * y.Book.Price).Sum()


                })
                .OrderByDescending(x=>x.totalPrice).ThenBy(x=>x.category)
                .ToList();

               

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.category} ${b.totalPrice:f2}");
            }

            return sb.ToString().TrimEnd();
           
           

        }

        //Return the total profit of all books by category. 
        //Profit for a book can be calculated by multiplying its number of copies by the price per single book. 
        //Order the results by descending by total profit for category and ascending by category name.






    }
}

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


            Console.WriteLine(GetAuthorNamesEndingIn(db, "a"));

        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {

            var authors = context.Authors.Where(a => a.FirstName.EndsWith(input))
                  .Select(a => new
                  {
                      firstName = a.FirstName,
                      lastName = a.LastName

                  })
                  .OrderBy(a => a.firstName).ThenBy(a => a.lastName)
                  .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in authors)
            {
                if (a.firstName==null)
                {
                    sb.AppendLine(a.lastName);
                }
                else
                {
                    sb.AppendLine($"{a.firstName} {a.lastName}");
                }
                
            }

            return sb.ToString().TrimEnd();


        }

        //Return the full names of authors, whose first name ends with a given string.
        //Return all names in a single string, each on a new row, ordered alphabetically.



    }
}

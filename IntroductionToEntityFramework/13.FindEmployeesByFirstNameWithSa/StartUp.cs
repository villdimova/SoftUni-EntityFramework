using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {

            var context = new SoftUniContext();

            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
        }

        /*
         
        Write a program that finds all employees whose first name starts with "Sa". 
        Return their first, last name, their job title and salary, rounded to 2 symbols 
        after the decimal separator in the format given in the example below. Order them by first name,
        then by last name (ascending).
      
         */

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {

            var employees = context.Employees
                .Where(x => x.FirstName.StartsWith("Sa"))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Job = x.JobTitle,
                    Salary = x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.Job} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

       
    }
}

using SoftUni.Data;
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

            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
        }

        /*

         Extract all employees from the Research and Development department.
        Order them by salary (in ascending order), then by first name (in descending order).
        Return only their first name, last name, department name and salary rounded to 2 symbols,
        after the decimal separator

         */

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context
                .Employees
                .Where(e=>e.Department.Name== "Research and Development")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Department= e.Department.Name,
                    Salary= e.Salary
                })
                .OrderBy(e=>e.Salary).ThenByDescending(e=>e.FirstName)
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    Name = String.Join(" ", e.FirstName, e.LastName, e.MiddleName), 
                    JobTitle=e.JobTitle,
                    Salary=Math.Round(e.Salary,2)

                })
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.Name} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    Name = e.FirstName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Name)
                .ToList();

            foreach (var empl in employees)
            {
                sb.AppendLine($"{empl.Name} - {empl.Salary:f2}");
            }

            return sb.ToString().TrimEnd();

        }
    }
}

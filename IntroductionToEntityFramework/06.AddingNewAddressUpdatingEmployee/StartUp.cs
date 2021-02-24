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

            Console.WriteLine(AddNewAddressToEmployee(context));
        }

        /*

        Create a new address with text "Vitoshka 15" and TownId 4.
        Set that address to the employee with last name "Nakov".
Then order by descending all the employees by their Address’ Id, take 10 rows and from them,
        take the AddressText. Return the results each on a new line:


         */

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(address);

            context.SaveChanges();

            var nakovEmployee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            nakovEmployee.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Select(e => new
                {
                    addressId = e.AddressId,
                    address = e.Address.AddressText
                })
                .OrderByDescending(e => e.addressId)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine(e.address);
            }

            return sb.ToString().TrimEnd();
        }
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

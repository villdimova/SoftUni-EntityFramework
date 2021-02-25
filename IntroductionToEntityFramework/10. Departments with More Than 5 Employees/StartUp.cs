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

            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
        }

        /*
        Find all departments with more than 5 employees. Order them by employee count (ascending),
        then by department name (alphabetically). 
For each department, print the department name and the manager’s first and last name on the first row. 
Then print the first name, the last name and the job title of every employee on a new row. 
Order the employees by first name (ascending), then by last name (ascending).
Format of the output: For each department print it in the format 
 "<DepartmentName> - <ManagerFirstName>  <ManagerLastName>" and for each employee print it 
 in the format "<EmployeeFirstName> <EmployeeFirstName> - <JobTitle>".

         */

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {

            var departments = context.Departments
                .Include(x => x.Employees)
                .Include(x => x.Manager)
                .Where(x=>x.Employees.Count()>5)
                .Select(x => new
                {

                    DepartmentName = x.Name,
                    ManagerFN = x.Manager.FirstName,
                    ManagerLN = x.Manager.LastName,
                    Employees = x.Employees.Select(e => new
                    {
                        EmployeeFN = e.FirstName,
                        EmployeeLN = e.LastName,
                        EmployeeJob = e.JobTitle

                    })
                })
                .OrderBy(e => e.Employees.Count())
                .ThenBy(d => d.DepartmentName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepartmentName} – {d.ManagerFN} {d.ManagerLN}");

                foreach (var e in d.Employees.OrderBy(e=>e.EmployeeFN).ThenBy(e=>e.EmployeeLN))
                {
                    sb.AppendLine($"{e.EmployeeFN} {e.EmployeeLN} - {e.EmployeeJob}");
                }
            }

           return sb.ToString().TrimEnd();


        }
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    Id = x.EmployeeId,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name
                    })

                })
                .FirstOrDefault(x => x.Id == 147);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var p in employee.Projects.OrderBy(x => x.ProjectName))
            {
                sb.AppendLine(p.ProjectName);
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Include(x => x.Employees)
                .ThenInclude(x => x.Address.Town)
                .OrderByDescending(x => x.Employees.Count)
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Select(x => new
                {
                    Address = x.AddressText,
                    TownName = x.Town.Name,
                    CountEmployees = x.Employees.Count()
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.Address}, {a.TownName} - {a.CountEmployees} employees");
            }

            return sb.ToString().TrimEnd();

        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {

            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001
                  && p.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFN = e.Manager.FirstName,
                    ManagerLN = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(p => new
                    {

                        ProjectName = p.Project.Name,
                        ProjectStartDate = p.Project.StartDate,
                        ProjectEndDate = p.Project.EndDate
                    })

                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFN} {e.ManagerLN}");

                foreach (var p in e.Projects)
                {
                    if (p.ProjectEndDate == null)
                    {
                        sb.AppendLine($"--{p.ProjectName} - {p.ProjectStartDate} - {"not finished"}");
                    }
                    else
                    {
                        sb.AppendLine($"--{p.ProjectName} - {p.ProjectStartDate} - {p.ProjectEndDate}");
                    }

                }
            }

            return sb.ToString().TrimEnd();

        }
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
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Department = e.Department.Name,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName)
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
                    JobTitle = e.JobTitle,
                    Salary = Math.Round(e.Salary, 2)

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

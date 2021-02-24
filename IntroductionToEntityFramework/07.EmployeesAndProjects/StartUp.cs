﻿using Microsoft.EntityFrameworkCore;
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

            Console.WriteLine(GetEmployeesInPeriod(context));
        }

        /*

        Find the first 10 employees who have projects started in the period 
        2001 - 2003 (inclusive). Print each employee's first name, last name, manager’s first name
        and last name. 
        Then return all of their projects in the format "--<ProjectName> - <StartDate> - <EndDate>", 
        each on a new row. If a project has no end date, print "not finished" instead.

         */

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
                    LastName= e.LastName,
                    ManagerFN= e.Manager.FirstName,
                    ManagerLN= e.Manager.LastName,
                    Projects= e.EmployeesProjects.Select(p=> new { 
                    
                       ProjectName= p.Project.Name,
                       ProjectStartDate= p.Project.StartDate,
                       ProjectEndDate= p.Project.EndDate
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
                    if (p.ProjectEndDate==null)
                    {
                        sb.AppendLine($"--{p.ProjectName} - {p.ProjectStartDate} - {"not finished"}");
                    }
                    else
                    {
                        sb.AppendLine($"--{p.ProjectName} - {p.ProjectStartDate} - {p.ProjectEndDate}");
                    }
                    
                }
            }

           return  sb.ToString().TrimEnd();

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

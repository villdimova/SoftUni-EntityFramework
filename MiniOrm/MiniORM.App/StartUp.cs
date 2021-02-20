

namespace MiniORM.App
{
    using MiniORM.App.Data.Entities;
    using System;
    using System.Linq;

    public class StartUp
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.;Database=MiniORM;Integrated Security =true";

            SoftUniDbContext context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Tervel",
                LastName = "Dimov",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            });

            Employee employee = context.Employees.Last();
            employee.FirstName = "Aleksander";

            context.SaveChanges();
        }
    }
}

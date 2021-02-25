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

            Console.WriteLine(DeleteProjectById(context));
        }

        /*
         
        Let's delete the project with id 2. Then, 
        take 10 projects and return their names, each on a new line. 
        Remember to restore your database after this task.
        
      
         */

        public static string DeleteProjectById(SoftUniContext context)
        {

            var empl = context.EmployeesProjects
                .Where(p => p.ProjectId == 2)
                .ToList();

            foreach (var e in empl)
            {
                context.EmployeesProjects.Remove(e);
            }
                

            
            context.SaveChanges();

            var project = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            context.Projects.Remove(project);
            context.SaveChanges();

            var projects = context.Projects
              .Select(x => new
              {

                  Name = x.Name
              })
              .Take(10)
              .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().TrimEnd();


        }
      

       
    }
}

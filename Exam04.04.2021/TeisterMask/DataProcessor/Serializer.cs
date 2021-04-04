namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        //Export all projects that have at least one task.For each project, export its name, tasks count, and if it has end(due) date 
        //    which is represented like "Yes" and "No". For each task, export its name and label type.Order the tasks by name (ascending). 
        //        Order the projects by tasks count (descending), then by name (ascending).
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects.Where(p => p.Tasks.Any()).ToArray()
                .Select(x => new ExportProjectsTasks
                {
                    TasksCount = x.Tasks.Count,
                    ProjectName = x.Name,
                    HasEndDate = x.DueDate.HasValue ? "Yes" : "No",
                    Tasks = x.Tasks.Select(t => new ExportTaskDto
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()

                    }).ToArray().OrderBy(y => y.Name).ToArray()
                }).OrderByDescending(t => t.Tasks.Count()).ThenBy(t => t.ProjectName).ToList();



            var xmlSerializer = new XmlSerializer(typeof(List<ExportProjectsTasks>)
                                                , new XmlRootAttribute("Projects"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, projects, namespaces);
            }

            return sb.ToString().Trim();


        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees.ToArray().Where(t => t.EmployeesTasks.Any(x => x.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks.Where(b => b.Task.OpenDate >= date).ToArray()
                    .OrderByDescending(t => t.Task.DueDate)
                    .ThenBy(t => t.Task.Name).Select(c => new
                    {
                        TaskName = c.Task.Name,
                        OpenDate = c.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = c.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = c.Task.LabelType.ToString(),
                        ExecutionType = c.Task.ExecutionType.ToString()

                    })
                }).OrderByDescending(x=>x.Tasks.Count()).ThenBy(x=>x.Username).Take(10).ToList();

            var json = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return json;
        }
    }
}
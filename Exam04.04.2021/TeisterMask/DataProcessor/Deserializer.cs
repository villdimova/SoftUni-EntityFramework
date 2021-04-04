namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(ImportProjectDto[]), new XmlRootAttribute("Projects"));

            var projectsDtos = (ImportProjectDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var projectDto in projectsDtos)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime projectOpenDate;
                bool isProjectODValid = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy",
                                                CultureInfo.InvariantCulture, DateTimeStyles.None, out projectOpenDate);

                DateTime? projectDD;

                DateTime projectDueDate;
                bool isProjectDDateValid = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy",
                                                CultureInfo.InvariantCulture, DateTimeStyles.None, out projectDueDate);

                if (isProjectDDateValid)
                {
                    projectDD = projectDueDate;
                }
                else
                {
                    projectDD = null;
                }


               

                if (!isProjectODValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validProject = new Project
                {
                    Name=projectDto.Name,
                    OpenDate=projectOpenDate,
                    DueDate=projectDD

                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ExecutionType executionType;
                    bool isValidExecutionType = Enum.TryParse(taskDto.ExecutionType, out executionType);

                    if (!isValidExecutionType)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    LabelType labelType;
                    bool isValidLabelType = Enum.TryParse(taskDto.LabelType, out labelType);

                    if (!isValidLabelType)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate;
                    bool isTaskODateValid = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out taskOpenDate);

                    if (!isTaskODateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskDueDate;
                    bool isTaskDDateValid = DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);

                    if (!isTaskDDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate<projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (validProject.DueDate!=null && taskDueDate>projectDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    var validTask = new Task
                    {
                        Name=taskDto.Name,
                        OpenDate=taskOpenDate,
                        DueDate=taskDueDate,
                        ExecutionType=executionType,
                        LabelType=labelType
                    };

                    validProject.Tasks.Add(validTask);

                }

                context.Projects.Add(validProject);
                sb.AppendLine($"Successfully imported project - {validProject.Name} with {validProject.Tasks.Count} tasks.");
            }

            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var employeesDtos = JsonConvert.DeserializeObject<ImportEmployeesDto[]>(jsonString);

            foreach (var emplDto in employeesDtos)
            {
                if (!IsValid(emplDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validEmployee = new Employee
                {
                    Username=emplDto.Username,
                    Email=emplDto.Email,
                    Phone=emplDto.Phone
                };

                var validTasksIds = new List<int>();
                var invalidTasksIds = new List<int>();
                foreach (var taskId in emplDto.Tasks)
                {

                    var currentTask = context.Tasks.FirstOrDefault(t => t.Id == taskId);

                    if (currentTask==null)
                    {
                        if (!invalidTasksIds.Contains(taskId))
                        {
                            sb.AppendLine(ErrorMessage);
                            invalidTasksIds.Add(taskId);
                        }
                       
                        continue;
                    }

                    if (validTasksIds.Contains(taskId))
                    {
                        continue;
                    }

                    validTasksIds.Add(taskId);
                    var employeeTask = new EmployeeTask
                    {

                        Task=currentTask,
                        Employee=validEmployee
                    };

                    validEmployee.EmployeesTasks.Add(employeeTask);
                    
                }

                context.Employees.Add(validEmployee);
                sb.AppendLine($"Successfully imported employee - {validEmployee.Username} with {validEmployee.EmployeesTasks.Count} tasks.");
            }

            context.SaveChanges();
            return sb.ToString().Trim();
           
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
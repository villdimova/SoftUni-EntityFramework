namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {

            var sb = new StringBuilder();
            var departmentCellsDtos = JsonConvert.DeserializeObject<ImportDepartmentsCellDto[]>(jsonString);
            var departments = new List<Department>();

            foreach (var dcDto in departmentCellsDtos)
            {
                if (!IsValid(dcDto) || !dcDto.Cells.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dcDto.Cells.Length==0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department department = new Department
                {
                    Name = dcDto.Name,
                };

                foreach (var cellDto in dcDto.Cells)
                {
                    Cell cell = new Cell
                    {
                        CellNumber =cellDto.CellNumber,
                        HasWindow= cellDto.HasWindow,
                        
                    };

                    department.Cells.Add(cell);
                }

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var prisonersMailsDtos = JsonConvert.DeserializeObject<ImportPrisonersMailsDto[]>(jsonString);
            var prisoners = new List<Prisoner>();

            foreach (var pmDto in prisonersMailsDtos)
            {
                if (!IsValid(pmDto) || !pmDto.Mails.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime incarcerationDate;

                bool isIDateValid = DateTime.TryParseExact(pmDto.IncarcerationDate, "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);

                if (!isIDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime releaseDate;

                bool isRDateValid = DateTime.TryParseExact(pmDto.ReleaseDate, "dd/MM/yyyy",
                                              CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                //if (!isRDateValid)
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                var validPrisoner = new Prisoner
                {
                    FullName = pmDto.FullName,
                    Nickname = pmDto.Nickname,
                    Age = pmDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail= pmDto.Bail,
                    CellId=pmDto.CellId

                };

                foreach (var mailDto in pmDto.Mails)
                {
                    var validMail = new Mail
                    {
                        Description= mailDto.Description,
                        Sender=mailDto.Sender,
                        Address= mailDto.Address
                    };

                    validPrisoner.Mails.Add(validMail);
                }

                prisoners.Add(validPrisoner);
                sb.AppendLine($"Imported {validPrisoner.FullName} {validPrisoner.Age} years old");
            }

            context.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(ImportOfficersPrisonersDto[]), new XmlRootAttribute("Officers"));

            var officers = new List<Officer>();

            var officersPrisonersDtos = (ImportOfficersPrisonersDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var opDto in officersPrisonersDtos)
            {
                if (!IsValid(opDto) )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Weapon enumWeapon;
                bool isValidWeapon = Enum.TryParse(opDto.Weapon, out enumWeapon);

                if (!isValidWeapon)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Position enumPosition;
                bool isValidPosition = Enum.TryParse(opDto.Position, out enumPosition);

                if (!isValidPosition)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validOfficer = new Officer
                {
                    FullName=opDto.Name,
                    Salary=opDto.Money,
                    Position=enumPosition,
                    Weapon=enumWeapon,
                    DepartmentId=opDto.DepartmentId

                };

                foreach (var prisonerDto in opDto.Prisoners)
                {
                    var prisoner = context.Prisoners.FirstOrDefault(p => p.Id == prisonerDto.Id);

                    var officerPrisoner = new OfficerPrisoner
                    {
                        Prisoner=prisoner,
                        Officer=validOfficer,
                    };

                    validOfficer.OfficerPrisoners.Add(officerPrisoner);
                }

                officers.Add(validOfficer);

                sb.AppendLine($"Imported {validOfficer.FullName} ({ validOfficer.OfficerPrisoners.Count} prisoners)");
               

            }

            context.Officers.AddRange(officers);
            context.SaveChanges();
            return sb.ToString().Trim();

        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}
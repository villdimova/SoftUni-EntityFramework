
namespace MiniORM.App.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public ICollection<EmployeeProject> EmployeesProjects { get;  }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MiniORM.App.Data.Entities
{
  public  class EmployeeProject
    {
        [ForeignKey(nameof(Employee))]
        [Key]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(Project))]
        [Key]
        public int ProjectId { get; set; }

        public Employee Employee { get; set; }

        public Project Project { get; set; }
    }
}

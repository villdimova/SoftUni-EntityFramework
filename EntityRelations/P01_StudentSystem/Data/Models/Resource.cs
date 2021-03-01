
namespace P01_StudentSystem.Data.Models
{
    using P01_StudentSystem.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Resource
    {
        public int ResourceId { get; set; }

        [Required]
        [MaxLength(DataValidation.ResourceName)]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }

        public virtual Course Course { get; set; }

        [Required]
        public int CourseId { get; set; }



    }
}

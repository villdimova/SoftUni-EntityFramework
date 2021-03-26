using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportCardDto
    {

        [RegularExpression("^\\d{4} \\d{4} \\d{4} \\d{4}$")]
        [Required]
        public string Number { get; set; }

        [RegularExpression("^\\d{3}$")]
        [Required]
        public string CVC { get; set; }

        [Required]
      
        public string Type { get; set; }
    }
}

//"Number": "4902 6975 5076 5316",
//        "CVC": "091",
//        "Type": "Debit"
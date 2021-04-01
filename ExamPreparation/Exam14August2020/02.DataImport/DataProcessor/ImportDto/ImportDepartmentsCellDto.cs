using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentsCellDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        public ImportCellDto[] Cells { get; set; }
    }

    public class ImportCellDto
    {
        [Range(1,1000)]
        [Required]
        public int CellNumber  { get; set; }

        public bool HasWindow { get; set; }
    }
}

//"Name": "",
//    "Cells": [
//      {
//        "CellNumber": 101,
//        "HasWindow": true
//      },
//      {
//    "CellNumber": 102,
//        "HasWindow": false
//      }
//    ]

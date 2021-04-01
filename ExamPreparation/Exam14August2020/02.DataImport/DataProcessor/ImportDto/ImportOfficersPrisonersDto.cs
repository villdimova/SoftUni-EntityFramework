using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
  public  class ImportOfficersPrisonersDto
    {
        [MinLength(3)]
        [MaxLength(30)]
        [Required]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        [XmlElement("Money")]
        public decimal Money { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

     
        public PrisonerDto[] Prisoners { get; set; }

    }

    [XmlType("Prisoner")]
    public class PrisonerDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}

//< Officers >
//  < Officer >
//    < Name > Minerva Kitchingman </ Name >
//       < Money > 2582 </ Money >
//       < Position > Invalid </ Position >
//       < Weapon > ChainRifle </ Weapon >
//       < DepartmentId > 2 </ DepartmentId >
//       < Prisoners >
//         < Prisoner id = "15" />
//        </ Prisoners >

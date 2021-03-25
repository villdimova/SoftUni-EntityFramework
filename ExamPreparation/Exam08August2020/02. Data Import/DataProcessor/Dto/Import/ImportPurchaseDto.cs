﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [Required]
        [XmlAttribute("title")]
        public string Title { get; set; }

        [Required]
        [XmlElement("Type")]
        public string Type { get; set; }

        [Required]
        [RegularExpression("^([A-Z]*[0-9]*){4}-([A-Z]*[0-9]*){4}-([A-Z]*[0-9]*){4}$")]
        [XmlElement("Key")]
        public string Key { get; set; }

        [Required]
        [RegularExpression("^\\d{4} \\d{4} \\d{4} \\d{4}$")]
        [XmlElement("Card")]
        public string Card { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }
    }
}

////< Purchases >
////  < Purchase title = "Dungeon Warfare 2" >
////     < Type > Digital </ Type >
////     < Key > ZTZ3 - 0D2S - G4TJ </ Key >
////    < Card > 1833 5024 0553 6211 </ Card >
////     < Date > 07 / 12 / 2016 05:49 </ Date >
////         </ Purchase >


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class ExportOldestBooks
    {
      
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlAttribute("Pages")]
        public int Pages { get; set; }
    }
}

//<? xml version = "1.0" encoding = "utf-16" ?>
//   < Books >
//     < Book Pages = "4881" >
//       < Name > Sierra Marsh Fern</Name>
//          <Date>03/18/2016</Date>
//  </Book>


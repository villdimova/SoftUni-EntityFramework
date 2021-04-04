using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
   public  class ExportProjectsTasks
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray]
        public ExportTaskDto[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class ExportTaskDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Label")]
        public string Label { get; set; }
    }
}

//< Projects >
//  < Project TasksCount = "10" >
//     < ProjectName > Hyster - Yale </ ProjectName >
//     < HasEndDate > No </ HasEndDate >
//     < Tasks >
//       < Task >
//         < Name > Broadleaf </ Name >
//         < Label > JavaAdvanced </ Label >
//       </ Task >
//       < Task >
//         < Name > Bryum </ Name >
//         < Label > EntityFramework </ Label >

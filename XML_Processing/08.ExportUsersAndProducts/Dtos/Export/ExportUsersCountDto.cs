﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{

    public class ExportUsersCountDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public ExportUserDto[] Users { get; set; }
    }

    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public ExportSoldProductsDto SoldProducts { get; set; }
    }

    public class ExportSoldProductsDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ExportProductsDto[] Products { get; set; }
    }

    [XmlType("Product")]
    public class ExportProductsDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}

//< Users >
//  < count > 54 </ count >
//  < users >
//    < User >
//      < firstName > Cathee </ firstName >
//      < lastName > Rallings </ lastName >
//      < age > 33 </ age >
      //< SoldProducts >
      //  < count > 9 </ count >
      //  < products >
      //    < Product >
      //      < name > Fair Foundation SPF 15</name>
      //      <price>1394.24</price>
      //    </Product>



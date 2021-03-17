using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();

            
            var xml = File.ReadAllText("./Datasets/parts.xml");
            Console.WriteLine(ImportParts(db,xml));

        }

        public static string ImportParts(CarDealerContext context, string inputXml)
       {
            var xmlSerializer = new XmlSerializer(typeof(PartInputModel[]),
                                                 new XmlRootAttribute("Parts"));

            var partsDtos = (PartInputModel[])xmlSerializer
                              .Deserialize(new StringReader(inputXml));
            var validSupplierIds = context.Suppliers.Select(a=>a.Id).ToList();
            var parts = new List<Part>();

            foreach (var partDto in partsDtos.Where(x=> validSupplierIds.Contains(x.SupplierId)))
            {
                var part = new Part
                {
                   Name=partDto.Name,
                   Price=partDto.Price,
                   Quantity=partDto.Quantity,
                   SupplierId=partDto.SupplierId
                };

                parts.Add(part);

            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";

        }
    }
}
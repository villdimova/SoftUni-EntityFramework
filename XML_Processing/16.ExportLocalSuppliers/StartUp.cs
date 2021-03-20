using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();

            Console.WriteLine(GetLocalSuppliers(db));

        }


        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var suppliers = context.Suppliers.Where(s => !s.IsImporter)
                .Select(x => new ExportLocalSuppliers
                {
                    Id = x.Id,
                    Name=x.Name,
                    PartsCount=x.Parts.Count
                }).ToList();
                

            var xmlSerializer = new XmlSerializer(typeof(List<ExportLocalSuppliers>)
                                                , new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, suppliers, namespaces);
            }

            return sb.ToString().Trim();

        }

    }
}
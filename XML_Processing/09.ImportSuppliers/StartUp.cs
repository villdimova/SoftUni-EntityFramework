using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();

            
            var xml = File.ReadAllText("./Datasets/suppliers.xml");
            Console.WriteLine(ImportSuppliers(db,xml));

        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
       {
            var xmlSerializer = new XmlSerializer(typeof(SupplierInputModel[]),
                                                 new XmlRootAttribute("Suppliers"));

            var supplierDtos = (SupplierInputModel[])xmlSerializer
                              .Deserialize(new StringReader(inputXml));

            var suppliers = new List<Supplier>();

            foreach (var supplierDto in supplierDtos)
            {
                var supplier = new Supplier
                {
                    Name = supplierDto.Name,
                    IsImporter = supplierDto.IsImporter
                };

                suppliers.Add(supplier);

            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";

        }
    }
}
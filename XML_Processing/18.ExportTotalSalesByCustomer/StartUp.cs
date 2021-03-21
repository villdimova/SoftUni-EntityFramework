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

            Console.WriteLine(GetTotalSalesByCustomer(db));

        }


        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(c => new ExportCustomers
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Select(x=>x.Car)
                                        .SelectMany(y=>y.PartCars)
                                        .Sum(z=>z.Part.Price)
                })
                .OrderByDescending(m => m.SpentMoney)
                .ToList();
               
                

            var xmlSerializer = new XmlSerializer(typeof(List<ExportCustomers>)
                                                , new XmlRootAttribute("customers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString().Trim();

        }

    }
}
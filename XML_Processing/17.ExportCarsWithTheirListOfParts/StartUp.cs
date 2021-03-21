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

            Console.WriteLine(GetCarsWithTheirListOfParts(db));

        }


        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var cars = context.Cars
                .Select(c => new ExportCarsWithListParts
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(x => new ExportCarParts
                    {
                        Name = x.Part.Name,
                        Price = x.Part.Price

                    }).OrderByDescending(p => p.Price)
                      .ToArray()

                })
                .OrderByDescending(a => a.TravelledDistance)
                .ThenBy(a => a.Model)
                .Take(5)
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<ExportCarsWithListParts>)
                                                , new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, cars, namespaces);
            }

            return sb.ToString().Trim();

        }

    }
}
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

            Console.WriteLine(GetCarsFromMakeBmw(db));

        }


        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var cars = context.Cars.Where(c => c.Make== "BMW")
                .Select(x => new ExportBMWCars
                {
                    Id=x.Id,
                    Model=x.Model,
                    TravelledDistance=x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x=>x.TravelledDistance).ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<ExportBMWCars>)
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
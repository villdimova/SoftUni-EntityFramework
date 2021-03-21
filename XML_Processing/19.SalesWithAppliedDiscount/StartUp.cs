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

            Console.WriteLine(GetSalesWithAppliedDiscount(db));

        }


        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var sales = context.Sales
                 .Select(s => new ExportSalesWithDiscount
                 {
                     Car=new ExportSoldCarInfo 
                     { 
                        Make=s.Car.Make,
                        Model=s.Car.Model,
                        TravelledDistance=s.Car.TravelledDistance
                     },
                     Discount=s.Discount,
                     CustomerName=s.Customer.Name,
                     Price=s.Car.PartCars.Sum(x=>x.Part.Price),
                     PriceWithDiscount= s.Car.PartCars.Sum(x => x.Part.Price)-
                     s.Car.PartCars.Sum(x => x.Part.Price)*s.Discount/100m
                 })
                 .ToList();

               
                

            var xmlSerializer = new XmlSerializer(typeof(List<ExportSalesWithDiscount>)
                                                , new XmlRootAttribute("sales"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");


            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, sales, namespaces);
            }

            return sb.ToString().Trim();

        }

    }
}
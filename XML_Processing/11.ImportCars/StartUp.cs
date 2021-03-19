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

            
            var xml = File.ReadAllText("./Datasets/cars.xml");
            Console.WriteLine(ImportCars(db,xml));

        }

        public static string ImportCars(CarDealerContext context, string inputXml)
       {
            var xmlSerializer = new XmlSerializer(typeof(CarInputModel[]),
                                                 new XmlRootAttribute("Cars"));

            var carsDtos = (CarInputModel[])xmlSerializer
                              .Deserialize(new StringReader(inputXml));
           
            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDto in carsDtos.Distinct())
            {
                var car = new Car
                {
                   Make=carDto.Make,
                   Model= carDto.Model,
                   TravelledDistance=carDto.TraveledDistance,
                  
                };
                var parts = carDto.Parts
                    .Where(pDto => context.Parts.Any(p => p.Id == pDto.partId))
                    .Select(p => p.partId)
                    .Distinct();

                foreach (var partId in parts)
                {
                    var carPart = new PartCar
                    {
                        PartId = partId,
                        Car=car
                    };

                    partCars.Add(carPart);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partCars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";

        }
    }
}
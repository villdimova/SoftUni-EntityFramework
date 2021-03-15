using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();


            //db.Database.EnsureDeleted();
            // db.Database.EnsureCreated();

            var inputJson = File.ReadAllText("./../../../Datasets/cars.json");

           Console.WriteLine(ImportCars(db, inputJson));

            //var inputJson = File.ReadAllText("./../../../Datasets/parts.json");

           // Console.WriteLine(ImportParts(db, inputJson));


        }


        public static string ImportCars(CarDealerContext context, string inputJson)

        {
            var carsDto = JsonConvert.DeserializeObject<IEnumerable<CarInputModel>>(inputJson);
            var listCars = new List<Car>();

            foreach (var car in carsDto)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var partId in car?.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });

                    
                }

                listCars.Add(currentCar);
            }

            context.Cars.AddRange(listCars);
            context.SaveChanges();

            return $"Successfully imported {listCars.Count}.";
        }   

    }
}
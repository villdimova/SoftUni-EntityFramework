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
           //db.Database.EnsureCreated();

           

            Console.WriteLine(GetCarsFromMakeToyota(db));

        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {

            var carsToyota = context.Cars.Where(c => c.Make == "Toyota")

                 .Select(z => new
                 {
                     Id = z.Id,
                     Make = z.Make,
                     Model= z.Model,
                     TravelledDistance = z.TravelledDistance
                 })
                 .OrderBy(x => x.Model)
                 .ThenByDescending(y => y.TravelledDistance)
                 .ToList();

            var json = JsonConvert.SerializeObject(carsToyota, Formatting.Indented);

            return json;
        }

        //Get all cars from make Toyota and order them by model alphabetically and by 
        //travelled distance descending. 
        //Export the list of cars to JSON in the format provided below.

    }
}
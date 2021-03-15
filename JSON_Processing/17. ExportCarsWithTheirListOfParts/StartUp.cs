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

           

            Console.WriteLine(GetCarsWithTheirListOfParts(db));

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new
                {
                    car= new 
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance
                    },
                    
                    parts = x.PartCars.Select(p => new
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price.ToString("f2")

                    })
                }).ToList();
                

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return json;
            
        }

        //Get all cars along with their list of parts. For the car get only make,
       // model and travelled distance and for the parts get only name and price 
        //(formatted to 2nd digit after the decimal point).
        //Export the list of cars and their parts to JSON in the format provided below.
    }
}
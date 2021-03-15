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

           

            Console.WriteLine(GetSalesWithAppliedDiscount(db));

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {

            var sales = context
                .Sales
                
                .Select(a => new
                {
                    car = new
                    {
                        Make = a.Car.Make,
                        Model = a.Car.Model,
                        TravelledDistance= a.Car.TravelledDistance,
                       
                    },

                    customerName= a.Customer.Name,
                    Discount= a.Discount.ToString("F2"),
                    price= a.Car.PartCars.Sum(b=>b.Part.Price).ToString("F2"),
                    priceWithDiscount = (a.Car.PartCars.
                        Sum(b => b.Part.Price)*(100-a.Discount)/100).ToString("F2")
                })
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return json;
            
        }

       // Get first 10 sales with information about the car,
       // customer and price of the sale with and without discount.
       // Export the list of sales to JSON in the format provided below.
    }
}
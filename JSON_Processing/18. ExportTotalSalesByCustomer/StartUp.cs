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

           

            Console.WriteLine(GetTotalSalesByCustomer(db));

        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {

            var customers = context.Customers.Where(c => c.Sales.Count >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Sum(x => x.Car.PartCars.Sum(y => y.Part.Price))
                })
                .OrderByDescending(x=>x.spentMoney).ThenByDescending(y=>y.boughtCars)
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;
            
        }

        //Get all customers that have bought at least 1 car and get their names, 
        //bought cars count and total spent money on cars.Order the result list by total 
        //spent money descending then by total bought cars again in descending order.
        //Export the list of customers to JSON in the format provided below.
    }
}
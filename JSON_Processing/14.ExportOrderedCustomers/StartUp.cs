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

           

            Console.WriteLine(GetOrderedCustomers(db));

        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {

            var customers = context.Customers.
                OrderBy(x => x.BirthDate).ThenBy(y => y.IsYoungDriver)
                .Select(c=>new 
                {
                    Name= c.Name,
                    BirthDate= c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver= c.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(customers,Formatting.Indented);

            return json;
        }

        //Get all customers ordered by their birth date ascending.
       //If two customers are born on the same date first print those who are not young drivers
       //(e.g.print experienced drivers first).
       //Export the list of customers to JSON in the format provided below.

    }
}
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

           

            Console.WriteLine(GetLocalSuppliers(db));

        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers.Where(s => s.IsImporter == false)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();

            var json = JsonConvert.SerializeObject(suppliers,Formatting.Indented);
            return json;
            
        }

        //Get all suppliers that do not import parts from abroad.Get their id,
       // name and the number of parts they can offer to supply.
       // Export the list of suppliers to JSON in the format provided below.
    }
}
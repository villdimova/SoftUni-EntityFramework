using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
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

            var inputJson = File.ReadAllText("./../../../Datasets/suppliers.json");

            Console.WriteLine(ImportSuppliers(db,inputJson));

        }


        public static string ImportSuppliers(CarDealerContext context, string inputJson)

        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";


        }
    }
}
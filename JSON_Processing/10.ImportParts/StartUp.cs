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

            var inputJson = File.ReadAllText("./../../../Datasets/parts.json");

            Console.WriteLine(ImportParts(db,inputJson));

        }


        public static string ImportParts(CarDealerContext context, string inputJson)

        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var suppliersId = context.Suppliers.Select(a => a.Id).ToArray();

            var validParts = parts.Where(x => suppliersId.Contains(x.SupplierId));

            context.Parts.AddRange(validParts);

            context.SaveChanges();

            return $"Successfully imported {validParts.Count()}.";


        }
    }
}
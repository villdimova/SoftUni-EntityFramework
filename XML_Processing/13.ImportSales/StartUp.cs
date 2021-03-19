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

            
            var xml = File.ReadAllText("./Datasets/sales.xml");
            Console.WriteLine(ImportSales(db,xml));

        }


        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(SalesInputModel[]),
                                                 new XmlRootAttribute("Sales"));

            var salesDtos = (SalesInputModel[])xmlSerializer
                              .Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();
            var validSales = salesDtos.Where(c => context.Cars.Select(a => a.Id)
                                                     .Contains(c.CarId))
                                                       .ToList();
            
            foreach (var saleDto in validSales)
            {
                var sale = new Sale
                {
                    CarId = saleDto.CarId,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";

        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
       {
            var xmlSerializer = new XmlSerializer(typeof(CustomerInputModel[]),
                                                 new XmlRootAttribute("Customers"));

            var customersDtos = (CustomerInputModel[])xmlSerializer
                              .Deserialize(new StringReader(inputXml));
           
            var customers = new List<Customer>();
            

            foreach (var customerDto in customersDtos)
            {
                var customer = new Customer
                {
                    Name= customerDto.Name,
                    BirthDate=customerDto.BirthDate,
                    IsYoungDriver=customerDto.IsYoungDriver
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";

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
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance,

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
                        Car = car
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
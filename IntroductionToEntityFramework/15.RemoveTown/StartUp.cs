using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {

            var context = new SoftUniContext();

            Console.WriteLine(RemoveTown(context));
        }

        /*
         
      Write a program that deletes a town with name „Seattle”. 
        Also, delete all addresses that are in those towns. 
        Return the number of addresses that were deleted in format “{count} addresses in
        Seattle were deleted”. There will be employees living at those addresses, 
        which will be a problem when trying to delete the addresses. 
        So, start by setting the AddressId of each employee for the given address to null. 
        After all of them are set to null, you may safely remove all the addresses from the context.
        Addresses and finally remove the given town.
        
      
         */

        public static string RemoveTown(SoftUniContext context)
        {

            var addresses = context.Addresses
                .Where(x => x.Town.Name == "Seattle")
                .ToList();

            var count = addresses.Count();

            var employees = context.Employees
                .Where(x => x.Address.Town.Name == "Seattle");
               
            foreach (var e in employees)
            {
                e.AddressId = null;

            }
            context.SaveChanges();
            context.Addresses.RemoveRange(addresses);
            
            context.SaveChanges();

            var town = context.Towns.FirstOrDefault(x => x.Name == "Seattle");

            context.Towns.Remove(town);
            context.SaveChanges();



            return $"{count} addresses in Seattle were deleted";


        }
      

       
    }
}

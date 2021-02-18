using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _07.PrintAllVillainsNames
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server =.; Database = MinionsDB; Integrated Security = true";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var minionsQuery = @"SELECT Name FROM Minions";

            using var selectCommand = new SqlCommand(minionsQuery, connection);
            
            using var reader = selectCommand.ExecuteReader();

            List<string> names = new List<string>();
            {
                while (reader.Read())
                {

                    names.Add($"{reader["Name"]}");
                }
            }

            int minionsCount = names.Count;

            
            if (names.Count%2==0)
            {
                for (int i = 0; i < names.Count / 2; i++)
                {

                    Console.WriteLine(names[i]);
                    Console.WriteLine(names[names.Count-1 - i]);
                }
            }

            else
            {
                for (int i = 0; i < names.Count / 2; i++)
                {

                    Console.WriteLine(names[i]);
                    Console.WriteLine(names[names.Count -1- i]);
                }

                Console.WriteLine(names[names.Count/2]);
            }
            

        }
    }
}

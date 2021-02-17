using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _03._MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server =.; Database = MinionsDB; Integrated Security = true";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            int id = int.Parse(Console.ReadLine());

        
            string villainNameQuery = "SELECT Name FROM Villains WHERE Id = @Id";

            using var command = new SqlCommand(villainNameQuery, connection);
            command.Parameters.AddWithValue("@Id", id);
            var name = command.ExecuteScalar();

            string countVillainsQuery = @"SELECT  COUNT(*) AS Count
                                                         FROM Villains v
                                                         JOIN MinionsVillains mv ON mv.VillainId = v.Id
                                                         where v.id = @Id";

            using var commandCountMinions = new SqlCommand(countVillainsQuery, connection);
            commandCountMinions.Parameters.AddWithValue("@Id", id);
            var countMinions = (int)commandCountMinions.ExecuteScalar();

            string namesMinionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                             m.Name, 
                                                             m.Age
                                                        FROM MinionsVillains AS mv
                                                        JOIN Minions As m ON mv.MinionId = m.Id
                                                       WHERE mv.VillainId = @Id
                                                    ORDER BY m.Name;";


            using var commandsNameMinions = new SqlCommand(namesMinionsQuery,connection);
            commandsNameMinions.Parameters.AddWithValue("@Id", id);
          
           
                if (name == null)
                {
                    Console.WriteLine($"No villain with ID {id} exists in the database.");
                }

                else if (countMinions == 0)
                {
                    Console.WriteLine($" Villain: {name}");
                    Console.WriteLine("(no minions)");

                }

                else
                {
                    Console.WriteLine($" Villain: {name}");

                using (SqlDataReader reader = commandsNameMinions.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var counter = 1;
                        Console.WriteLine($"{reader["RowNum"]}. {reader["Name"]} {reader["Age"]}");
                        
                    }

                }
            }
        }

        
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace _08.IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server =.; Database = MinionsDB; Integrated Security = true";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            int[] minionsIndexes = Console.ReadLine().Split().Select(int.Parse).ToArray();

            string updateMinions = @" UPDATE Minions
                                                   SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                    WHERE Id = @Id";
            
            foreach (var index in minionsIndexes)
            {
                using var command = new SqlCommand(updateMinions, connection);
                command.Parameters.AddWithValue("@Id", index);
                command.ExecuteNonQuery();
            }

            string selectMinionsQuery = @"SELECT Name, Age FROM Minions";

            var selectMinionsCommand = new SqlCommand(selectMinionsQuery, connection);

            using var reader = selectMinionsCommand.ExecuteReader();
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{ reader["Name"]} {reader["Age"]}");
                }
            }
           
        }
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeTownNamesCasing
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server =.; Database = MinionsDB; Integrated Security = true";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string countryName = Console.ReadLine();

            string changeTNamesQuery = @"UPDATE Towns
                                                       SET Name = UPPER(Name)
                                                     WHERE CountryCode = (SELECT c.Id FROM Countries AS c 
                                                WHERE c.Name = @countryName)";
            using var command = new SqlCommand(changeTNamesQuery,connection);
            command.Parameters.AddWithValue("@countryName", countryName);
           int numChangedTowns= command.ExecuteNonQuery();

            if (numChangedTowns==0)
            {
                Console.WriteLine("No town names were affected.");
            }

            else
            {
                Console.WriteLine($"{numChangedTowns} town names were affected. ");

                string selectQuery = @"SELECT t.Name 
                                               FROM Towns as t
                                            JOIN Countries AS c ON c.Id = t.CountryCode
                                            WHERE c.Name = @countryName";

                using var selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@countryName", countryName);
                using var reader = selectCommand.ExecuteReader();

                List<string> names = new List<string>();
                {
                    while (reader.Read())
                    {

                        names.Add($"{reader["Name"]}");
                    }
                }

                Console.Write("[" + (String.Join(", ", names)) + "]");


            }

            
        }
    }
}

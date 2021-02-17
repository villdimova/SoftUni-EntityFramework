using Microsoft.Data.SqlClient;
using System;

namespace _02.VillainNames
{
    class Program
    {
        static void Main(string[] args)

            
        {
            const string connectionString = "Server =.; Database = MinionsDB; Integrated Security = true";

            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string selectVillainNames = "SELECT v.Name, COUNT(*) AS Count" +
                                                        " FROM Villains v" +
                                                       " JOIN MinionsVillains mv ON mv.VillainId = v.Id" +
                                                        " GROUP BY  v.Id,v.Name" +
                                                        " HAVING COUNT(mv.MinionId) > 3";

            SqlCommand villainNames = new SqlCommand(selectVillainNames, sqlConnection);

            using (SqlDataReader dataReader = villainNames.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Console.WriteLine($"{dataReader["Name"]} - {dataReader["Count"]}");
                }
            
            }

            

        }
    }
}

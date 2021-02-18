using Microsoft.Data.SqlClient;
using System;

namespace RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server =.; Database = MinionsDB; Integrated Security = true";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            int villainId = int.Parse(Console.ReadLine());

            string villainNameQuery = @"SELECT Name FROM Villains WHERE Id = @villainId";

            using var villainNameCommand =new  SqlCommand(villainNameQuery, connection);

            villainNameCommand.Parameters.AddWithValue("@villainId", villainId);
            var villainName = villainNameCommand.ExecuteScalar();

            if (villainName==null)
            {
                Console.WriteLine("No such villain was found.");
            }

            else
            {
                string countVillainMinions = @"SELECT COUNT(*)
                                                                FROM MinionsVillains
                                                                WHERE villainId=@villainId";
                using var countVillainMinionsCommand = new SqlCommand(countVillainMinions, connection);
                countVillainMinionsCommand.Parameters.AddWithValue("@villainId", villainId);
                int villainCount = (int)countVillainMinionsCommand.ExecuteScalar();

                string deleteMinionsQuery = @"DELETE FROM MinionsVillains 
                                                                WHERE VillainId = @villainId";
                using var deleteMinionsCommand = new SqlCommand(deleteMinionsQuery,connection);
                deleteMinionsCommand.Parameters.AddWithValue("@villainId", villainId);
                deleteMinionsCommand.ExecuteNonQuery();

                string deleteVillianQuery = @"DELETE FROM Villains
                                                            WHERE Id = @villainId";
                using var deleteVillianCommand = new SqlCommand(deleteVillianQuery, connection);
                deleteVillianCommand.Parameters.AddWithValue("@villainId", villainId);
                deleteVillianCommand.ExecuteNonQuery();

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{villainCount} minions were released.");

              
            }



        }
    }
}

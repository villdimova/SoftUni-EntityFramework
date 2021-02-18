using Microsoft.Data.SqlClient;
using System;

namespace _04.AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server =.; Database = MinionsDB; Integrated Security = true";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            //Minion: Bob 14 Berlin
            string[] minionInfo = Console.ReadLine().Split(" ");

            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string town = minionInfo[3];

            string[] villainInfo = Console.ReadLine().Split(" ");
            string villainName = villainInfo[1];
            
            object townId=GetTownId(connection, town);

            if (townId == null)
            {
                string createTownQuery = @"INSERT INTO Towns (Name) VALUES (@townName)";
                using var insertTownCommand = new SqlCommand(createTownQuery, connection);
                insertTownCommand.Parameters.AddWithValue("@townName", town);
                insertTownCommand.ExecuteNonQuery();
                Console.WriteLine($"Town {town} was added to the database.");

                townId= GetTownId(connection, town);
            }


            var villainId = GetVillainId(connection, villainName);

            if (villainId == null)
            {
                string createVillainQuery = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                using var insertVillainCommand = new SqlCommand(createVillainQuery, connection);
                insertVillainCommand.Parameters.AddWithValue("@villainName", villainName);
                insertVillainCommand.ExecuteNonQuery();

                Console.WriteLine($"Villain {villainName} was added to the database.");

                villainId = GetVillainId(connection, villainName);
            }



            string insertMinionQuery = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";
            using var insertMinionCommand = new SqlCommand(insertMinionQuery, connection);
            insertMinionCommand.Parameters.AddWithValue("@nam", minionName);
            insertMinionCommand.Parameters.AddWithValue("@age", minionAge);
            insertMinionCommand.Parameters.AddWithValue("@townId", townId);
            insertMinionCommand.ExecuteNonQuery();

            int minionId = (int)GetMinionId(connection, minionName);

            string insertMinionVillainsQuery = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES 
                                                                (@villainId, @minionId)";
            using var insrtMinionVillainsCommand= new SqlCommand(insertMinionVillainsQuery, connection);
            insrtMinionVillainsCommand.Parameters.AddWithValue("@villainId", villainId);
            insrtMinionVillainsCommand.Parameters.AddWithValue("@minionId", minionId);
            insrtMinionVillainsCommand.ExecuteNonQuery();

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");




        }

        private static Object GetTownId(SqlConnection connection, string town)
        {
            string townIdQuery = "SELECT Id FROM Towns WHERE Name = @townName";
           var  townIidCommand = new SqlCommand(townIdQuery, connection);
                  townIidCommand.Parameters.AddWithValue("@townName", town);
           var  townId = townIidCommand.ExecuteScalar();
            return townId;
        }

        private static Object GetVillainId(SqlConnection connection, string villainName)
        {
            string villainExistingQuery = "SELECT Id FROM Villains WHERE Name = @Name";
            var villainIdCommand = new SqlCommand(villainExistingQuery, connection);
            villainIdCommand.Parameters.AddWithValue("@Name", villainName);
            var villainId = villainIdCommand.ExecuteScalar();
            return villainId;
        }

        private static Object GetMinionId(SqlConnection connection,string minionName)
        {
            string minionIdQuery = "SELECT Id FROM Minions WHERE Name = @Name";
            var minionIdCommand = new SqlCommand(minionIdQuery, connection);
            minionIdCommand.Parameters.AddWithValue("@Name", minionName);
            var minionId = minionIdCommand.ExecuteScalar();
            return minionId;
        }
    }
}

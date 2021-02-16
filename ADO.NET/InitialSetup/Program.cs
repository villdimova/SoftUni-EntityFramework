using Microsoft.Data.SqlClient;
using System;

namespace InitialSetup
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connecting to SSMS
            using SqlConnection sqlConnection = new SqlConnection("Server=;Integrated Security=true; ");
            sqlConnection.Open();

            //Creating MinionsDB
            using SqlCommand sqlCommand = new SqlCommand("CREATE DATABASE MinionsDB", sqlConnection);
            sqlCommand.ExecuteNonQuery();

            //Connectiong to Minions

            using SqlConnection minionsConnection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");
            minionsConnection.Open();


            //Creating Countries Table
            string queryCreateCoutries = @"CREATE TABLE Countries
                                 (Id INT PRIMARY KEY IDENTITY,
                                  Name VARCHAR(20) NOT NULL)";
            using SqlCommand createCoutriesTb = new SqlCommand(queryCreateCoutries, minionsConnection);
            createCoutriesTb.ExecuteNonQuery();

            //Creating Towns Table
            string queryCreateTowns = @"CREATE TABLE Towns
                            (Id INT PRIMARY KEY IDENTITY,
	                            Name VARCHAR(20) NOT NULL,
	                            CountruCode INT NOT NULL REFERENCES Countries(Id) )";
            using SqlCommand createTownsTb = new SqlCommand(queryCreateTowns, minionsConnection);
            createTownsTb.ExecuteNonQuery();

            //Creating EvilnessFactors Table
            string queryCreateEvilnessFactors = @"CREATE TABLE 	EvilnessFactors
                                                                ( Id INT PRIMARY KEY IDENTITY,
	                                                                Name VARCHAR(20) NOT NULL  )";
            using SqlCommand createEvilnessFactorsTb = new SqlCommand(queryCreateEvilnessFactors, minionsConnection);
            createEvilnessFactorsTb.ExecuteNonQuery();


            //Creating Villains Table
            string queryCreateVillains = @"CREATE TABLE Villains
                                                        (Id INT PRIMARY KEY IDENTITY,
	                                                     Name VARCHAR(20) NOT NULL,
	                                                     EvilnessFactorId INT NOT NULL REFERENCES EvilnessFactors(Id))";
            using SqlCommand createVillainsTb = new SqlCommand(queryCreateVillains, minionsConnection);
            createVillainsTb.ExecuteNonQuery();

            //Creating Minions Table
            string queryCreateMinions = @"CREATE TABLE Minions
                                                         (Id INT PRIMARY KEY IDENTITY,
	                                                     Name VARCHAR(20) NOT NULL,
	                                                     Age INT NOT NULL,
	                                                     TownId INT NOT NULL REFERENCES Towns(Id))";
            using SqlCommand createMinionsTb = new SqlCommand(queryCreateMinions, minionsConnection);
            createMinionsTb.ExecuteNonQuery();

            //Creating MinionsVillains Table
            string queryCreateMinionsVillains = @"CREATE TABLE MinionsVillains
                                                                     (MinionId INT NOT NULL REFERENCES Minions(Id),
	                                                                  VillainId INT NOT NULL REFERENCES Villains(Id)
	                                                                  CONSTRAINT pk_MinionsVillains PRIMARY KEY(MinionId,VillainId))";
            using SqlCommand createMinionsVillainsTb = new SqlCommand(queryCreateMinionsVillains, minionsConnection);
            createMinionsVillainsTb.ExecuteNonQuery();


        }
    }
}

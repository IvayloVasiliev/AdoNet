using System;
using System.Data.SqlClient;

namespace AddMinion04
{
    public class StartUp
    {
        private static string connectionString = "Server=DESKTOP-4Q4QNM5\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            string[] minionInfo = Console.ReadLine().Split();
            string[] villianInfo = Console.ReadLine().Split();

            string minionName = minionInfo[1];
            int age = int.Parse(minionInfo[2]);
            string town = minionInfo[3];

            string villianName = villianInfo[1];

            using (connection)
            {
                connection.Open();

               int? townId = GetTownByName(town, connection);

                if (townId == null)
                {
                    AddTown(connection, town);
                }

                townId = GetTownByName(town, connection);

                AddMinion(connection, minionName, age, townId);

                int? villianId = GetVillianByName(connection, villianName);

                if (villianId == null)
                {
                    AddVillian(connection, villianName);
                }
                villianId = GetVillianByName(connection, villianName);

                int minionId = GetMinionByName(connection, minionName);

                AddMinionVillian(connection, villianId, minionId, minionName, villianName);

            }
        }

        private static void AddMinionVillian(SqlConnection connection, int? villianId, int minionId, string minionName, string villianName)
        {
            string insertMinionVillian = "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(@villainId, @minionId)";

            using (SqlCommand command = new SqlCommand(insertMinionVillian, connection))
            {
                command.Parameters.AddWithValue("@villainId", villianId);
                command.Parameters.AddWithValue("@minionId", minionId);
                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");

        }

        private static int GetMinionByName(SqlConnection connection, string minionName)
        {
            string minionQuery = "SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(minionQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", minionName);
                return (int)command.ExecuteScalar();
            }
        }

        private static void AddVillian(SqlConnection connection, string villianName)
        {
            string insertVillian = "INSERT INTO Villains(Name, EvilnessFactorId)  VALUES(@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillian, connection))
            {
                command.Parameters.AddWithValue("@villainName", villianName);
                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Villain {villianName} was added to the database.");
        }

        private static int? GetVillianByName(SqlConnection connection, string villianName)
        {
            string villianIdQuery = "SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(villianIdQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", villianName);

                return (int?)command.ExecuteScalar();
            }

        }

        private static void AddMinion(SqlConnection connection, string minionName, int age, int? townId)
        {
             string insertMinion = "INSERT INTO Minions(Name, Age, TownId) VALUES(@nam, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinion, connection))
            {
                command.Parameters.AddWithValue("@nam", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }
        }

        private static int? GetTownByName(string town, SqlConnection connection)
        {
            string townIdQuery = "SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(townIdQuery, connection))
            {
                command.Parameters.AddWithValue("@townName", town);

               return (int?)command.ExecuteScalar();

            }
        }

        private static void AddTown(SqlConnection connection, string town)
        {
            string insertTownSql = "INSERT INTO Towns(Name) VALUES(@townName)";

            using (SqlCommand cmd = new SqlCommand(insertTownSql, connection))
            {
                cmd.Parameters.AddWithValue("@townName", town);
                cmd.ExecuteNonQuery(); 
            }

            Console.WriteLine($"Town {town} was added to the database.");
        }
    }
}






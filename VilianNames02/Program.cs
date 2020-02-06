using System;
using System.Data.SqlClient;

namespace VilianNames02
{
    class Program
    {
        private static string connectionString = "Server=DESKTOP-4Q4QNM5\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            connection.Open();

            using (connection)
            {
                string sqlText = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                    FROM Villains AS v 
                                    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                    GROUP BY v.Id, v.Name 
                                    HAVING COUNT(mv.VillainId) > 3 
                                    ORDER BY COUNT(mv.VillainId)";

                SqlCommand cmd = new SqlCommand(sqlText, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                    }
                }

            }

        }
    }
}

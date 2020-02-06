using System;
using System.Data.SqlClient;

namespace MinionNames03
{
    class Program
    {
        private static string connectionString = "Server=DESKTOP-4Q4QNM5\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            connection.Open();

            using (connection)
            {
                string sqlText = @"SELECT Name FROM Villains WHERE Id = @Id

                                   SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                SqlCommand cmd = new SqlCommand(sqlText, connection);
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Villian: {reader["Name"]}");
                        Console.WriteLine($"{reader["m.Name"]} {reader["m.Age"]}");
                    }
                }

            }




        }
    }
}

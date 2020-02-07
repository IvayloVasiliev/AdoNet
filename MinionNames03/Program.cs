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

                string villianNameQuery = @"SELECT Name FROM Villains WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(villianNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    string villianName = (string)command.ExecuteScalar();

                    if (villianName == null)
                    {
                        Console.WriteLine($"No villain with ID {id} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villian: {villianName}");
                }

                

                string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                

                using (SqlCommand command = new SqlCommand(minionsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //long rowNumber = (long)reader[0];
                            //string name = (string)reader[1];
                            int age = (int)reader[2];

                            Console.WriteLine($"{reader[0]}. {reader["Name"]} {age}");
                        }

                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions )");
                        }
                    }
                }

            }
        }
    }
}

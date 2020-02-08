using System;
using System.Data;
using System.Data.SqlClient;

namespace IncreaseAgeStoredProcedure09
{
    public class StartUp
    {
        private static string connectionString = "Server=DESKTOP-4Q4QNM5\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using (connection)
            {
                connection.Open();

                string getOlderProcedure = @"EXEC usp_GetOlder @id";

                using (SqlCommand command = new SqlCommand(getOlderProcedure, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} - {reader[1]} years old");
                        }
                    }
                }
            }




        }
    }
}

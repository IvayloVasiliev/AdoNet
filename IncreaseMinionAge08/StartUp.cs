using System;
using System.Data.SqlClient;
using System.Linq;

namespace IncreaseMinionAge08
{
    public class StartUp
    {
        private static string connectionString = "Server=DESKTOP-4Q4QNM5\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            int[] ids = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using (connection)
            {
                connection.Open();

                string updateMinions = @" UPDATE Minions
                                SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                WHERE Id = @Id";

                for (int i = 0; i < ids.Length; i++)
                {
                    using (SqlCommand command = new SqlCommand(updateMinions, connection))
                    {
                        command.Parameters.AddWithValue("@Id", ids[i]);
                        command.ExecuteNonQuery();
                    }
                }

                string minionsQuery = "SELECT Name, Age FROM Minions";

                using (SqlCommand command = new SqlCommand(minionsQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} {reader[1]}");
                        }
                    }
                }
            }
        }
    }
}

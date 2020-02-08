using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ChangeTownNamesCasing05
{
    public class StartUp
    {
        private static string connectionString = "Server=DESKTOP-4Q4QNM5\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            string country = Console.ReadLine();

            connection.Open();

            using (connection)
            {
                string updateTownNames = @"UPDATE Towns
                       SET Name = UPPER(Name)
                       WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (SqlCommand command = new SqlCommand(updateTownNames, connection))
                {
                    command.Parameters.AddWithValue("@countryName", country);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} town names were affected.");

                }

                string townNameQuery = @"SELECT t.Name 
                                        FROM Towns as t
                                        JOIN Countries AS c ON c.Id = t.CountryCode
                                        WHERE c.Name = @countryName";

                List<string> towns = new List<string>();

                using (SqlCommand command = new SqlCommand(townNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@countryName", country);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            towns.Add((string)reader[0]);
                        }
                    }
                }

                Console.WriteLine("[" + string.Join(", ", towns) +"]");
            }



        }
    }
}

﻿using System;
using System.Data.SqlClient;

namespace RemoveVillian06
{
    class Program
    {
        private static string connectionString = "Server=DESKTOP-4Q4QNM5\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        private static SqlTransaction transaction;

        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            connection.Open();

            using (connection)
            {
                transaction = connection.BeginTransaction();

                try
                {

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = "SELECT Name FROM Villains WHERE Id = @villainId";
                command.Parameters.AddWithValue("@villainId", id);

                object value = command.ExecuteScalar();

                if (value == null)
                {
                    throw new ArgumentException( "No such villain was found.");
                }

                    string villianName = (string)value;

                    command.CommandText = @"DELETE FROM MinionsVillains 
                                                WHERE VillainId = @villainId";

                    int miniosDeleted = command.ExecuteNonQuery();

                    command.CommandText = @"DELETE FROM Villains
                                                WHERE Id = @villainId";

                    command.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine($"{villianName} was deleted.");
                    Console.WriteLine($"{miniosDeleted} minions were released.");
                }
                catch (ArgumentException ane)
                {
                    try
                    {
                        Console.WriteLine(ane.Message);
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                catch (Exception a)
                {
                    try
                    {
                        Console.WriteLine(a.Message);
                        transaction.Rollback();
                    }
                    catch (Exception re)
                    {
                        Console.WriteLine(re.Message);
                    }
                }
            }
        }
    }
}

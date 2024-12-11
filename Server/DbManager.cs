using System;
using System.Collections.Generic;
using Google.Protobuf.Compiler;
using MySql.Data.MySqlClient;

namespace Server
{
    public class DbManager
    {
        private readonly string connectionStr = "server=localhost;user=root;database=4BetCapitalDB;password=;";
        private MySqlConnection conn;

        public DbManager()
        {
            try
            {
                conn = new MySqlConnection(connectionStr);
                conn.Open(); // Opening the connection
                Console.WriteLine("Connection Opened");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error opening connection to MySQL: " + ex.Message);
                // Optionally log the exception
            }
        }

        public List<Person> GetUsersFromDB()
        {
            List<Person> people = new List<Person>();

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                string sql = "SELECT UserName, UserEmail, UserPassword, Balance FROM userstable";
                MySqlCommand command = new MySqlCommand(sql, conn);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) 
                    {
                        string userName = reader["UserName"].ToString();
                        string userEmail = reader["UserEmail"].ToString();
                        string userPassword = reader["UserPassword"].ToString();
                        string userBalance = reader["Balance"].ToString();

                        Person person = new Person() { UserName = userName, Email = userEmail, Password = userPassword, Balance = Convert.ToInt32(userBalance) };  // Create a Person object
                        people.Add(person); 
                    }
                }
                else
                {
                    Console.WriteLine("No data found.");
                }

                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error executing query: " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }

            return people;  
        }

        public bool PostPersonToDb(Person person)
        {
            List<Person> registredUsers = GetUsersFromDB();

            if (registredUsers.Any(u => u.UserName == person.UserName || u.Email == person.Email))
                return false;

            string sql = "INSERT INTO userstable (UserName, UserEmail, UserPassword, Balance) VALUES (@UserName, @UserEmail, @UserPassword, @Balance)";
            MySqlCommand command = new MySqlCommand(sql, conn);

            command.Parameters.AddWithValue("@UserName", person.UserName);
            command.Parameters.AddWithValue("@UserEmail", person.Email);
            command.Parameters.AddWithValue("@UserPassword", person.Password);
            command.Parameters.AddWithValue("@Balance", person.Balance);

            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open(); // Open the connection if it's not already open
            }

            int rowsAffected = command.ExecuteNonQuery(); // Execute the query

            if (rowsAffected > 0)
            {
                Console.WriteLine("Person added successfully.");
                conn.Close();
                return true; // Success
            }
            else
            {
                Console.WriteLine("Failed to add person.");
                conn.Close();
                return false; // Failure
            }

        }

        public bool PostSportEventToDB(SportEvent sportEvent)
        {
            string sql = "INSERT INTO sporteventstable (NameTeam1, NameTeam2, DateStart, DateEnd, CoefficientsWinTeam1, CoefficientsWinTeam2, CoefficientDraw) " +
                "VALUES (@NameTeam1, @NameTeam2, @DateStart, @DateEnd, @CoefficientsWinTeam1, @CoefficientsWinTeam2, @CoefficientDraw)";
            MySqlCommand command = new MySqlCommand(sql, conn);

            command.Parameters.AddWithValue("@NameTeam1", sportEvent.Team1Name);
            command.Parameters.AddWithValue("@NameTeam2", sportEvent.Team2Name);
            command.Parameters.AddWithValue("@DateStart", sportEvent.DateStart);
            command.Parameters.AddWithValue("@DateEnd", sportEvent.DateEnd);
            command.Parameters.AddWithValue("@CoefficientsWinTeam1", sportEvent.CoefficientsTeam1Win);
            command.Parameters.AddWithValue("@CoefficientsWinTeam2", sportEvent.CoefficientsTeam2Win);
            command.Parameters.AddWithValue("@CoefficientDraw", sportEvent.CoefficientsDraw);

            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Event added successfully.");
                conn.Close();
                return true; // Success
            }
            else
            {
                Console.WriteLine("Failed to add person.");
                conn.Close();
                return false; // Failure
            }
        }
    }
}

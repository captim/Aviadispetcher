using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Aviadispetcher
{
    class DBConnection
    {
        private static DBConnection db;
        private string connStr;

        private DBConnection() { }

        public static DBConnection GetInstance()
        {
            if (db == null)
            {
                db = new DBConnection();
            }
            db.Connection();
            return db;
        }

        private void Connection()
        {
            connStr = "Server = localhost; Database = mytest; Uid = root; Pwd = ;";
        }

        public FlightList GetAllFlights()
        {
            string commandString = "SELECT * FROM rozklad;";
            MySqlCommand command = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connStr);
            command.CommandText = commandString;
            command.Connection = conn;
            MySqlDataReader reader;
            command.Connection.Open();
            reader = command.ExecuteReader();
            int i = 0;
            FlightList flightList = new FlightList();
            while (reader.Read())
            {
                flightList.Add(new Flight((int)reader["id"], (string)reader["number"], (string)reader["city"],
                    (System.TimeSpan)reader["depature_time"], (int)reader["free_seats"]));
                i += 1;
            }
            reader.Close();
            return flightList;
        }
        public void Add(Flight flight)
        {
            MySqlCommand command = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connStr);
            command = new MySqlCommand("INSERT INTO rozklad (`number`, `city`, `depature_time`, `free_seats`) VALUES (?, ?, ?, ?)", conn);
            command.Parameters.Add("@number", MySqlDbType.VarChar, 6).Value = flight.number;
            command.Parameters.Add("@city", MySqlDbType.VarChar, 25).Value = flight.city;
            command.Parameters.Add("@departure_time", MySqlDbType.Time).Value = flight.depature_time;
            command.Parameters.Add("@free_seats", MySqlDbType.Int16, 4).Value = flight.free_seats;
            conn.Open();
            command.ExecuteNonQuery();
        }

        public int GetMaxId()
        {
            MySqlCommand command = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connStr);
            string commandString = "SELECT MAX(ID) ID FROM rozklad;";
            command.CommandText = commandString;
            command.Connection = conn;
            MySqlDataReader reader;
            command.Connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["id"];
        }

        public void Update(Flight flight)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand cmd = new MySqlCommand("UPDATE rozklad SET number = ?, city = ?, depature_time = ?, free_seats = ? WHERE id = ?", conn);
            cmd.Parameters.Add("@number", MySqlDbType.VarChar, 6).Value = flight.number;
            cmd.Parameters.Add("@city", MySqlDbType.VarChar, 25).Value = flight.city;
            cmd.Parameters.Add("@departure_time", MySqlDbType.Time).Value = flight.depature_time;
            cmd.Parameters.Add("@free_seats", MySqlDbType.Int16, 4).Value = flight.free_seats;
            cmd.Parameters.Add("@id", MySqlDbType.Int32, 11).Value = flight.id;
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

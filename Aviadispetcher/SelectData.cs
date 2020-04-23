using System;
using System.Collections.Generic;
using System.Text;

namespace Aviadispetcher
{
    static class SelectData
    {
        public static List<Flight> SelectX(FlightList flightList, string city)
        {
            List<Flight> selectedFlights = new List<Flight>();
            foreach (Flight flight in flightList.Flights_list)
            {
                if (flight.city.Equals(city))
                {
                    selectedFlights.Add(flight);
                }
            }
            return selectedFlights;
        }
        public static List<Flight> SelectXY(FlightList flightList, string city, TimeSpan deptime)
        {
            List<Flight> selectedFlights = new List<Flight>();
            foreach (Flight flight in flightList.Flights_list)
            {
                if (flight.city.Equals(city) && (flight.depature_time.Equals(deptime) || flight.depature_time.CompareTo(deptime) == -1))
                {
                    selectedFlights.Add(flight);
                }
            }
            return selectedFlights;
        }
    }
}

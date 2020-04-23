using System;
using System.Collections.Generic;

namespace Aviadispetcher
{
    /// <summary>
    /// клас записів про рейс
    /// </summary>
    public class Flight
    {
        public Flight(int idNum, string nF, string cF, System.TimeSpan tF, int fS)
        {
            this.id = idNum;
            this.number = nF;
            this.city = cF;
            this.depature_time = tF;
            this.free_seats = fS;
        }
        public int id { get; set; }
        public string number { get; set; }
        public string city { get; set; }
        public System.TimeSpan depature_time { get; set; }
        public int free_seats { get; set; }
    }

    public class FlightList
    {
        public List<Flight> Flights_list { get; set; }
        public const int MAX_AMOUNT = 85;
        public FlightList()
        {
            Flights_list = new List<Flight>();
        }
        public void Add(Flight flight)
        {
            if (Flights_list.Count == MAX_AMOUNT)
            {
                throw new OverflowException("Number of flights is limited. Max number of flights equals 85.");
            }
            Flights_list.Add(flight);
        }
        public void Delete(int id)
        {
            if (Flights_list.Count <= id)
            {
                throw new IndexOutOfRangeException("Number of flights is " + Flights_list.Count + " but you want to delete " + id + ".");
            }
            Flights_list.RemoveAt(id);
        }
        public void Add(Flight flight, int id)
        {
            if (Flights_list.Count <= id)
            {
                throw new IndexOutOfRangeException("Number of flights is " + Flights_list.Count + " but you want to add " + id + ".");
            }
            Flights_list.Insert(id, flight);
        }
        public override bool Equals(object obj)
        {
            FlightList list = (FlightList)obj;
            if (list.Flights_list.Count != this.Flights_list.Count)
            {
                return false;
            }
            for(int i = 0; i < list.Flights_list.Count; i++)
            {
                if (!list.Flights_list[i].Equals(this.Flights_list[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
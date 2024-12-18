using System;
using System.Collections.Generic;

namespace KingICT_FlightSearchEngine.Models
{
    public class FlightOffer
    {
        public string Id { get; set; } 
        public List<Itinerary> Itineraries { get; set; }
        public Price Price { get; set; }
    }

    public class Itinerary
    {
        public List<Segment> Segments { get; set; }
    }

    public class Segment
    {
        public Departure Departure { get; set; }
        public Arrival Arrival { get; set; }
        public int NumberOfStops { get; set; }
    }

    public class Departure
    {
        public string IataCode { get; set; }
        public DateTime At { get; set; } 
    }

    public class Arrival
    {
        public string IataCode { get; set; }
        public DateTime At { get; set; } 
    }

    public class Price
    {
        public string Currency { get; set; }
        public string Total { get; set; }
    }

    public class FlightSearchResult
    {
        public List<FlightOffer> Data { get; set; }
    }
}
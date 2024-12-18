namespace KingICT_FlightSearchEngine.Models
{
    public class FlightSearchRequest
    {
        public string OriginLocationCode { get; set; }
        public string DestinationLocationCode { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Adults { get; set; }
        public string CurrencyCode { get; set; }
    }

}

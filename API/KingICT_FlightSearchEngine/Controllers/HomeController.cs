using KingICT_FlightSearchEngine.Models;
using KingICT_FlightSearchEngine.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class HomeController : Controller
{
    private readonly TravelAPI _travelAPI;

    public HomeController(TravelAPI travelAPI)
    {
        _travelAPI = travelAPI;
    }

    [HttpPost("search-flights")]
    public async Task<IActionResult> SearchFlights([FromBody] FlightSearchRequest searchRequest)
    {
    
        //One way flight - if returnDate is empty
        if (string.IsNullOrEmpty(searchRequest.ReturnDate))
        {
            
            var oneWayFlightSearchResults = await _travelAPI.GetFlightSearchResult(
                searchRequest.OriginLocationCode,
                searchRequest.DestinationLocationCode,
                searchRequest.DepartureDate,
                null,  
                searchRequest.Adults,
                searchRequest.CurrencyCode
            );

            if (oneWayFlightSearchResults == null || !oneWayFlightSearchResults.Any())
            {
                return Json(new { message = "No one-way flight offers available.", data = new List<FlightOffer>() });
            }

            return Json(new { data = oneWayFlightSearchResults });
        }
        else
        {
         
            var roundTripFlightSearchResults = await _travelAPI.GetFlightSearchResult(
                searchRequest.OriginLocationCode,
                searchRequest.DestinationLocationCode,
                searchRequest.DepartureDate,
                searchRequest.ReturnDate,  
                searchRequest.Adults,
                searchRequest.CurrencyCode
            );

            if (roundTripFlightSearchResults == null || !roundTripFlightSearchResults.Any())
            {
                return Json(new { message = "No round-trip flight offers available.", data = new List<FlightOffer>() });
            }

            return Json(new { data = roundTripFlightSearchResults });
        }
    }
}


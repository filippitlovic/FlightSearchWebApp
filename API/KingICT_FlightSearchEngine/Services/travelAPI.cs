using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using KingICT_FlightSearchEngine.Models;

namespace KingICT_FlightSearchEngine.Services
{
    public class TravelAPI
    {
        private string apiKey;
        private string apiSecret;
        private string bearerToken;
        private HttpClient http;

        public TravelAPI(IConfiguration config, IHttpClientFactory httpFactory)
        {
            apiKey = config.GetValue<string>("AmadeusAPI:APIKey");
            apiSecret = config.GetValue<string>("AmadeusAPI:APISecret");
            http = httpFactory.CreateClient("TravelAPI"); 
        } 

        public async Task ConnectOAuth()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token");
            message.Content = new StringContent(
                $"grant_type=client_credentials&client_id={apiKey}&client_secret={apiSecret}",
                Encoding.UTF8, "application/x-www-form-urlencoded"
            );


            try
            {
                var results = await http.SendAsync(message);

                if (!results.IsSuccessStatusCode)
                {
                    var errorResponse = await results.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response: {errorResponse}");
                    return;
                }

                await using var stream = await results.Content.ReadAsStreamAsync();
                var oauthResults = await JsonSerializer.DeserializeAsync<OAuthResults>(stream);

                if (oauthResults?.access_token != null)
                {
                    bearerToken = oauthResults.access_token;
                    Console.WriteLine($"Received OAuth token: {bearerToken}");
                }
                else
                {
                    Console.WriteLine("OAuth token not received or is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OAuth connection: {ex.Message}");
            }
        }
        private class OAuthResults
        {
            public string access_token { get; set; }
        }

        public async Task<List<FlightOffer>> GetFlightSearchResult(string originLocationCode, string destinationLocationCode, string departureDate, string returnDate, int adults, string currencyCode)
        {
            if (string.IsNullOrEmpty(bearerToken))
            {
                await ConnectOAuth();
            }

            ConfigBearerTokenHeader();

            
            string url;
            if (string.IsNullOrEmpty(returnDate))
            {
                // One flight - no returnDate as param
                url = $"/v2/shopping/flight-offers?originLocationCode={originLocationCode}&destinationLocationCode={destinationLocationCode}&departureDate={departureDate}&adults={adults}&currencyCode={currencyCode}";
            }
            else
            {
                // return flight
                url = $"/v2/shopping/flight-offers?originLocationCode={originLocationCode}&destinationLocationCode={destinationLocationCode}&departureDate={departureDate}&returnDate={returnDate}&adults={adults}&currencyCode={currencyCode}";
            }

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            try
            {
                var response = await http.SendAsync(message);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();

                // json to model
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var flightSearchResult = await JsonSerializer.DeserializeAsync<FlightSearchResult>(stream, options);

                if (flightSearchResult?.Data == null || !flightSearchResult.Data.Any())
                {
                    Console.WriteLine("No flight offers found.");
                }
                else
                {
                    Console.WriteLine("Found flight offers.");
                }

                return flightSearchResult?.Data ?? new List<FlightOffer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching flight data: {ex.Message}");
                return new List<FlightOffer>();
            }
        }


        private void ConfigBearerTokenHeader() {
            if (string.IsNullOrEmpty(bearerToken))
            {
                throw new InvalidOperationException("Bearer token is null or empty.");
            }
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("Authorization",$"Bearer {bearerToken}");
        }
    }
}

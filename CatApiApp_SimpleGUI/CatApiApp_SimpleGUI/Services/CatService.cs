// Services/CatService.cs

// This file defines the ICatService interface and its implementation, CatService, 
// which provides methods for fetching cat images and facts, as well as retrieving image data as byte arrays.

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CatApiApp_SimpleGUI.Services
{
    // Interface that defines methods for fetching cat images and facts.
    public interface ICatService
    {
        // Asynchronously retrieves a random cat image URL.
        Task<string> GetCatImageAsync();

        // Asynchronously retrieves a random cat fact.
        Task<string> GetCatFactAsync();

        // Asynchronously fetches the image data as a byte array from a given image URL.
        Task<byte[]> GetImageAsByteArrayAsync(string imageUrl);
    }

    // Implementation of the ICatService interface, using HttpClient to fetch data from external APIs.
    public class CatService : ICatService
    {
        private readonly HttpClient _httpClient;

        // Constructor that initializes the HttpClient used for making API requests.
        public CatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Asynchronously retrieves a random cat image URL from an external API.
        public async Task<string> GetCatImageAsync()
        {
            string catImageUrl = "https://api.thecatapi.com/v1/images/search";
            var response = await _httpClient.GetAsync(catImageUrl);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            JArray json = JArray.Parse(content);
            return json[0]["url"]?.ToString() ?? string.Empty; // Extracts and returns the URL of the cat image.
        }

        // Asynchronously retrieves a random cat fact from an external API.
        public async Task<string> GetCatFactAsync()
        {
            string catFactUrl = "https://catfact.ninja/fact";
            var response = await _httpClient.GetAsync(catFactUrl);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            return json["fact"]?.ToString() ?? "No fact available."; // Extracts and returns the fact.
        }

        // Asynchronously fetches image data from the given image URL as a byte array.
        public async Task<byte[]> GetImageAsByteArrayAsync(string imageUrl)
        {
            var response = await _httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync(); // Returns the image as a byte array.
        }
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using CatApiApp_SimpleGUI.Models; // Import the CatData model

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

            try
            {
                // Deserialize JSON array into a list of CatData objects
                var catImages = JsonSerializer.Deserialize<List<CatData>>(content);

                // Loop through each item and return the first valid ImageUrl
                if (catImages != null)
                {
                    foreach (var catData in catImages)
                    {
                        if (!string.IsNullOrEmpty(catData.ImageUrl))
                        {
                            return catData.ImageUrl; // Return the first valid image URL
                        }
                    }
                    // If catImages is not null but contains no valid ImageUrl
                    return "No valid image URL found in the response.";
                }
                else
                {
                    // If catImages is null
                    return "Failed to retrieve cat image data.";
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
            }

            // Return an empty string if no valid image URL is found
            return string.Empty;
        }

        // Asynchronously retrieves a random cat fact from an external API.
        public async Task<string> GetCatFactAsync()
        {
            string catFactUrl = "https://catfact.ninja/fact";
            var response = await _httpClient.GetAsync(catFactUrl);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            try
            {
                // Deserialize JSON content directly into a CatData object (not a list)
                var catData = JsonSerializer.Deserialize<CatData>(content);

                // Check if the fact is available and return it, otherwise return a fallback message
                if (catData != null && !string.IsNullOrEmpty(catData.Fact))
                {
                    return catData.Fact;
                }
                else
                {
                    return "No valid fact found in the response.";
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
            }

            // Return an empty string if deserialization fails or no valid fact is found
            return string.Empty;
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

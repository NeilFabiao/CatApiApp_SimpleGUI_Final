//Models/CatData.cs

// This file defines the CatData model, which represents the structure of data related to cats, 
//including image URLs, facts, user names, and timestamps.
using System;
using System.Text.Json.Serialization;

namespace CatApiApp_SimpleGUI.Models
{
    // This class represents the data model for cat-related information.
    public class CatData
    {   
        // Property to store the URL of a cat image.
        // The default value is set to an empty string as the remainder of the string data types.
        [JsonPropertyName("url")]
        public string ImageUrl { get; set; } = string.Empty;

        // Property to store fact about the cat.
        [JsonPropertyName("fact")]
        public string Fact { get; set; } = string.Empty;

        // Property to store the name of the user who interacted with the data.
        public string UserName { get; set; } = string.Empty;

        // Property to store the date and time when this data was created or updated.
        public DateTime Timestamp { get; set; }
    }
}
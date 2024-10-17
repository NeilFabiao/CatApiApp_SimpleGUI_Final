// ViewModels/MainWindowViewModel.cs

// This file defines the MainWindowViewModel, which manages the user interface logic 
// and data for the main window of the CatApiApp. It interacts with services to fetch cat data, 
// store user interactions, and update the displayed information.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CatApiApp_SimpleGUI.Models;
using CatApiApp_SimpleGUI.Services;
using System.Net.Http; // Required for handling HTTP-related exceptions

namespace CatApiApp_SimpleGUI.ViewModels
{
    // ViewModel class responsible for managing data and operations related to the main window.
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ICatService _catService;  // Injected service for fetching cat data
        private readonly IFileService _fileService;  // Injected service for file operations
        private string _userName;  // Stores the username of the current session
        private ObservableCollection<CatData> _catDataList;  // List to hold cat data
        private string _catFact = string.Empty;  // Stores the latest cat fact
        private readonly string _filePath = "CatData.json"; // File path for storing cat data

        // Collection of cat data, used to display information in the user interface.
        public ObservableCollection<CatData> CatDataList
        {
            get => _catDataList;
            set => this.RaiseAndSetIfChanged(ref _catDataList, value, nameof(CatDataList)); 
        }

        // Property for storing and updating the latest cat fact.
        public string CatFact
        {
            get => _catFact;
            set => this.RaiseAndSetIfChanged(ref _catFact, value, nameof(CatFact));
        }

        // Constructor that injects ICatService and IFileService dependencies.
        public MainWindowViewModel(ICatService catService, IFileService fileService, bool addDummyData = true)
        {
            _catService = catService;
            _fileService = fileService;
            _userName = GenerateRandomUserName();
            _catDataList = new ObservableCollection<CatData>();

            // Populate the list with some initial dummy data for display only if requested.
            if (addDummyData)
            {
                AddDummyData();
            }
        }

        // Generates a random username for the session using a GUID.
        private string GenerateRandomUserName()
        {
            return "User" + Guid.NewGuid().ToString().Substring(0, 8);
        }

        // Adds dummy cat data entries to the observable collection for testing and demonstration.
        private void AddDummyData()
        {
            _catDataList.Add(new CatData
            {
                ImageUrl = "https://cdn2.thecatapi.com/images/e9m.jpg",
                Fact = "Unlike humans, cats do not need to blink their eyes on a regular basis to keep their eyes lubricated.",
                UserName = _userName,
                Timestamp = DateTime.UtcNow
            });

            _catDataList.Add(new CatData
            {
                ImageUrl = "https://cdn2.thecatapi.com/images/IFXsxmXLm.jpg",
                Fact = "A cat's smell is their strongest sense, and they rely on this leading sense to identify people and objects.",
                UserName = _userName,
                Timestamp = DateTime.UtcNow
            });

            _catDataList.Add(new CatData
            {
                ImageUrl = "https://cdn2.thecatapi.com/images/adg.jpg",
                Fact = "The cat appears to be the only domestic companion animal not mentioned in the Bible.",
                UserName = _userName,
                Timestamp = DateTime.UtcNow
            });
        }

        // Fetches a random cat image and fact asynchronously using the injected cat service.
        public async Task<(string imageUrl, string catFact)> FetchCatDataAsync()
        {
            CatFact = "Fetching data..."; // Update the UI to indicate data is being fetched.
            try
            {
                var catImage = await _catService.GetCatImageAsync();  // Fetches a cat image URL.
                var catFact = await _catService.GetCatFactAsync();    // Fetches a cat fact.

                // Handle empty response.
                if (string.IsNullOrEmpty(catFact))
                {
                    catFact = "No fact available.";
                }

                var newCatData = new CatData
                {
                    ImageUrl = catImage,
                    Fact = catFact,
                    UserName = _userName,
                    Timestamp = DateTime.UtcNow
                };

                UpdateCatList(newCatData); // Update the UI with the new cat data.

                return (catImage, catFact);
            }
            catch (HttpRequestException ex)
            {
                // Displays an error message if there's a problem with the HTTP request.
                CatFact = $"Whoa there! It looks like we hit the limit for fetching cat data. Please try again in a few moments.\n\n" +
                $"If the problem persists, please check your network connection or try again later. " +
                $"Error Details: {ex.Message}";
                return (string.Empty, string.Empty);
            }
            catch (TaskCanceledException)
            {
                // Handle task cancellation
                CatFact = "The request was canceled. Please try again.";
                return (string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                // Handles general exceptions and updates the UI with an error message.
                CatFact = $"An error occurred: {ex.Message}";
                return (string.Empty, string.Empty);
            }
        }

        // Adds a new cat data entry to the list and triggers UI update.
        public void UpdateCatList(CatData catData)
        {
            CatDataList.Add(catData); // Adds the new data to the observable collection.
            OnPropertyChanged(nameof(CatDataList)); // Notifies the UI of the updated collection.

            SaveCatDataToFile(catData);  // Saves the new data to a file for persistence.
        }

        // Saves the given cat data to a file in JSON format.
        private void SaveCatDataToFile(CatData newCatData)
        {
            string jsonData = JsonConvert.SerializeObject(newCatData, Formatting.Indented);
            _fileService.SaveToFile(_filePath, jsonData);  // Uses the injected file service to save data.
        }
    }
}

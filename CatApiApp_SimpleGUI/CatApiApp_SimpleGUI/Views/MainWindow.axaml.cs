// Views/MainWindow.axaml.cs

// This file defines the MainWindow class, which represents the main window of the CatApiApp. 
// It handles user interactions, displays data, and manages the communication between the view and the ViewModel.

using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CatApiApp_SimpleGUI.ViewModels;
using CatApiApp_SimpleGUI.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CatApiApp_SimpleGUI.Views
{
    // Represents the main window of the application.
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;  // HttpClient for making API requests.
        private readonly ILogger<MainWindow> _logger; // Logger to log information and errors.

        // Property to hold the instance of the MainWindowViewModel.
        public MainWindowViewModel ViewModel { get; set; }

        // Parameterless constructor for XAML runtime loading.
        // This constructor is required by Avalonia's XAML runtime loader,
        // which needs a default constructor to instantiate the window.
        // It uses the ServiceProvider to inject the necessary HttpClient and Logger.
        public MainWindow() : this(Program.ServiceProvider.GetRequiredService<HttpClient>(), 
                                   Program.ServiceProvider.GetRequiredService<ILogger<MainWindow>>())
        {
        }

        // Public constructor that injects HttpClient and Logger.
        // This constructor is used when dependency injection is required.
        // HttpClient is used to make API requests, and Logger is used for logging errors and events.
        public MainWindow(HttpClient httpClient, ILogger<MainWindow> logger)
        {
            InitializeComponent(); // Initialize the window components from XAML.

            // Assign the injected dependencies.
            _httpClient = httpClient;
            _logger = logger;

            // Resolve MainWindowViewModel from the DI container and set it as the DataContext.
            // DataContext binds the ViewModel to the view (UI) for data binding.
            ViewModel = Program.ServiceProvider.GetRequiredService<MainWindowViewModel>();
            DataContext = ViewModel;
        }

        // Event handler for the "Get Cat" button click.
        // This method asynchronously fetches cat data from the API when the user clicks the button.
        private async void OnGetCatButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Get Cat button clicked."); // Log the button click event.

                // Fetch the cat image and fact asynchronously from the ViewModel.
                var (catImage, catFact) = await ViewModel.FetchCatDataAsync();

                // Check if a cat image URL was returned.
                if (!string.IsNullOrEmpty(catImage))
                {
                    // Log the image URL for debugging.
                    _logger.LogInformation($"Setting CatImage Source to: {catImage}");

                    // Download the image bytes from the URL using HttpClient.
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(catImage);

                    // Display the image in the UI.
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        CatImage.Source = new Bitmap(stream);
                    }

                    _logger.LogInformation("Image loaded successfully.");
                }
                else
                {
                    // If no image URL is available, clear the image in the UI.
                    CatImage.Source = null;
                }

                // Display the retrieved cat fact in the UI.
                CatFactTextBlock.Text = catFact;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors by displaying an error message to the user
                // and logging the error with a description.
                CatFactTextBlock.Text = $"Request error: {ex.Message}";
                _logger.LogError($"Error fetching cat data: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Catch any other unexpected exceptions, display an error message, and log the error.
                CatFactTextBlock.Text = $"An unexpected error occurred: {ex.Message}";
                _logger.LogError($"Unexpected error: {ex.Message}");
            }
        }

        // Event handler for selection changes in the CatListBox.
        // This method loads and displays the image and fact of the selected cat.
        private async void OnCatListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if the selected item in the ListBox is of type CatData.
            if (CatListBox.SelectedItem is CatData selectedCat)
            {
                try
                {
                    // Log the selected image URL for debugging.
                    _logger.LogInformation($"Setting CatImage Source to: {selectedCat.ImageUrl}");

                    // Download the image bytes using HttpClient.
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(selectedCat.ImageUrl);

                    // Display the selected cat's image in the UI.
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        CatImage.Source = new Bitmap(stream);
                    }

                    // Display the selected cat's fact in the UI.
                    CatFactTextBlock.Text = selectedCat.Fact;
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur while loading the image or fact.
                    CatFactTextBlock.Text = $"An error occurred while loading the image: {ex.Message}";
                    _logger.LogError($"Error loading image: {ex.Message}");
                }
            }
        }
    }
}

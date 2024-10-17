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
using Microsoft.Extensions.DependencyInjection;  // Import DI namespace for dependency injection.

namespace CatApiApp_SimpleGUI.Views
{
    // Represents the main window of the application.
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;  // HttpClient for making API requests.

        // Property to hold the instance of the MainWindowViewModel.
        public MainWindowViewModel ViewModel { get; set; }

        // Public constructor that injects the HttpClient instance.
        // The constructor is public to allow access from XAML.
        public MainWindow(HttpClient httpClient)
        {
            InitializeComponent(); // Initialize the window components.
            
            // Assign the injected HttpClient.
            _httpClient = httpClient;

            // Resolve MainWindowViewModel from the DI container and set it as the DataContext.
            ViewModel = Program.ServiceProvider.GetRequiredService<MainWindowViewModel>();
            DataContext = ViewModel; // Set the DataContext to bind the ViewModel to the UI.
        }

        // Event handler for the "Get Cat" button click.
        // Asynchronously fetches cat data and updates the UI with the image and fact.
        private async void OnGetCatButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Button clicked!"); // Output for debugging.
                var (catImage, catFact) = await ViewModel.FetchCatDataAsync();
                
                if (!string.IsNullOrEmpty(catImage))
                {
                    Console.WriteLine($"Setting CatImage Source to: {catImage}"); // Log the image URL.
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(catImage);
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        CatImage.Source = new Bitmap(stream); // Set the image source.
                    }

                    Console.WriteLine("Image loaded successfully!"); // Output for debugging.
                }
                else
                {
                    CatImage.Source = null; // Clear the image if no URL is available.
                }
                
                // Display the retrieved cat fact in the text block.
                CatFactTextBlock.Text = catFact;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors and display a message to the user.
                CatFactTextBlock.Text = $"Request error: {ex.Message}";
                Console.WriteLine($"Error in button click: {ex.Message}"); // Log the error.
            }
            catch (Exception ex)
            {
                // Handle general exceptions and display an error message.
                CatFactTextBlock.Text = $"An unexpected error occurred: {ex.Message}";
                Console.WriteLine($"Error in button click: {ex.Message}"); // Log the error.
            }
        }

        // Event handler for selection changes in the CatListBox.
        // Loads and displays the image and fact of the selected cat.
        private async void OnCatListBoxSelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (CatListBox.SelectedItem is CatData selectedCat)
            {
                try
                {
                    Console.WriteLine($"Setting CatImage Source to: {selectedCat.ImageUrl}"); // Log the selected image URL.
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(selectedCat.ImageUrl);
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        CatImage.Source = new Bitmap(stream); // Set the image source.
                    }
                    CatFactTextBlock.Text = selectedCat.Fact;
                }
                catch (Exception ex)
                {
                    CatFactTextBlock.Text = $"An error occurred while loading the image: {ex.Message}";
                }
            }
        }
    }
}

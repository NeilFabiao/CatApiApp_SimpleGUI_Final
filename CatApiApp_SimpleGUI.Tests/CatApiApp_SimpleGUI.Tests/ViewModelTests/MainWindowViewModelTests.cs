// File: CatApiApp_SimpleGUI.Tests/ViewModelTests/MainWindowViewModelTests.cs
// This file defines tests for the MainWindowViewModel, focusing on the handling of cat data and errors.

using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ViewModelTests
{
    // A base class to handle common setup logic for the MainWindowViewModel tests.
    public abstract class MainWindowViewModelTestBase
    {
        protected Mock<ICatService>? _mockCatService; // Mock object for the ICatService interface
        protected Mock<IFileService>? _mockFileService; // Mock object for the IFileService interface
        protected MainWindowViewModel? _viewModel; // Instance of the MainWindowViewModel to be tested

        // Common setup method for initializing mocks and the ViewModel before each test.
        protected void Setup()
        {
            // Initialize the mocks for ICatService and IFileService
            _mockCatService = new Mock<ICatService>();
            _mockFileService = new Mock<IFileService>();

            // Initialize the ViewModel using the mocked services
            _viewModel = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object);
        }
    }

    // Main test class for xUnit, inheriting from the base setup class
    public class MainWindowViewModelTests : MainWindowViewModelTestBase
    {
        // Constructor that calls the Setup method to initialize mocks and ViewModel
        public MainWindowViewModelTests()
        {
            Setup(); // Initialize the test environment
        }

        [Fact]
        public async Task FetchCatDataAsync_ReturnsCorrectImageUrlAndFact()
        {
            // Arrange: Set up mock methods to return a predefined cat image URL and fact
            _mockCatService!.Setup(service => service.GetCatImageAsync())
                            .ReturnsAsync("https://mockcatimage.com/image.jpg");
            _mockCatService!.Setup(service => service.GetCatFactAsync())
                            .ReturnsAsync("Mock cat fact: Cats sleep for 70% of their lives.");

            // Act: Call the FetchCatDataAsync method on the ViewModel
            var (imageUrl, catFact) = await _viewModel!.FetchCatDataAsync();

            // Assert: Check if the ViewModel's properties were updated with the expected values
            Assert.Equal("https://mockcatimage.com/image.jpg", imageUrl); // Ensure the image URL is correct
            Assert.Equal("Mock cat fact: Cats sleep for 70% of their lives.", catFact); // Ensure the fact is correct
        }

        [Fact]
        public async Task FetchCatDataAsync_HandlesHttp429Error()
        {
            // Arrange: Simulate a scenario where the cat service throws an HTTP 429 Too Many Requests error
            _mockCatService!.Setup(service => service.GetCatImageAsync())
                            .ThrowsAsync(new HttpRequestException("Response status code does not indicate success: 429 (Too Many Requests)"));
            
            // Mock the cat fact to return a normal value to isolate the image error scenario
            _mockCatService!.Setup(service => service.GetCatFactAsync())
                            .ReturnsAsync("Cats are great companions!");

            // Act: Call the FetchCatDataAsync method on the ViewModel
            var (imageUrl, catFact) = await _viewModel!.FetchCatDataAsync();

            // Assert: Ensure the ViewModel handled the error by setting appropriate values
            Assert.Equal(string.Empty, imageUrl); // Check if the image URL is empty on error
            Assert.StartsWith("Whoa there! It looks like we hit the limit for fetching cat data.", _viewModel.CatFact); // Verify the correct error message
        }

        [Fact]
        public async Task FetchCatDataAsync_HandlesMalformedCatFactResponse()
        {
            // Arrange: Simulate a successful cat image response but a malformed cat fact response
            _mockCatService!.Setup(service => service.GetCatImageAsync())
                            .ReturnsAsync("https://cdn2.thecatapi.com/images/adg.jpg");
            
            // Simulate a malformed response from GetCatFactAsync
            _mockCatService!.Setup(service => service.GetCatFactAsync())
                            .ThrowsAsync(new HttpRequestException("Malformed response"));

            // Act: Call the FetchCatDataAsync method on the ViewModel
            var (imageUrl, catFact) = await _viewModel!.FetchCatDataAsync();

            // Assert: Ensure the ViewModel handled the malformed fact response correctly
            Assert.Equal("https://cdn2.thecatapi.com/images/adg.jpg", imageUrl); // Verify that the image URL is still returned correctly
            Assert.StartsWith("An error occurred: Malformed response", _viewModel.CatFact); // Ensure the ViewModel shows the correct error message
        }
    }
}

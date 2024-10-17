// CatApiApp_SimpleGUI.Tests/ServiceTests/MainWindowViewModelErrorIntegrationTests.cs

// This file defines tests that simulate error conditions in the MainWindowViewModel 
// to ensure that it handles exceptions and updates the UI appropriately.

using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ServiceTests
{
    // A test class that focuses on testing how the MainWindowViewModel handles errors 
    // when interacting with the cat data service.
    public class MainWindowViewModelErrorIntegrationTests
    {
        private readonly Mock<ICatService> _mockCatService; // Mock for the cat data service
        private readonly Mock<IFileService> _mockFileService; // Mock for the file service
        private readonly MainWindowViewModel _viewModel; // The ViewModel under test

        // Constructor that sets up the necessary mocks and initializes the ViewModel with them.
        public MainWindowViewModelErrorIntegrationTests()
        {
            // Arrange: Initialize mocks for the services
            _mockCatService = new Mock<ICatService>();
            _mockFileService = new Mock<IFileService>();

            // Create the ViewModel instance using the mocked services
            _viewModel = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object);
        }

        // This test verifies that when the cat data service throws an HttpRequestException, 
        // the ViewModel updates the CatFact property with an appropriate error message.
        [Fact]
        public async Task FetchCatDataAsync_ServiceThrowsError_DisplaysErrorMessage()
        {
            // Arrange: Set up the mocked service to throw an HttpRequestException
            _mockCatService.Setup(service => service.GetCatImageAsync())
                           .ThrowsAsync(new HttpRequestException("Service unavailable"));

            // Act: Call the FetchCatDataAsync method on the ViewModel
            var (imageUrl, catFact) = await _viewModel.FetchCatDataAsync();

            // Assert: Verify that the ViewModel handled the error correctly
            Assert.Equal(string.Empty, imageUrl); // Check that the image URL is empty on error
            Assert.StartsWith("Whoa there!", _viewModel.CatFact); // Verify that the error message is set correctly
        }
    }
}

// CatApiApp_SimpleGUI.Tests/ServiceTests/MainWindowViewModelIntegrationTests.cs
// This file defines integration tests for the MainWindowViewModel class 
// by using mock services to simulate real-world interactions with cat data and file services.

using Moq;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ServiceTests
{
    // A test class that contains integration tests for the MainWindowViewModel 
    // to ensure its correct behavior when interacting with mocked services.
    public class MainWindowViewModelIntegrationTests
    {
        private readonly Mock<ICatService> _mockCatService; // Mock for the cat data service
        private readonly Mock<IFileService> _mockFileService; // Mock for the file service
        private readonly MainWindowViewModel _viewModel; // The ViewModel under test

        // Constructor that sets up the necessary mocks and initializes the ViewModel with them.
        public MainWindowViewModelIntegrationTests()
        {
            // Arrange: Initialize mocks for the services
            _mockCatService = new Mock<ICatService>();
            _mockFileService = new Mock<IFileService>();

            // Create the ViewModel instance using the mocked services
            _viewModel = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object);
        }

        // This test verifies that when valid cat data is returned by the services, 
        // the ViewModel updates its properties accordingly.
        [Fact]
        public async Task FetchCatDataAsync_ValidResponse_UpdatesProperties()
        {
            // Arrange: Define expected values for the mock service responses
            var expectedImageUrl = "https://mockcatimage.com/image.jpg";
            var expectedFact = "Mock cat fact: Cats sleep for 70% of their lives.";

            // Set up the mocked cat service to return these values
            _mockCatService.Setup(service => service.GetCatImageAsync()).ReturnsAsync(expectedImageUrl);
            _mockCatService.Setup(service => service.GetCatFactAsync()).ReturnsAsync(expectedFact);

            // Act: Call the FetchCatDataAsync method on the ViewModel
            var (imageUrl, catFact) = await _viewModel.FetchCatDataAsync();

            // Assert: Verify that the ViewModel's properties were updated with the expected values
            Assert.Equal(expectedImageUrl, imageUrl); // Check if the image URL matches the expected value
            Assert.Equal(expectedFact, catFact); // Check if the fact matches the expected value
        }
    }
}

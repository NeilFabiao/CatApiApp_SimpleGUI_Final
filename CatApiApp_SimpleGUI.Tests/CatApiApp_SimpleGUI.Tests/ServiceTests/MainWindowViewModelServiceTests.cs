// File: CatApiApp_SimpleGUI.Tests/ServiceTests/MainWindowViewModelServiceTests.cs
// This file defines additional service-related tests for the MainWindowViewModel, including
// handling task cancellation, consecutive API calls, and file service error handling.

using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ServiceTests
{
    // A test class that focuses on testing the MainWindowViewModel's interaction with services.
    public class MainWindowViewModelServiceTests
    {
        private readonly Mock<ICatService> _mockCatService;
        private readonly Mock<IFileService> _mockFileService;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelServiceTests()
        {
            // Arrange: Initialize mocks for services
            _mockCatService = new Mock<ICatService>();
            _mockFileService = new Mock<IFileService>();

            // Create the ViewModel instance using the mocked services
            _viewModel = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task FetchCatDataAsync_HandlesTaskCancellation()
        {
            // Arrange: Set up the mock to throw a TaskCanceledException
            _mockCatService.Setup(service => service.GetCatImageAsync())
                           .ThrowsAsync(new TaskCanceledException());

            // Act: Call FetchCatDataAsync to simulate the task cancellation
            var (imageUrl, catFact) = await _viewModel.FetchCatDataAsync();

            // Assert: Ensure that the task cancellation is handled gracefully
            Assert.Equal(string.Empty, imageUrl);  // Image URL should be empty
            Assert.Equal("The request was canceled. Please try again.", _viewModel.CatFact);  // Correct cancellation message
        }

        [Fact]
        public async Task FetchCatDataAsync_HandlesMultipleConsecutiveApiCalls()
        {
            // Arrange: Set up the mock to return different values for consecutive calls
            _mockCatService.SetupSequence(service => service.GetCatImageAsync())
                           .ReturnsAsync("https://mockcatimage.com/image1.jpg")
                           .ReturnsAsync("https://mockcatimage.com/image2.jpg");
            _mockCatService.SetupSequence(service => service.GetCatFactAsync())
                           .ReturnsAsync("First cat fact")
                           .ReturnsAsync("Second cat fact");

            // Act: Call FetchCatDataAsync twice
            var (imageUrl1, catFact1) = await _viewModel.FetchCatDataAsync();
            var (imageUrl2, catFact2) = await _viewModel.FetchCatDataAsync();

            // Assert: Ensure both calls return different data as expected
            Assert.Equal("https://mockcatimage.com/image1.jpg", imageUrl1);
            Assert.Equal("First cat fact", catFact1);
            Assert.Equal("https://mockcatimage.com/image2.jpg", imageUrl2);
            Assert.Equal("Second cat fact", catFact2);
        }

        [Fact]
        public async Task FetchCatDataAsync_SavesCatDataToFile_WhenDataFetched()
        {
            // Arrange: Set up valid cat data
            var expectedImageUrl = "https://mockcatimage.com/image.jpg";
            var expectedFact = "Mock cat fact: Cats sleep for 70% of their lives.";

            // Set up the mocked cat service to return valid data
            _mockCatService.Setup(service => service.GetCatImageAsync()).ReturnsAsync(expectedImageUrl);
            _mockCatService.Setup(service => service.GetCatFactAsync()).ReturnsAsync(expectedFact);

            // Act: Call FetchCatDataAsync to trigger saving
            var (imageUrl, catFact) = await _viewModel.FetchCatDataAsync();

            // Assert: Verify that SaveToFile is called with the correct data
            _mockFileService.Verify(fs => fs.SaveToFile(It.IsAny<string>(), It.Is<string>(json => json.Contains(expectedFact))), Times.Once);
        }

        [Fact]
        public async Task FetchCatDataAsync_FileServiceThrowsError_DisplaysErrorMessage()
        {
            // Arrange: Set up valid cat data and simulate a file service error
            _mockCatService.Setup(service => service.GetCatImageAsync())
                           .ReturnsAsync("https://mockcatimage.com/image.jpg");
            _mockCatService.Setup(service => service.GetCatFactAsync())
                           .ReturnsAsync("Mock cat fact");
            _mockFileService.Setup(fs => fs.SaveToFile(It.IsAny<string>(), It.IsAny<string>()))
                            .Throws(new Exception("File service error"));

            // Act: Call FetchCatDataAsync
            var (imageUrl, catFact) = await _viewModel.FetchCatDataAsync();

            // Assert: Ensure that the error message is handled correctly in the ViewModel
            Assert.Equal("An error occurred: File service error", _viewModel.CatFact);
        }
    }
}

// File: CatApiApp_SimpleGUI.Tests/ViewModelTests/MainWindowViewModelErrorTests.cs
// This file defines tests for error scenarios in the MainWindowViewModel, 
// particularly simulating an HTTP 429 error after multiple successful requests.

using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ViewModelTests
{
    // A test class that focuses on testing how the MainWindowViewModel handles 
    // HTTP 429 Too Many Requests error after several successful requests.
    public class MainWindowViewModelErrorTests
    {
        [Fact]
        public async Task FetchCatDataAsync_SimulatesHttp429ErrorAfterSeveralRequests()
        {
            // Arrange: Create mocks for the cat service and file service
            var mockCatService = new Mock<ICatService>();
            var mockFileService = new Mock<IFileService>();

            // Setup sequence: Simulate successful responses followed by an HTTP 429 error
            mockCatService.SetupSequence(service => service.GetCatImageAsync())
                .ReturnsAsync("https://cdn2.thecatapi.com/images/byQhF07iV.jpg")   // 1st call
                .ReturnsAsync("https://cdn2.thecatapi.com/images/aOj.jpg")        // 2nd call
                .ThrowsAsync(new HttpRequestException("Response status code does not indicate success: 429 (Too Many Requests)"));  // 3rd call throws

            mockCatService.SetupSequence(service => service.GetCatFactAsync())
                .ReturnsAsync("Tylenol and chocolate are both poisonous to cats.")  // 1st call
                .ReturnsAsync("Cats sleep for 70% of their lives.")                // 2nd call
                .ThrowsAsync(new HttpRequestException("Response status code does not indicate success: 429 (Too Many Requests)"));  // 3rd call throws

            // Create an instance of the ViewModel with the mocked services
            var viewModel = new MainWindowViewModel(mockCatService.Object, mockFileService.Object);

            // Act & Assert
            // 1st request: Expect success
            var (imageUrl1, fact1) = await viewModel.FetchCatDataAsync();
            Assert.Equal("https://cdn2.thecatapi.com/images/byQhF07iV.jpg", imageUrl1);
            Assert.Equal("Tylenol and chocolate are both poisonous to cats.", fact1);

            // 2nd request: Expect success
            var (imageUrl2, fact2) = await viewModel.FetchCatDataAsync();
            Assert.Equal("https://cdn2.thecatapi.com/images/aOj.jpg", imageUrl2);
            Assert.Equal("Cats sleep for 70% of their lives.", fact2);

            // 3rd request: Expect HTTP 429 error
            var (imageUrl3, fact3) = await viewModel.FetchCatDataAsync();
            Assert.Equal(string.Empty, imageUrl3);  // Check that the image URL is empty on error
            Assert.Contains("429 (Too Many Requests)", viewModel.CatFact); // Check that the error message contains relevant information
        }
    }
}

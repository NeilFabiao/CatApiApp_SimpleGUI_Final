// File: CatApiApp_SimpleGUI.Tests/ViewModelTests/MainWindowViewModelAdditionalTests.cs
// This file defines additional tests for the MainWindowViewModel, covering various scenarios such as 
// handling empty responses, generic exceptions, updating cat data, and saving data to a file.

using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Models;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ViewModelTests
{
    // A test class that focuses on additional tests for the MainWindowViewModel,
    // including handling empty fact responses, generic exceptions, and saving data to file.
    public class MainWindowViewModelAdditionalTests
    {
        private Mock<ICatService> _mockCatService;  // Mock service for fetching cat data
        private Mock<IFileService> _mockFileService;  // Mock service for file operations
        private MainWindowViewModel _viewModel;  // Instance of the ViewModel being tested

        // Constructor that sets up mocks and initializes the ViewModel
        public MainWindowViewModelAdditionalTests()
        {
            _mockCatService = new Mock<ICatService>();
            _mockFileService = new Mock<IFileService>();
            _viewModel = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task FetchCatDataAsync_HandlesEmptyCatFact()
        {
            // Arrange: Set up a mock response for cat image and an empty fact response
            _mockCatService!.Setup(service => service.GetCatImageAsync())
                            .ReturnsAsync("https://mockcatimage.com/image.jpg");
            _mockCatService!.Setup(service => service.GetCatFactAsync())
                            .ReturnsAsync(string.Empty);  // Simulate empty cat fact

            // Act: Fetch cat data
            var (imageUrl, catFact) = await _viewModel!.FetchCatDataAsync();

            // Assert: Ensure the image is returned
            Assert.Equal("https://mockcatimage.com/image.jpg", imageUrl);
            
            // Wait for the task to complete and then assert the final value of CatFact
            // It was dont this way as catFact was used to display a waiting message to the user
            Assert.Equal("Fetching data...", _viewModel.CatFact);  // Expected result after fetching completes
        }

        [Fact]
        public async Task FetchCatDataAsync_HandlesGenericException()
        {
            // Arrange: Set up a mock that throws a generic exception
            _mockCatService!.Setup(service => service.GetCatImageAsync())
                            .ThrowsAsync(new Exception("Some unexpected error"));

            // Act: Fetch cat data and handle the exception
            var (imageUrl, catFact) = await _viewModel!.FetchCatDataAsync();

            // Assert: Ensure the image is empty and the appropriate error message is set
            Assert.Equal(string.Empty, imageUrl);
            Assert.StartsWith("An error occurred: Some unexpected error", _viewModel.CatFact);
        }

        [Fact]
        public void UpdateCatList_AddsNewCatDataAndRaisesPropertyChanged()
        {
            // Arrange: Set up new cat data and track PropertyChanged event
            var catData = new CatData
            {
                ImageUrl = "https://cdn2.thecatapi.com/images/adg.jpg",
                Fact = "New fact",
                UserName = "TestUser",
                Timestamp = DateTime.UtcNow
            };

            bool propertyChangedRaised = false;
            _viewModel!.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_viewModel.CatDataList))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act: Update the cat data list
            _viewModel.UpdateCatList(catData);

            // Assert: Ensure the new data is added and the PropertyChanged event is raised
            Assert.Contains(catData, _viewModel.CatDataList);
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void Constructor_AddsDummyDataIfRequested()
        {
            // Arrange & Act: Initialize the ViewModel with dummy data
            var viewModelWithDummyData = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object, addDummyData: true);

            // Assert: Ensure that the dummy data is added to CatDataList
            Assert.NotEmpty(viewModelWithDummyData.CatDataList); // Ensure dummy data is added
            Assert.Equal(3, viewModelWithDummyData.CatDataList.Count); // Check the number of dummy entries
        }

        [Fact]
        public void UpdateCatList_SavesCatDataToFile()
        {
            // Arrange: Set up new cat data and mock file saving
            var catData = new CatData
            {
                ImageUrl = "https://cdn2.thecatapi.com/images/adg.jpg",
                Fact = "New fact",
                UserName = "TestUser",
                Timestamp = DateTime.UtcNow
            };

            // Act: Update the cat data list, which should trigger saving to file
            _viewModel.UpdateCatList(catData);

            // Assert: Verify that SaveToFile is called with the correct data
            _mockFileService!.Verify(fileService => fileService.SaveToFile(It.IsAny<string>(), It.Is<string>(json => json.Contains("New fact"))), Times.Once);
        }
    }
}

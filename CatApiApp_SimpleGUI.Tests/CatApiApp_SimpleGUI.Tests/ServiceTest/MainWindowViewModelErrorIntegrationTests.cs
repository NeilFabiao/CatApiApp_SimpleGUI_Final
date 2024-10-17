// CatApiApp_SimpleGUI.Tests/ServiceTests/MainWindowViewModelErrorIntegrationTests.cs
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ViewModelTests
{
    public class MainWindowViewModelErrorIntegrationTests
    {
        private readonly Mock<ICatService> _mockCatService;
        private readonly Mock<IFileService> _mockFileService;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelErrorIntegrationTests()
        {
            // Arrange
            _mockCatService = new Mock<ICatService>();
            _mockFileService = new Mock<IFileService>();

            // Create the ViewModel with the mocked services
            _viewModel = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task FetchCatDataAsync_ServiceThrowsError_DisplaysErrorMessage()
        {
            // Arrange
            _mockCatService.Setup(service => service.GetCatImageAsync())
                           .ThrowsAsync(new HttpRequestException("Service unavailable"));

            // Act
            var (imageUrl, catFact) = await _viewModel.FetchCatDataAsync();

            // Assert
            Assert.Equal(string.Empty, imageUrl); // Check that the image URL is empty on error
            Assert.StartsWith("Whoa there!", _viewModel.CatFact); // Check that the error message is set
        }
    }
}
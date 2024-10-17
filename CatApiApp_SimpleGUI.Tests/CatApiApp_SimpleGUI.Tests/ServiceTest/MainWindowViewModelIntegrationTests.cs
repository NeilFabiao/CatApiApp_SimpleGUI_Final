// CatApiApp_SimpleGUI.Tests/ServiceTests/MainWindowViewModelIntegrationTests.cs
using Moq;
using System.Threading.Tasks;
using CatApiApp_SimpleGUI.Services;
using CatApiApp_SimpleGUI.ViewModels;
using Xunit;

namespace CatApiApp_SimpleGUI.Tests.ViewModelTests
{
    public class MainWindowViewModelIntegrationTests
    {
        private readonly Mock<ICatService> _mockCatService;
        private readonly Mock<IFileService> _mockFileService;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelIntegrationTests()
        {
            // Arrange
            _mockCatService = new Mock<ICatService>();
            _mockFileService = new Mock<IFileService>();

            // Create the ViewModel with the mocked services
            _viewModel = new MainWindowViewModel(_mockCatService.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task FetchCatDataAsync_ValidResponse_UpdatesProperties()
        {
            // Arrange
            var expectedImageUrl = "https://mockcatimage.com/image.jpg";
            var expectedFact = "Mock cat fact: Cats sleep for 70% of their lives.";

            _mockCatService.Setup(service => service.GetCatImageAsync()).ReturnsAsync(expectedImageUrl);
            _mockCatService.Setup(service => service.GetCatFactAsync()).ReturnsAsync(expectedFact);

            // Act
            var (imageUrl, catFact) = await _viewModel.FetchCatDataAsync();

            // Assert
            Assert.Equal(expectedImageUrl, imageUrl);
            Assert.Equal(expectedFact, catFact);
        }
    }
}

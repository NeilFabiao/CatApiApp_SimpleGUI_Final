## Tests

The **CatApiApp_SimpleGUI.Tests** project includes a suite of unit and integration tests to ensure the correct behavior of the `MainWindowViewModel` and its interaction with the services, while also handling various error scenarios.

### Test Suite Overview

The following test files can be found in the `CatApiApp_SimpleGUI.Tests` project directory:

1. **MainWindowViewModelErrorIntegrationTests.cs**  
   Focuses on testing error handling in the ViewModel when interacting with the `CatService`, ensuring appropriate error messages are shown in the UI.

2. **MainWindowViewModelIntegrationTests.cs**  
   Contains integration tests to ensure the ViewModel behaves as expected when using mocked services for fetching cat data (both images and facts).

3. **MainWindowViewModelServiceTests.cs**  
   Tests various service-related interactions, such as task cancellation, consecutive API calls, and file service error handling.

4. **MainWindowViewModelAdditionalTests.cs**  
   Additional tests covering various edge cases, including empty responses, generic exceptions, updating cat data, and saving data to a file.

5. **MainWindowViewModelErrorTests.cs**  
   Simulates HTTP 429 (Too Many Requests) errors after multiple successful requests, testing how the ViewModel handles rate-limiting errors.

6. **MainWindowViewModelTests.cs**  
   Core tests for the ViewModel, verifying the fetching of cat data and correct handling of various types of malformed responses and errors.

### Folder Structure

```bash
CatApiApp_SimpleGUI.Tests/
│
├── ServiceTests/
│   ├── MainWindowViewModelErrorIntegrationTests.cs   # Tests ViewModel error handling for service exceptions
│   ├── MainWindowViewModelIntegrationTests.cs        # Integration tests with mocked services for fetching cat data
│   └── MainWindowViewModelServiceTests.cs            # Tests related to task cancellation, consecutive calls, and file handling
│
├── ViewModelTests/
│   ├── MainWindowViewModelAdditionalTests.cs         # Additional tests for handling edge cases and saving data to file
│   ├── MainWindowViewModelErrorTests.cs              # Simulates HTTP 429 errors and other rate-limiting scenarios
│   └── MainWindowViewModelTests.cs                   # Tests basic functionality, fetching cat data, and error handling
│
└── CatApiApp_SimpleGUI.Tests.csproj                  # Project file for test configurations
```

### Example Test Structure

```csharp
// Example from MainWindowViewModelErrorIntegrationTests.cs
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
```

### Running Tests
To run the tests, navigate to the CatApiApp_SimpleGUI.Tests directory and use the following command:

``` dotnet test ```

This will execute all the test suites, verifying the functionality of the application and ensuring proper error handling and API interaction.


This full **CatApiApp_SimpleGUI.Tests** section gives a detailed description of the test files, folder structure, and instructions on how to run the tests, providing a comprehensive guide for anyone working with or reviewing the tests in your project.

#### Extra

You can run the tests in a specific folder of a .NET project by specifying the path to the test folder when running the dotnet test command. Here’s how you can run the tests in a particular folder, for example, the ServiceTests folder in your CatApiApp_SimpleGUI.Tests project:

```bash
dotnet test --filter FullyQualifiedName~CatApiApp_SimpleGUI.Tests.ServiceTests
```

This command uses the `--filter` option to run tests based on their fully qualified name, which includes the namespace. By specifying `CatApiApp_SimpleGUI.Tests.ServiceTests`, it will run all the tests that are located in that specific folder.

### Breakdown:

- **`dotnet test`**: Runs tests in the specified project.
- **`--filter FullyQualifiedName~`**: Filters tests by the fully qualified name (namespace). The `~` operator matches any tests containing the provided string.
- **`CatApiApp_SimpleGUI.Tests.ServiceTests`**: The namespace corresponding to the `ServiceTests` folder.

You can adjust the filter to point to a different folder or namespace by changing the `FullyQualifiedName~` value.


## References

0. The people I collaborated with during the project
1. [xUnit Documentation](https://xunit.net/docs/getting-started/netcore/cmdline)
2. [The .NET CLI Documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
3. [Test Filtering in .NET](https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests)
4. [NUnit Documentation](https://nunit.org/docs/2.5/testRunner.html)
5. Github and various community discussions for advanced testing techniques

6. GPT-4-turbo, for guidance and assistance with test command structure and filtering options

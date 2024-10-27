# CatApiApp_SimpleGUI_Final

**CatApiApp_SimpleGUI_Final** is a lightweight, cross-platform desktop application built with C# and Avalonia UI, designed to help developers learn the basics of C#. The app fetches random cat images and facts from **TheCatAPI** and **CatFactsAPI**, providing fun with a clean, simple interface.

Built using the robust **MVVM** (Model-View-ViewModel) architecture, this project ensures maintainability and scalability, while an extensive test suite, powered by **xUnit** and **Moq**, guarantees reliability and correctness of the app’s logic.

## Example Cat Images

<p align="center">
  <img src="CatApiApp_SimpleGUI/CatApiApp_SimpleGUI/img_1.png" alt="Example Cat Image 1" width="45%">
  <img src="CatApiApp_SimpleGUI/CatApiApp_SimpleGUI/img_2.png" alt="Example Cat Image 2" width="45%">
</p>

<p align="center">
  <strong>Cat Image 1</strong> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>Cat Image 2</strong>
</p>

### Description:
On the left, **Cat Image 1** shows an example of the app using dummy data for both images and facts. On the right, **Cat Image 2** displays the app with actual data and different cat images, demonstrating how it handles multiple sets of information while maintaining layout and functionality.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [Avalonia UI](https://avaloniaui.net/) framework
- Compatible with macOS, Windows, Linux, or any platform that supports .NET

## Project Structure

The project follows a modular structure with clear separation of concerns for Models, Services, ViewModels, and Views. This structure ensures that the application is maintainable and scalable, adhering to the **MVVM** design pattern.

## Folder Structure

```bash
CatApiApp_SimpleGUI_Final/
│
├── CatApiApp_SimpleGUI/          # Main app folder
│   │
│   ├── Models/
│   │   └── CatData.cs            # Model that represents cat image, fact, and user data.
│   │
│   ├── Services/
│   │   ├── CatService.cs         # Service that handles fetching cat images and facts from APIs.
│   │   └── FileService.cs        # Service for saving and reading cat data to/from files.
│   │
│   ├── ViewModels/
│   │   ├── MainWindowViewModel.cs  # ViewModel containing the logic for fetching and managing cat data.
│   │   └── ViewModelBase.cs        # Base ViewModel providing property change notification mechanism.
│   │
│   ├── Views/
│   │   ├── MainWindow.axaml       # XAML for the main window layout.
│   │   └── MainWindow.axaml.cs    # Code-behind for the main window, handles user interaction.
│   │
│   ├── App.axaml                 # Main application XAML file.
│   ├── App.axaml.cs              # Code-behind for the main application class.
│   ├── Program.cs                # Entry point for the application with dependency injection setup.
│   └── CatApiApp_SimpleGUI.csproj # Project file for the main app.
│
├── CatApiApp_SimpleGUI.Tests/    # Tests folder
│   │
│   ├── ViewModelTests/           # Contains unit tests for ViewModels.
│   ├── ServiceTests/             # Contains unit tests for Services.
│   └── CatApiApp_SimpleGUI.Tests.csproj # Project file for the test suite.
│
└── README.md                    # This file.
```


## Testing

The project includes a suite of unit and integration tests for both the ViewModels and Services using **xUnit** and **Moq**. This ensures the application's logic is properly tested and functions as expected.

To run the tests, use the following command:

```bash
dotnet test 
```


You can also filter tests by specific namespaces or categories:

```bash
dotnet test --filter FullyQualifiedName~CatApiApp_SimpleGUI.Tests.ServiceTests
```



## Install Dependencies

Before running the application, you need to install the required dependencies. Follow these steps:

```bash

dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Newtonsoft.Json
dotnet add package Avalonia
dotnet add package CommunityToolkit.Mvvm

dotnet add package Moq
dotnet add package xunit
dotnet add package Microsoft.Extensions.DependencyInjection
```

## If Starting from Scratch:

Follow these steps to create the project and set up the folder structure:

```bash 
mkdir CatApiApp_SimpleGUI_Final
cd CatApiApp_SimpleGUI_Final
dotnet new avalonia.mvvm -o CatApiApp_SimpleGUI

## Navigate to the CatApiApp_SimpleGUI folder
cd CatApiApp_SimpleGUI

## Create subfolders for Models, Services, ViewModels, and Views
mkdir -p Models Services ViewModels Views

cd ..
mkdir CatApiApp_SimpleGUI.Tests
cd CatApiApp_SimpleGUI.Tests
dotnet new xunit -o CatApiApp_SimpleGUI.Tests

mkdir -p ServiceTests ViewModelTests

```

After installing the necessary prerequisites, clone the repository and navigate to the project directory:

```bash 
git clone https://github.com/yourusername/CatApiApp_SimpleGUI_Final.git
cd CatApiApp_SimpleGUI_Final/CatApiApp_SimpleGUI
```

Restore dependencies and build the project:

```bash 
dotnet restore
dotnet build
```

Finally, run the application:

```
dotnet run
```

References

0. The people I collaborated with during the project
1. [The Cat API](https://thecatapi.com/)
2. [Cat Facts API](https://catfact.ninja/)
3. [AvaloniaUI Documentation](https://docs.avaloniaui.net/)
4. [MVVM Design Pattern](https://learn.microsoft.com/en-us/windows/communitytoolkit/mvvm/introduction)
5. [xUnit Documentation](https://xunit.net/docs/getting-started/netcore/cmdline)
6. [The .NET CLI Documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
7. [Test Filtering in .NET](https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests)
8. [NUnit Documentation](https://nunit.org/docs/2.5/testRunner.html)
9. Github and various resources online

10. GPT-4-turbo, for guidance and assistance throughout the development process




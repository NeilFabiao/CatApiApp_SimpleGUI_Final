# CatApiApp_SimpleGUI_Final

This project is a simple Avalonia-based desktop application built with C# that fetches random cat images and facts using `TheCatAPI` and `CatFact` API. It also includes a testing suite with xUnit and Moq for testing the application’s logic.

## Prerequisites

- .NET SDK (version 6.0 or later)
- Avalonia UI framework
- MacOS or any other platform that supports .NET

## Project Structure

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

## 2. Install Prerequisites

Make sure you have the .NET SDK installed. If not, run the following command:

```bash
brew install dotnet
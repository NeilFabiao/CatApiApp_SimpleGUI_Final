// Program.cs

// This file is the entry point for the CatApiApp application.
// It sets up dependency injection, configures services, and starts the Avalonia application.

using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using System;
using CatApiApp_SimpleGUI.Views;
using CatApiApp_SimpleGUI.ViewModels;
using CatApiApp_SimpleGUI.Services;  // Import necessary services for the application.

namespace CatApiApp_SimpleGUI
{
    // The Program class initializes and configures the application.
    sealed class Program
    {
        // Static property to hold the IServiceProvider instance, enabling access to registered services.
        public static IServiceProvider ServiceProvider { get; private set; } = new ServiceCollection().BuildServiceProvider();

        // The Main method serves as the entry point for the application.
        [STAThread]  // Indicates that the COM threading model for the application is single-threaded.
        public static void Main(string[] args)
        {
            // Create a new ServiceCollection to register dependencies.
            var services = new ServiceCollection();
            ConfigureServices(services); // Configure the services required by the app.
            ServiceProvider = services.BuildServiceProvider(); // Build the service provider for DI.

            // Start the Avalonia app with a classic desktop lifetime, passing in the command-line arguments.
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Configures the services used in the application.
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddHttpClient();  // Register HttpClient for making HTTP requests.
            services.AddTransient<MainWindow>();  // Register MainWindow with transient lifetime.
            services.AddTransient<MainWindowViewModel>();  // Register MainWindowViewModel with transient lifetime.
            
            // Register ICatService and its implementation CatService for dependency injection.
            services.AddTransient<ICatService, CatService>();  // Register cat service.
            services.AddTransient<IFileService, FileService>();  // Register file service.
        }

        // Method to configure and build the Avalonia application.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>() // Configure the main application class.
                         .UsePlatformDetect() // Use platform-specific settings for the application.
                         .LogToTrace(); // Enable logging to trace output for debugging.
    }
}

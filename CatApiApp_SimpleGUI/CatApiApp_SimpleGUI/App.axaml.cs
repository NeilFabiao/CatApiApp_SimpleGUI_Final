// App.axaml.cs

// This file defines the entry point for the CatApiApp, configuring the Avalonia application,
// setting up the main window, and ensuring the necessary configurations for data validation.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CatApiApp_SimpleGUI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CatApiApp_SimpleGUI
{
    // The main application class that extends Avalonia's Application class.
    public partial class App : Application
    {
        // Method to initialize the XAML components of the application.
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this); // Load the XAML defined UI components.
        }

        // Method called when the framework initialization is completed, setting up the main window.
        public override void OnFrameworkInitializationCompleted()
        {
            // Check if the application is using the classic desktop style lifetime.
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CommunityToolkit.
                BindingPlugins.DataValidators.RemoveAt(0);

                // Resolve MainWindow from the DI container to ensure dependencies are injected.
                desktop.MainWindow = Program.ServiceProvider.GetRequiredService<MainWindow>();
            }

            // Call the base method to ensure any additional initialization logic is run.
            base.OnFrameworkInitializationCompleted();
        }
    }
}

// Services/FileService.cs

// This file defines the IFileService interface and its implementation, FileService, 
// which provides methods for saving content to a file and reading content from a file asynchronously.

using System.IO;
using System.Threading.Tasks;

namespace CatApiApp_SimpleGUI.Services
{
    // Interface that defines methods for file operations such as saving and reading data.
    public interface IFileService
    {
        // Saves the specified content to a file at the given file path.
        void SaveToFile(string filePath, string content);

        // Asynchronously reads the content of a file from the given file path.
        Task<string> ReadFromFileAsync(string filePath);
    }

    // Implementation of the IFileService interface, handling file operations like reading and writing data.
    public class FileService : IFileService
    {
        // Saves the specified content to a file at the specified path.
        public void SaveToFile(string filePath, string content)
        {
            // Code for saving content to the file would go here.
        }

        // Asynchronously reads all content from a file at the specified path and returns it as a string.
        public async Task<string> ReadFromFileAsync(string filePath)
        {
            try
            {
                // Opens the file at the given path for reading and returns its content.
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync(); // Reads the entire content of the file asynchronously.
                }
            }
            catch (IOException ex)
            {
                // Logs an error message to the console if reading the file fails.
                System.Console.WriteLine($"Failed to read data: {ex.Message}");
                return string.Empty; // Returns an empty string if an error occurs.
            }
        }
    }
}

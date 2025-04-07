using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using MemoryGame.Helpers;

namespace MemoryGame;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
            
        // Ensure directories exist
        EnsureDirectoriesExist();
        
        GameImagesHelper.InitializeGameCategories();
    }

    private void EnsureDirectoriesExist()
    {
        try
        {
            // Create Data directory if it doesn't exist
            if (!Directory.Exists("./Data"))
            {
                Directory.CreateDirectory("./Data");
            }
                
            // Create SavedGames directory if it doesn't exist
            if (!Directory.Exists("./Data/SavedGames"))
            {
                Directory.CreateDirectory("./Data/SavedGames");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating application directories: {ex.Message}", 
                "Error", 
                MessageBoxButton.OK, 
                MessageBoxImage.Error);
        }
    }
}
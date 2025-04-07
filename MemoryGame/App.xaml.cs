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
        AppDataHelper.EnsureDataFoldersExists();
        
        GameImagesHelper.InitializeGameCategories();
    }
}
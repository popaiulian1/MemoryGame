using System.IO;
using System.Threading.Tasks.Dataflow;

namespace MemoryGame.Helpers;

public static class AppDataHelper
{
    public static string AppRootFolder
    {
        get
        {
            string executingDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(executingDir).Parent.Parent.Parent.FullName;
            return projectRoot;
        }
    }
    
    public static string DataFolder => Path.Combine(AppRootFolder, "Data");
    public static string SavedGamesFolder => Path.Combine(DataFolder, "SavedGames");
    public static string GetUserFilePath() => Path.Combine(DataFolder, "users.json");
    public static string GetStatsFilePath() => Path.Combine(DataFolder, "statistics.json");

    public static void EnsureDataFoldersExists()
    {
        try
        {
            if (!Directory.Exists(DataFolder)) Directory.CreateDirectory(DataFolder);

            if (!Directory.Exists(SavedGamesFolder)) Directory.CreateDirectory(SavedGamesFolder);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error creating data folder: {e.Message}");
            // Fallback to local folders
            Directory.CreateDirectory("./Data");
            Directory.CreateDirectory("./Data/SaveGames");
        }
    }
}
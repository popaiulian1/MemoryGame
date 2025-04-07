using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using MemoryGame.Models;

namespace MemoryGame.Services.SaveGameService;

public class SaveGameService : ISaveGameService
{
    private readonly JsonSerializerOptions _options;

    public SaveGameService()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        VerifySavedGamesDirectoryExists();
    }

    private void VerifySavedGamesDirectoryExists()
    {
        Directory.CreateDirectory("./Data/SavedGames");
    }

    private string GetSaveFilePath(string username, DateTime saveTime)
    {
        string sanitizedUsername = SanitizeFilename(username);
        return Path.Combine("./Data/SavedGames", $"{sanitizedUsername}_{saveTime:yyMMdd_HHmmss}.json");
    }

    private string SanitizeFilename(string filename)
    {
        return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
    }
    
    public bool SaveGame(string username, GameBoard gameBoard, TimeSpan timeRemaining)
    {
        try
        {
            var gameSave = new UserGameSave
            {
                Username = username,
                SavedAt = DateTime.Now,
                BoardHeight = gameBoard.Height,
                BoardWidth = gameBoard.Width,
                CategoryName = gameBoard.Category.Name,
                TimeRemaining = timeRemaining,
                CardsStates = gameBoard.Cards.Select(card => new CardSaveState
                {
                    Id = card.Id,
                    ImagePath = card.ImagePath,
                    IsSelected = card.IsSelected,
                    IsMatched = card.IsMatched,
                }).ToList()
            };

            string saveFilePath = GetSaveFilePath(username, gameSave.SavedAt);
            string jsonContent = JsonSerializer.Serialize(gameSave, _options);
            File.WriteAllText(saveFilePath, jsonContent);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save game: {ex.Message}");
            return false;
        }
    }

    public List<UserGameSave> GetUserSavedGames(string username)
    {
        try
        {
            string sanitizedUsername = SanitizeFilename(username);
            var savedGameFiles = Directory.GetFiles("./Data/SavedGames", $"{sanitizedUsername}.json");

            return savedGameFiles.Select(file =>
            {
                string jsonContent = File.ReadAllText(file);
                return JsonSerializer.Deserialize<UserGameSave>(jsonContent, _options);
            }).OrderByDescending(save => save.SavedAt).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load saved games: {ex.Message}");
            return new List<UserGameSave>();
        }
    }

    public UserGameSave LoadGame(string username, DateTime saveTime)
    {
        try
        {
            string saveFilePath = GetSaveFilePath(username, saveTime);

            if (!File.Exists(saveFilePath)) return null;

            string jsonContent = File.ReadAllText(saveFilePath);
            return JsonSerializer.Deserialize<UserGameSave>(jsonContent, _options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load game: {ex.Message}");
            return null;
        }
    }

    public bool DeleteSavedGame(string username, DateTime saveTime)
    {
        try
        {
            string saveFilePath = GetSaveFilePath(username, saveTime);
            if (!File.Exists(saveFilePath)) return false;

            File.Delete(saveFilePath);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to delete saved games: {e.Message}");
            return false;
        }
    }
}
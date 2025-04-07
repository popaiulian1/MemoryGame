using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using MemoryGame.Helpers;
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
        Directory.CreateDirectory(AppDataHelper.SavedGamesFolder);
    }

    private string GetSaveFilePath(string username, DateTime saveTime)
    {
        string sanitizedUsername = SanitizeFilename(username);
        return Path.Combine(AppDataHelper.SavedGamesFolder, $"{sanitizedUsername}_{saveTime:yyMMdd_HHmmss}.json");
    }

    private string SanitizeFilename(string filename)
    {
        return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
    }
    
    public bool SaveGame(string username, GameBoard gameBoard, TimeSpan timeRemaining)
    {
        try
        {
            Console.WriteLine($"[DEBUG] SaveGame called for user: {username}");
            
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("[DEBUG] Username is null or empty");
                return false;
            }
            
            if (gameBoard == null)
            {
                Console.WriteLine("[DEBUG] GameBoard is null");
                return false;
            }
            
            Console.WriteLine($"[DEBUG] Creating UserGameSave with: Width={gameBoard.Width}, Height={gameBoard.Height}, Category={gameBoard.Category?.Name ?? "null"}");
            
            // Check for null values in any critical components
            if (gameBoard.Category == null)
            {
                Console.WriteLine("[DEBUG] GameBoard.Category is null");
                return false;
            }
            
            if (string.IsNullOrEmpty(gameBoard.Category.Name))
            {
                Console.WriteLine("[DEBUG] GameBoard.Category.Name is null or empty");
                return false;
            }
            
            if (gameBoard.Cards == null)
            {
                Console.WriteLine("[DEBUG] GameBoard.Cards is null");
                return false;
            }
            
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
                    ImagePath = card.ImagePath ?? "unknown",
                    IsSelected = card.IsSelected,
                    IsMatched = card.IsMatched,
                }).ToList()
            };

            string saveFilePath = GetSaveFilePath(username, gameSave.SavedAt);
            Console.WriteLine($"[DEBUG] Save file path: {saveFilePath}");
            
            // Ensure the directory exists
            string directory = Path.GetDirectoryName(saveFilePath);
            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"[DEBUG] Creating directory: {directory}");
                Directory.CreateDirectory(directory);
            }
            
            string jsonContent = JsonSerializer.Serialize(gameSave, _options);
            Console.WriteLine($"[DEBUG] Serialized JSON length: {jsonContent.Length}");
            Console.WriteLine($"[DEBUG] Serialized JSON: {jsonContent}");
            
            File.WriteAllText(saveFilePath, jsonContent);
            Console.WriteLine($"[DEBUG] File written successfully to: {saveFilePath}");
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] Failed to save game: {ex.Message}");
            Console.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
            return false;
        }
    }

    public List<UserGameSave> GetUserSavedGames(string username)
    {
        try
        {
            // Print every step to identify where the null reference occurs
            Console.WriteLine($"[DEBUG] GetUserSavedGames called with username: {username}");
            
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("[DEBUG] Username is null or empty");
                return new List<UserGameSave>();
            }

            string sanitizedUsername = SanitizeFilename(username);
            Console.WriteLine($"[DEBUG] Sanitized username: {sanitizedUsername}");
            
            string savedGamesFolder = AppDataHelper.SavedGamesFolder;
            Console.WriteLine($"[DEBUG] Saved games folder: {savedGamesFolder}");
            
            // Check if directory exists
            Console.WriteLine($"[DEBUG] Directory exists check: {Directory.Exists(savedGamesFolder)}");
            
            if (!Directory.Exists(savedGamesFolder))
            {
                Console.WriteLine($"[DEBUG] Creating directory: {savedGamesFolder}");
                Directory.CreateDirectory(savedGamesFolder);
                return new List<UserGameSave>();
            }
            
            // Get files with full error handling
            string[] savedGameFiles;
            try {
                string pattern = $"{sanitizedUsername}_*.json";
                Console.WriteLine($"[DEBUG] Using search pattern: {pattern}");
                savedGameFiles = Directory.GetFiles(savedGamesFolder, pattern);
                Console.WriteLine($"[DEBUG] Found {savedGameFiles.Length} files");
            }
            catch (Exception ex) {
                Console.WriteLine($"[DEBUG] Error in GetFiles: {ex.Message}");
                return new List<UserGameSave>();
            }
            
            List<UserGameSave> result = new List<UserGameSave>();
            
            // Process each file carefully
            foreach (var file in savedGameFiles)
            {
                Console.WriteLine($"[DEBUG] Processing file: {file}");
                
                try
                {
                    if (!File.Exists(file))
                    {
                        Console.WriteLine($"[DEBUG] File doesn't exist: {file}");
                        continue;
                    }
                    
                    string jsonContent = File.ReadAllText(file);
                    Console.WriteLine($"[DEBUG] File content read, length: {jsonContent?.Length ?? 0}");
                    Console.WriteLine($"[DEBUG] File content: {jsonContent}");
                    
                    if (string.IsNullOrWhiteSpace(jsonContent))
                    {
                        Console.WriteLine($"[DEBUG] File is empty or whitespace: {file}");
                        continue;
                    }
                    
                    // Try to deserialize with careful handling
                    UserGameSave save = null;
                    try {
                        save = JsonSerializer.Deserialize<UserGameSave>(jsonContent, _options);
                        Console.WriteLine($"[DEBUG] Deserialization result: {(save != null ? "success" : "null")}");
                    } 
                    catch (Exception ex) {
                        Console.WriteLine($"[DEBUG] Deserialization exception: {ex.Message}");
                        continue;
                    }
                    
                    if (save == null)
                    {
                        Console.WriteLine($"[DEBUG] Save is null after deserialization");
                        continue;
                    }
                    
                    // Validate all properties
                    Console.WriteLine($"[DEBUG] Validating save: Username={save.Username}, " +
                        $"BoardWidth={save.BoardWidth}, BoardHeight={save.BoardHeight}, " +
                        $"CategoryName={save.CategoryName}, CardsStates={(save.CardsStates != null ? save.CardsStates.Count : 0)}");
                    
                    if (string.IsNullOrEmpty(save.Username))
                    {
                        Console.WriteLine($"[DEBUG] Username is null or empty in save");
                        continue;
                    }
                    
                    if (save.BoardWidth <= 0 || save.BoardHeight <= 0)
                    {
                        Console.WriteLine($"[DEBUG] Invalid board dimensions: {save.BoardWidth}x{save.BoardHeight}");
                        continue;
                    }
                    
                    if (string.IsNullOrEmpty(save.CategoryName))
                    {
                        Console.WriteLine($"[DEBUG] CategoryName is null or empty in save");
                        continue;
                    }
                    
                    if (save.CardsStates == null)
                    {
                        Console.WriteLine($"[DEBUG] CardsStates is null in save");
                        continue;
                    }
                    
                    if (save.CardsStates.Count == 0)
                    {
                        Console.WriteLine($"[DEBUG] CardsStates is empty in save");
                        continue;
                    }
                    
                    // If we get here, save should be valid
                    result.Add(save);
                    Console.WriteLine($"[DEBUG] Added save from {save.SavedAt} to result");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Exception processing file {file}: {ex.Message}");
                    Console.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
                }
            }
            
            var orderedResult = result.OrderByDescending(save => save.SavedAt).ToList();
            Console.WriteLine($"[DEBUG] Returning {orderedResult.Count} saved games");
            return orderedResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] General exception in GetUserSavedGames: {ex.Message}");
            Console.WriteLine($"[DEBUG] Stack trace: {ex.StackTrace}");
            return new List<UserGameSave>();
        }
    }

    public UserGameSave LoadGame(string username, DateTime saveTime)
    {
        try
        {
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Username is null or empty");
                return null;
            }
        
            string saveFilePath = GetSaveFilePath(username, saveTime);
            Console.WriteLine($"Attempting to load game from: {saveFilePath}");

            if (!File.Exists(saveFilePath)) 
            {
                Console.WriteLine($"Save file does not exist: {saveFilePath}");
                return null;
            }

            string jsonContent = File.ReadAllText(saveFilePath);
            var save = JsonSerializer.Deserialize<UserGameSave>(jsonContent, _options);
        
            if (save == null)
            {
                Console.WriteLine("Deserialized save game is null");
                return null;
            }
        
            // Validate the deserialized object
            if (string.IsNullOrEmpty(save.Username) || 
                save.BoardWidth <= 0 || save.BoardHeight <= 0 || 
                string.IsNullOrEmpty(save.CategoryName) || 
                save.CardsStates == null || save.CardsStates.Count == 0)
            {
                Console.WriteLine("Save game is invalid or incomplete");
                return null;
            }
        
            Console.WriteLine($"Successfully loaded save from {save.SavedAt}");
            return save;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load game: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
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
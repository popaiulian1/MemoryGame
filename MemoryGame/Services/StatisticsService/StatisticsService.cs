using System.IO;
using System.Text.Json;
using MemoryGame.Helpers;
using MemoryGame.Models;

namespace MemoryGame.Services.StatisticsService;

public class StatisticsService : IStatisticsService
{
    private readonly string STATISTICS_FILE_PATH;
    private readonly JsonSerializerOptions _options;

    public StatisticsService()
    {
        STATISTICS_FILE_PATH = AppDataHelper.GetStatsFilePath();
        
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        VerifyStatisticsFileExists();
    }

    private void VerifyStatisticsFileExists()
    {
        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(STATISTICS_FILE_PATH));
        
            // Check if file exists and if it's valid JSON array
            if (File.Exists(STATISTICS_FILE_PATH))
            {
                string content = File.ReadAllText(STATISTICS_FILE_PATH);
                // Try to deserialize to check format
                try
                {
                    var users = JsonSerializer.Deserialize<List<User>>(content, _options);
                }
                catch
                {
                    // If deserialization fails, reset the file with empty array
                    File.WriteAllText(STATISTICS_FILE_PATH, "[]");
                    Console.WriteLine("Statistics file was corrupt and has been reset");
                }
            }
            else
            {
                // Create new file with empty array
                File.WriteAllText(STATISTICS_FILE_PATH, "[]");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verifying statistics file: {ex.Message}");
            // Fallback - create in current directory
            File.WriteAllText("./statistics.json", "[]");
        }
    }
    
    public List<User> GetAllStatistics()
    {
        try
        {
            if (!File.Exists(STATISTICS_FILE_PATH))
            {
                File.WriteAllText(STATISTICS_FILE_PATH, "[]");
                return new List<User>();
            }

            string jsonContent = File.ReadAllText(STATISTICS_FILE_PATH);
        
            // Check if file is empty or just whitespace
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                File.WriteAllText(STATISTICS_FILE_PATH, "[]");
                return new List<User>();
            }
        
            return JsonSerializer.Deserialize<List<User>>(jsonContent, _options) ?? new List<User>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading statistics: {e.Message}");
        
            // Reset the file with a valid empty array
            try
            {
                File.WriteAllText(STATISTICS_FILE_PATH, "[]");
            }
            catch { /* Ignore if we can't write */ }
        
            return new List<User>();
        }
    }

    public void UpdateUserStatistics(string username, bool won, TimeSpan gameTime)
    {
        try
        {
            var users = GetAllStatistics();

            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                user = new User
                {
                    Username = username,
                    Statistics = new UserStatistics()
                };
                users.Add(user);
            }

            user.Statistics.RecordGame(won);
            
            string updatedJson = JsonSerializer.Serialize(users, _options);
            File.WriteAllText(STATISTICS_FILE_PATH, updatedJson);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error updating statistics: {e.Message}");
        }
    }

    public User GetUserStatistics(string username)
    {
        return GetAllStatistics().FirstOrDefault(u => u.Username == username);
    }

    public bool ResetUserStatistics(string username)
    {
        try
        {
            var users = GetAllStatistics();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null) return false;

            user.Statistics.GamesPlayed = 0;
            user.Statistics.GamesWon = 0;

            string updatedJson = JsonSerializer.Serialize(user, _options);
            File.WriteAllText(STATISTICS_FILE_PATH, updatedJson);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error resetting statistics: {e.Message}");
            return false;
        }
    }

    public List<User> GetTopPlayers(int count = 10)
    {
        return GetAllStatistics().OrderByDescending(u => u.Statistics.GamesPlayed > 0 
            ? (double)u.Statistics.GamesWon / u.Statistics.GamesPlayed : 0)
            .Take(count).ToList();
    }
}
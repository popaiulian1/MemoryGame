using System.IO;
using System.Text.Json;
using MemoryGame.Models;

namespace MemoryGame.Services.StatisticsService;

public class StatisticsService : IStatisticsService
{
    private const string STATISTICS_FILE_PATH = "./Data/statistics.json";
    private readonly JsonSerializerOptions _options;

    public StatisticsService()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        VerifyStatisticsFileExists();
    }

    private void VerifyStatisticsFileExists()
    {
        if (!File.Exists(STATISTICS_FILE_PATH))
        {
            File.WriteAllText(STATISTICS_FILE_PATH, "[]");
        }
    }
    
    public List<User> GetAllStatistics()
    {
        try
        {
            string jsonContent = File.ReadAllText(STATISTICS_FILE_PATH);
            return JsonSerializer.Deserialize<List<User>>(jsonContent, _options) ?? new List<User>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading statistics: {e.Message}");
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
            
            string updatedJson = JsonSerializer.Serialize(user, _options);
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
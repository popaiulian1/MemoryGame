using MemoryGame.Models;

namespace MemoryGame.Services.StatisticsService;

public interface IStatisticsService
{
    List<User> GetAllStatistics();
    void UpdateUserStatistics(string username, bool won, TimeSpan gameTime);
    User GetUserStatistics(string username);
    bool ResetUserStatistics(string username);
    List<User> GetTopPlayers(int count = 10);
}
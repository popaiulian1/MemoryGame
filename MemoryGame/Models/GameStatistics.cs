using MemoryGame.Helpers;

namespace MemoryGame.Models;

public class GameStatistics : ObservableObject
{
    private int _totalGamesPlayed;
    private int _totalGamesWon;
    private double _winPercentage;
    private TimeSpan _totalPlayedTime;
    private DateTime _lastPlayed;
    
    public int TotalGamesPlayed { get => _totalGamesPlayed; set => SetProperty(ref _totalGamesPlayed, value); }
    public int TotalGamesWon { get => _totalGamesWon; set => SetProperty(ref _totalGamesWon, value); }
    public double WinPercentage { get => _winPercentage; set => SetProperty(ref _winPercentage, value); }
    public TimeSpan TotalPlayedTime { get => _totalPlayedTime; set => SetProperty(ref _totalPlayedTime, value); }
    public DateTime LastPlayed { get => _lastPlayed; set => SetProperty(ref _lastPlayed, value); }

    public void RecordGame(bool won, TimeSpan gameTime)
    {
        TotalGamesPlayed++;
        if (won) TotalGamesWon++;
        TotalPlayedTime += gameTime;
        LastPlayed = DateTime.Now;

        CalculateWinPercentage();
    }

    private void CalculateWinPercentage()
    {
        WinPercentage = TotalGamesPlayed > 0 ? Math.Round((double)TotalGamesWon / TotalGamesPlayed * 100, 2) : 0;
    }

    public void Reset()
    {
        TotalGamesPlayed = 0;
        TotalGamesWon = 0;
        WinPercentage = 0;
        TotalPlayedTime = TimeSpan.Zero;
        LastPlayed = DateTime.MinValue;
    }

    public string GetStatisticsDescription()
    {
        return $"Games Played: {TotalGamesPlayed}\n"+
               $"Games Won: {TotalGamesWon}\n"+
               $"Win Percentage: {WinPercentage}%\n"+
               $"Total Played Time: {TotalPlayedTime}\n"+
               $"Last Played: {(LastPlayed == DateTime.MinValue ? "Never" : LastPlayed.ToString())}";
    }
}
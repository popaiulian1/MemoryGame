using MemoryGame.Helpers;

namespace MemoryGame.Models;

public class UserStatistics : ObservableObject
{
    private int _gamesPlayed;
    private int _gamesWon;
    
    public int GamesPlayed { get => _gamesPlayed; set => SetProperty(ref _gamesPlayed, value); }
    public int GamesWon { get => _gamesWon; set => SetProperty(ref _gamesWon, value); }

    public void RecordGame(bool won)
    {
        GamesPlayed++;
        if (won) GamesWon++;
    }
}
using MemoryGame.Models;

namespace MemoryGame.Services.GameService;

public class GameService : IGameService
{
    public List<GameCategory> GetCategories()
    {
        return new List<GameCategory>
        {
            GameCategory.Animals,
            GameCategory.Fruits,
            GameCategory.Flags
        };
    }

    public bool ValidateBoardConfig(int width, int height)
    {
        return (width * height % 2 == 0) && width is >= 2 and <= 6 && height is >= 2 and <= 6;
    }

    public GameBoard CreateGameBoard(GameCategory category, int width, int height)
    {
        if (!ValidateBoardConfig(width, height))
        {
            throw new ArgumentException("Invalid board configuration");
        }
        
        return new GameBoard(width, height, category);
    }

    public bool IsGameComplete(GameBoard board)
    {
        return board.Cards.All(card => card.IsMatched);
    }

    public int CalculateGameScore(TimeSpan totalTime, int moves)
    {
        int baseScore = 1000;
        int timeScore = (int)(baseScore * (1 - (totalTime.TotalSeconds / 300.0)));
        int moveScore = (int)(baseScore * (1 - (moves / 40.0)));
        return Math.Max(0, (timeScore + moveScore)/2);
    }
}
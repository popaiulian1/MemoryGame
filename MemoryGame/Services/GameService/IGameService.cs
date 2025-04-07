using MemoryGame.Models;

namespace MemoryGame.Services.GameService;

public interface IGameService
{
    List<GameCategory> GetCategories();
    bool ValidateBoardConfig(int width, int height);
    GameBoard CreateGameBoard(GameCategory category, int width, int height);
    bool IsGameComplete(GameBoard board);
    int CalculateGameScore(TimeSpan totalTime, int moves);
}
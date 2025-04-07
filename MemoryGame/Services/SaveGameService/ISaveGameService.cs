using MemoryGame.Models;

namespace MemoryGame.Services.SaveGameService;

public interface ISaveGameService
{
    bool SaveGame(string username, GameBoard gameBoard, TimeSpan timeRemaining);
    List<UserGameSave> GetUserSavedGames(string username);
    UserGameSave LoadGame(string username, DateTime saveTime);
    bool DeleteSavedGame(string username, DateTime saveTime);
}
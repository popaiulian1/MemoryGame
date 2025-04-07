namespace MemoryGame.Models;

public class UserGameSave
{
    public string Username { get; set; }
    public DateTime SavedAt { get; set; }
    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }
    public string CategoryName { get; set; }
    public TimeSpan TimeRemaining { get; set; }
    public List<CardSaveState> CardsStates { get; set; }
}
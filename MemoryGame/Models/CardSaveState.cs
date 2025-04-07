namespace MemoryGame.Models;

public class CardSaveState
{
    public int Id { get; set; }
    public string ImagePath { get; set; }
    public bool IsSelected { get; set; }
    public bool IsMatched { get; set; }
}
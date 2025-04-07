using System.ComponentModel;
using MemoryGame.Helpers;
namespace MemoryGame.Models;

public class GameCard : ObservableObject
{
    private int _id;
    private string _imagePath;
    private bool _isSelected;
    private bool _isMatched;

    public int Id { get => _id; set => SetProperty(ref _id, value); }
    public string ImagePath { get => _imagePath; set => SetProperty(ref _imagePath, value); }
    public bool IsSelected 
    { 
        get => _isSelected; 
        set 
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        } 
    }
    public bool IsMatched { get => _isMatched; set => SetProperty(ref _isMatched, value); }

    public void Reset()
    {
        _isSelected = false;
        _isMatched = false;
    }
}
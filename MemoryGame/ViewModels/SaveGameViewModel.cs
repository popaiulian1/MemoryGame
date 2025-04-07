using System.Collections.ObjectModel;
using System.Windows.Input;
using MemoryGame.Helpers;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Services.GameService;
using MemoryGame.Services.SaveGameService;

namespace MemoryGame.ViewModels;

public class SaveGameViewModel : ObservableObject
{
    private readonly ISaveGameService _saveGameService;
    private readonly User _currentUser;

    private ObservableCollection<UserGameSave> _savedGames;
    private UserGameSave _selectedSave;
    private string _statusMessage;
    
    public ObservableCollection<UserGameSave> SavedGames { get => _savedGames; set => SetProperty(ref _savedGames, value); }
    public UserGameSave SelectedSave { get => _selectedSave; set => SetProperty(ref _selectedSave, value); }
    public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }
    
    public ICommand LoadGameCommand { get; }
    public ICommand DeleteSaveCommand { get; }
    public ICommand RefreshSavesCommand { get; }

    public event EventHandler<GameBoard> GameLoaded;
    
    public SaveGameViewModel(User currentUser)
    {
        _saveGameService = new SaveGameService();
        _currentUser = currentUser;

        LoadGameCommand = new RelayCommand(LoadGame, CanLoadGame);
        DeleteSaveCommand = new RelayCommand(DeleteSave, CanDeleteSave);
        RefreshSavesCommand = new RelayCommand(LoadSavedGames);

        LoadSavedGames();
    }

    private void LoadSavedGames()
    {
        try
        {
            var savedGames = _saveGameService.GetUserSavedGames(_currentUser.Username);
            SavedGames = new ObservableCollection<UserGameSave>(savedGames);

            if (SavedGames.Count > 0)
            {
                SelectedSave = SavedGames[0];
                StatusMessage = $"Found {SavedGames.Count} saved games.";
            }
            else
            {
                StatusMessage = "No saved games found.";
            }
        }
        catch (Exception e)
        {
            StatusMessage = $"Failed to load saved games: {e.Message}";
        }
    }

    private bool CanLoadGame()
    {
        return SelectedSave != null;
    }

    private void LoadGame()
    {
        try
        {
            var gameSave = _saveGameService.LoadGame(_currentUser.Username, SelectedSave.SavedAt);

            if (gameSave != null)
            {
                // Get the correct category
                var categoryList = new GameService().GetCategories();
                var category = categoryList.FirstOrDefault(c => c.Name == gameSave.CategoryName);

                if (category != null)
                {
                    // Create a new game board
                    var board = new GameBoard(gameSave.BoardWidth, gameSave.BoardHeight, category);

                    // Restore card states
                    for (int i = 0; i < board.Cards.Count && i < gameSave.CardsStates.Count; i++)
                    {
                        var cardState = gameSave.CardsStates[i];
                        var card = board.Cards.FirstOrDefault(c => c.Id == cardState.Id);
                    
                        if (card != null)
                        {
                            card.ImagePath = cardState.ImagePath;
                            card.IsMatched = cardState.IsMatched;
                            card.IsSelected = cardState.IsSelected;
                        }
                    }

                    // Raise the event to notify that a game has been loaded
                    GameLoaded?.Invoke(this, board);
                    StatusMessage = "Game loaded successfully!";
                
                    Console.WriteLine("GameLoaded event invoked");
                }
                else
                {
                    StatusMessage = "Failed to find category for saved game.";
                }
            }
            else
            {
                StatusMessage = "Failed to load saved game.";
            }
        }
        catch (Exception e)
        {
            StatusMessage = $"Failed to load saved game: {e.Message}";
            Console.WriteLine($"Exception in LoadGame: {e}");
        }
    }

    private bool CanDeleteSave()
    {
        return SelectedSave != null;
    }

    private void DeleteSave()
    {
        if (SelectedSave != null)
        {
            bool success = _saveGameService.DeleteSavedGame(_currentUser.Username, SelectedSave.SavedAt);

            if (success)
            {
                SavedGames.Remove(SelectedSave);
                StatusMessage = "Saved game deleted successfully.";

                if (SavedGames.Count > 0)
                {
                    SelectedSave = SavedGames[0];
                }
                else
                {
                    SelectedSave = null;
                }
            }
            else
            {
                StatusMessage = "Failed to delete saved game.";
            }
        }
    }
}
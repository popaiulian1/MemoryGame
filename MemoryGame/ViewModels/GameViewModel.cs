using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MemoryGame.Helpers;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Services.GameService;
using MemoryGame.Services.SaveGameService;
using MemoryGame.Services.StatisticsService;

namespace MemoryGame.ViewModels;

public class GameViewModel : ObservableObject
{
    private readonly IGameService _gameService;
    private readonly ISaveGameService _saveGameService;
    private readonly IStatisticsService _statisticsService;
    private readonly User _currentUser;

    private ObservableCollection<GameCategory> _categories;
    private GameCategory _selectedCategory;
    private int _boardWidth;
    private int _boardHeight;
    private GameBoard _board;
    private bool _isGameStarted;
    private bool _isGameFinished;
    private int _moveCount;
    private int _matchCount;
    private TimeSpan _timeElapsed;
    private string _gameStatus;
    private int _gameScore;
    private DispatcherTimer _gameTimer;
    private DateTime _gameStartTime;
    
    public List<int> BoardSizeOptions { get; } = new List<int> { 2, 3, 4, 5, 6 };
    public ObservableCollection<GameCategory> Categories{ get => _categories; set => SetProperty(ref _categories, value); }
    public GameCategory SelectedCategory { get => _selectedCategory; set => SetProperty(ref _selectedCategory, value); }
    public int BoardWidth { get => _boardWidth; set 
    {
        if (SetProperty(ref _boardWidth, value))
        {
            Console.WriteLine($"[DEBUG] BoardWidth set to: {_boardWidth}");
            // Trigger CanExecute re-evaluation for StartGameCommand
            (StartGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }  }
    public int BoardHeight { get => _boardHeight; set 
    {
        if (SetProperty(ref _boardHeight, value))
        {
            Console.WriteLine($"[DEBUG] BoardHeight set to: {_boardHeight}");
            // Trigger CanExecute re-evaluation for StartGameCommand
            (StartGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }  }
    public GameBoard Board { get => _board; set => SetProperty(ref _board, value); }
    public bool IsGameStarted { get => _isGameStarted; set => SetProperty(ref _isGameStarted, value); }
    public bool IsGameFinished { get => _isGameFinished; set => SetProperty(ref _isGameFinished, value); }
    public int MoveCount { get => _moveCount; set => SetProperty(ref _moveCount, value); }
    public int MatchCount { get => _matchCount; set => SetProperty(ref _matchCount, value); }
    public TimeSpan TimeElapsed { get => _timeElapsed; set => SetProperty(ref _timeElapsed, value); }
    public string GameStatus { get => _gameStatus; set => SetProperty(ref _gameStatus, value); }
    public int GameScore { get => _gameScore; set => SetProperty(ref _gameScore, value); }

    public ICommand StartGameCommand { get; }
    public ICommand CardClickCommand { get; }
    public ICommand ResetGameCommand { get; }
    public ICommand SaveGameCommand { get; }

    // Add this constructor overload
    public GameViewModel(IGameService gameService, User currentUser, GameBoard loadedBoard)
    {
        _gameService = gameService;
        _currentUser = currentUser;
    
        _saveGameService = new SaveGameService();
        _statisticsService = new StatisticsService();
    
        // Initialize basic properties
        Categories = new ObservableCollection<GameCategory>(_gameService.GetCategories());
    
        // Set up the loaded board
        Board = loadedBoard;
        BoardWidth = loadedBoard.Width;
        BoardHeight = loadedBoard.Height;
        SelectedCategory = loadedBoard.Category;
    
        // Set game state
        IsGameStarted = true;
        IsGameFinished = false;
    
        // Count matches already found
        MatchCount = loadedBoard.Cards.Count(c => c.IsMatched) / 2;
    
        // Set up move count - this is an estimate
        MoveCount = MatchCount * 2;
    
        // Initialize timer
        _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _gameTimer.Tick += OnTimerClick;
    
        // Start the timer
        _gameStartTime = DateTime.Now;
        _gameTimer.Start();
    
        GameStatus = "Game loaded successfully!";
    
        // Initialize commands
        StartGameCommand = new RelayCommand(StartGame, CanStartGame);
        CardClickCommand = new RelayCommand<GameCard>(OnCardClick, CanClickCard);
        SaveGameCommand = new RelayCommand(SaveGame, CanSaveGame);
        ResetGameCommand = new RelayCommand(ResetGame);
    }
    
    public GameViewModel(IGameService gameService, User currentUser)
    {
        _gameService = gameService;
        _currentUser = currentUser;
        
        _saveGameService = new SaveGameService();
        _statisticsService = new StatisticsService();
        
        // Initialize categories
        Categories = new ObservableCollection<GameCategory>(_gameService.GetCategories());
        SelectedCategory = Categories.FirstOrDefault();
        
        // Set default board size
        BoardWidth = 4;
        BoardHeight = 4;
        
        // Reset game state
        ResetGameState();

        // Initialize commands
        StartGameCommand = new RelayCommand(StartGame, CanStartGame);
        CardClickCommand = new RelayCommand<GameCard>(OnCardClick, CanClickCard);
        SaveGameCommand = new RelayCommand(SaveGame, CanSaveGame);
        ResetGameCommand = new RelayCommand(ResetGame);
        
        // Setup game timer
        _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _gameTimer.Tick += OnTimerClick;
    }

    // Validate and update board size
    private void ValidateAndUpdateBoardSize()
    {
        // Ensure width and height are within valid range
        BoardWidth = Math.Clamp(BoardWidth, 2, 6);
        BoardHeight = Math.Clamp(BoardHeight, 2, 6);

        // Log for debugging
        Console.WriteLine($"[DEBUG] Board size updated: {BoardWidth} x {BoardHeight}");

        // Trigger command re-evaluation
        (StartGameCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    // Check if game can be started
    private bool CanStartGame()
    {
        bool canStart = SelectedCategory != null 
                        && _gameService.ValidateBoardConfig(BoardWidth, BoardHeight)
                        && !IsGameStarted;
        
        Console.WriteLine($"[DEBUG] CanStartGame: " +
                          $"Category={SelectedCategory != null}, " +
                          $"BoardSize={BoardWidth}x{BoardHeight}, " +
                          $"ValidConfig={_gameService.ValidateBoardConfig(BoardWidth, BoardHeight)}, " +
                          $"GameNotStarted={!IsGameStarted}");
        
        return canStart;
    }

    // Start the game
    private void StartGame()
    {
        try
        {
            // Create game board
            Board = _gameService.CreateGameBoard(SelectedCategory, BoardWidth, BoardHeight);
            
            // Reset game state
            IsGameStarted = true;
            IsGameFinished = false;
            MoveCount = 0;
            MatchCount = 0;
            TimeElapsed = TimeSpan.Zero;
            GameStatus = "In progress";
            GameScore = 0;

            // Start game timer
            _gameStartTime = DateTime.Now;
            _gameTimer.Start();

            // Debug logging
            Console.WriteLine($"[DEBUG] Game started: {BoardWidth}x{BoardHeight}, {Board.Cards.Count} cards");
        }
        catch (Exception e)
        {
            GameStatus = $"Error starting game: {e.Message}";
            Console.WriteLine($"[DEBUG] StartGame error: {e}");
        }
    }

    // Reset game state
    private void ResetGameState()
    {
        IsGameStarted = false;
        IsGameFinished = false;
        MoveCount = 0;
        MatchCount = 0;
        TimeElapsed = TimeSpan.Zero;
        GameStatus = "Ready to start";
        GameScore = 0;
    }

    private bool CanClickCard(GameCard card)
    {
        if (card == null || !IsGameStarted || IsGameFinished || card.IsMatched || card.IsSelected)
            return false;
        
        // Don't allow clicking if 2 cards are already revealed
        bool twoCardsAlreadyRevealed = Board.Cards.Count(c => c.IsSelected && !c.IsMatched) >= 2;
    
        return !twoCardsAlreadyRevealed;
    }

    private void OnCardClick(GameCard card)
    {
        // Skip if card is already revealed or matched
        if (card.IsSelected || card.IsMatched)
            return;
        
        // Skip if two cards are already flipped
        var revealedCards = Board.Cards.Where(c => c.IsSelected && !c.IsMatched).ToList();
        if (revealedCards.Count >= 2)
            return;
        
        // Flip the card face up
        card.IsSelected = true;
        
        // Get updated list of revealed cards
        revealedCards = Board.Cards.Where(c => c.IsSelected && !c.IsMatched).ToList();
        
        // If we have two cards face up, check for a match
        if (revealedCards.Count == 2)
        {
            var firstCard = revealedCards[0];
            var secondCard = revealedCards[1];
            
            if (firstCard.ImagePath == secondCard.ImagePath)
            {
                // Match found
                firstCard.IsMatched = true;
                secondCard.IsMatched = true;
                MatchCount++;
                GameStatus = $"Match found! {MatchCount} pairs matched!";
                
                // Check for game completion
                if (Board.Cards.All(c => c.IsMatched))
                {
                    _gameTimer.Stop();
                    IsGameFinished = true;
                    GameScore = _gameService.CalculateGameScore(TimeElapsed, MoveCount);
                    GameStatus = $"Game completed! Score: {GameScore}";
                    
                    _statisticsService.UpdateUserStatistics(_currentUser.Username, true, TimeElapsed);
                }
            }
            else
            {
                // No match - set up timer to flip cards back
                MoveCount++;
                GameStatus = "No match! Cards will flip back...";
                
                // Use Dispatcher.BeginInvoke to ensure UI updates
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke(new Action(() => {
                    // Create a timer with a slight delay before hiding cards
                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(0.6);
                    timer.Tick += (sender, args) => {
                        // Flip cards back
                        firstCard.IsSelected = false;
                        secondCard.IsSelected = false;
                        
                        timer.Stop();
                        GameStatus = $"Turn {MoveCount} - Keep trying!";
                    };
                    timer.Start();
                }));
            }
        }
    }

    private bool CanSaveGame()
    {
        return IsGameStarted && !IsGameFinished;
    }

    private void SaveGame()
    {
        bool saved = _saveGameService.SaveGame(_currentUser.Username, Board, TimeSpan.FromMinutes(5) - TimeElapsed);

        GameStatus = saved ? "Game saved!" : "Failed to save game!";
    }

    private void ResetGame()
    {
        if (IsGameStarted)
        {
            _gameTimer.Stop();
            if (!IsGameFinished)
            {
                _statisticsService.UpdateUserStatistics(_currentUser.Username, false, _timeElapsed);
            }
        }

        Board = null;
        IsGameStarted = false;
        IsGameFinished = false;
        MoveCount = 0;
        MatchCount = 0;
        TimeElapsed = TimeSpan.Zero;
        GameStatus = "Ready to start";
        GameScore = 0;
    }

    private void OnTimerClick(object sender, EventArgs e)
    {
        TimeElapsed = DateTime.Now - _gameStartTime;

        if (TimeElapsed.TotalMinutes >= 5)
        {
            _gameTimer.Stop();
            IsGameFinished = true;
            GameStatus = "Time is up! Game Over. :(";
            
            _statisticsService.UpdateUserStatistics(_currentUser.Username, false, _timeElapsed);
        }
    }
}
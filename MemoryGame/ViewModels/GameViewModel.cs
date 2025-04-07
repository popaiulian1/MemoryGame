using System.Collections.ObjectModel;
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
    
    public ObservableCollection<GameCategory> Categories{ get => _categories; set => SetProperty(ref _categories, value); }
    public GameCategory SelectedCategory { get => _selectedCategory; set => SetProperty(ref _selectedCategory, value); }
    public int BoardWidth { get => _boardWidth; set => SetProperty(ref _boardWidth, value); }
    public int BoardHeight { get => _boardHeight; set => SetProperty(ref _boardHeight, value); }
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

    public GameViewModel(IGameService gameService, User currentUser)
    {
        _gameService = gameService;
        _currentUser = currentUser;
        
        _saveGameService = new SaveGameService();
        _statisticsService = new StatisticsService();
        
        Categories = new ObservableCollection<GameCategory>(_gameService.GetCategories());
        SelectedCategory = Categories.FirstOrDefault();
        BoardWidth = 4;
        BoardHeight = 4;
        IsGameStarted = false;
        IsGameFinished = false;
        MoveCount = 0;
        MatchCount = 0;
        TimeElapsed = TimeSpan.Zero;
        GameStatus = "Ready to start";

        StartGameCommand = new RelayCommand(StartGame, CanStartGame);
        CardClickCommand = new RelayCommand<GameCard>(OnCardClick, CanClickCard);
        SaveGameCommand = new RelayCommand(SaveGame, CanSaveGame);
        ResetGameCommand = new RelayCommand(ResetGame);
        
        _gameTimer = new DispatcherTimer{ Interval = TimeSpan.FromSeconds(1) };
        _gameTimer.Tick += OnTimerClick;
    }

    private bool CanStartGame()
    {
        return SelectedCategory != null 
               && _gameService.ValidateBoardConfig(BoardWidth, BoardHeight) 
               && !IsGameStarted;
    }

    private void StartGame()
    {
        try
        {
            Board = _gameService.CreateGameBoard(SelectedCategory, BoardWidth, BoardHeight);
            IsGameStarted = true;
            IsGameFinished = false;
            MoveCount = 0;
            MatchCount = 0;
            TimeElapsed = TimeSpan.Zero;
            GameStatus = "In progress";
            GameScore = 0;

            _gameStartTime = DateTime.Now;
            _gameTimer.Start();
        }
        catch (Exception e)
        {
            GameStatus = $"Error starting: {e.Message}";
        }
    }

    private bool CanClickCard(GameCard card)
    {
        return IsGameStarted 
               && !IsGameFinished 
               && card != null 
               && !card.IsMatched;
    }

    private void OnCardClick(GameCard card)
    {
        if (Board.RevealCard(card))
        {
            if (card.IsMatched)
            {
                MatchCount++;
                GameStatus = $"Match found! {MatchCount} pairs matched!";

                if (_gameService.IsGameComplete(Board))
                {
                    _gameTimer.Stop();
                    IsGameFinished = true;
                    GameScore = _gameService.CalculateGameScore(TimeElapsed, MoveCount);
                    GameStatus = $"Game completed! Score: {GameScore}";
                    
                    _statisticsService.UpdateUserStatistics(_currentUser.Username, true, _timeElapsed);
                }
            }
            else
            {
                MoveCount++;
                GameStatus = $"Turn {MoveCount} - Keep trying!";
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
using System.Windows.Input;
using MemoryGame.Helpers;
using MemoryGame.Models;
using MemoryGame.Services.GameService;
using MemoryGame.Services.StatisticsService;
using MemoryGame.Services.UserService;

namespace MemoryGame.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IGameService _gameService;
    private readonly IStatisticsService _statisticsService;

    private object _currentView;
    private User _currentUser;
    private bool _isLoggedIn;
    
    public object CurrentView  { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }
    public User CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }
    public bool IsLoggedIn { get => _isLoggedIn; set { _isLoggedIn = value; OnPropertyChanged(); } }
    
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToGameCommand { get; }
    public ICommand NavigateToStatisticsCommand { get; }
    public ICommand NavigateToSavedGamesCommand { get; }
    public ICommand LogOutCommand { get; }

    public MainViewModel(IUserService userService, IGameService gameService, IStatisticsService statisticsService)
    {
        _userService = userService;
        _gameService = gameService;
        _statisticsService = statisticsService;

        NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
        NavigateToGameCommand = new RelayCommand(NavigateToGame, () => IsLoggedIn);
        NavigateToStatisticsCommand = new RelayCommand(NavigateToStatistics, () => IsLoggedIn);
        NavigateToSavedGamesCommand = new RelayCommand(NavigateToSavedGames, () => IsLoggedIn);
        LogOutCommand = new RelayCommand(LogOut, () => IsLoggedIn);

        // Start with login view
        NavigateToLogin();
    }

    private void NavigateToLogin()
    {
        CurrentView = new LoginViewModel(_userService);
        ((LoginViewModel)CurrentView).LoginSuccessful += OnLoginSuccessful;
    }

    private void NavigateToGame()
    {
        CurrentView = new GameViewModel(_gameService, CurrentUser);
    }

    private void NavigateToStatistics()
    {
        CurrentView = new StatisticsViewModel(_statisticsService, CurrentUser);
    }

    private void NavigateToSavedGames()
    {
        CurrentView = new SaveGameViewModel(CurrentUser);
    }
    private void OnLoginSuccessful(object sender, User user) { CurrentUser = user; NavigateToGame(); }
    private void LogOut() { CurrentUser = null; NavigateToLogin(); }

}
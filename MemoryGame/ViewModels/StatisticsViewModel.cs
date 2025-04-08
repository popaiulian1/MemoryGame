using System.Collections.ObjectModel;
using System.Windows.Input;
using MemoryGame.Helpers;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Services.StatisticsService;

namespace MemoryGame.ViewModels;

public class StatisticsViewModel : ObservableObject
{
    private readonly IStatisticsService _statisticsService;

    private ObservableCollection<User> _topPlayers;
    private User _selectedUser;
    private string _statusMessage;

    public ObservableCollection<User> TopPlayers { get => _topPlayers; set => SetProperty(ref _topPlayers, value); }
    public User SelectedUser { get => _selectedUser; set => SetProperty(ref _selectedUser, value); }
    public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }
    public User CurrentUser { get; }

    public ICommand ResetStatsCommand { get; }
    public ICommand RefreshStatsCommand { get; }

    public StatisticsViewModel(IStatisticsService statisticsService, User currentUser)
    {
        _statisticsService = statisticsService;
        CurrentUser = currentUser;

        ResetStatsCommand = new RelayCommand(ResetStats);
        RefreshStatsCommand = new RelayCommand(RefreshStats);

        LoadStatistics();
    }

    private void LoadStatistics()
    {
        try
        {
            var topUsers = _statisticsService.GetTopPlayers();
            TopPlayers = new ObservableCollection<User>(topUsers);
            SelectedUser = CurrentUser;

            StatusMessage = "Statistics loaded successfully.";
        }
        catch (Exception e)
        {
            StatusMessage = $"Error loading statistics: {e.Message}";
        }
    }

    private void ResetStats()
    {
        bool success = _statisticsService.ResetUserStatistics(CurrentUser.Username);

        if (success)
        {
            StatusMessage = "Your statistics have been reset successfully.";
            RefreshStats();
        }
        else
        {
            StatusMessage = "Failed to reset statistics.";
        }
    }

    private void RefreshStats()
    {
        LoadStatistics();
    }
    
}
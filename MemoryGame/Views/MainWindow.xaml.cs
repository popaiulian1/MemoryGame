using System.Windows;
using MemoryGame.Services;
using MemoryGame.Services.GameService;
using MemoryGame.Services.StatisticsService;
using MemoryGame.Services.UserService;
using MemoryGame.ViewModels;

namespace MemoryGame.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var userService = new UserService();
        var gameService = new GameService();
        var statisticsService = new StatisticsService();
        
        DataContext = new MainViewModel(userService, gameService, statisticsService);
    }
}
using System.Windows.Input;
using MemoryGame.Helpers;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Services.UserService;

namespace MemoryGame.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly IUserService _userService;
    
    private string _username = string.Empty;
    private string _statusMessage = string.Empty;
    private bool _isNewUser = false;
    
    public string Username { get => _username; set => SetProperty(ref _username, value); }
    public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }
    public bool IsNewUser { get => _isNewUser; set => SetProperty(ref _isNewUser, value); }
    
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand ToggleRegisterCommand { get; }
    
    public event EventHandler<User> LoginSuccessful;

    public LoginViewModel(IUserService userService)
    {
        _userService = userService;

        LoginCommand = new RelayCommand(Login, CanLogin);
        RegisterCommand = new RelayCommand(Register, CanRegister);
        ToggleRegisterCommand = new RelayCommand(ToggleRegisterMode);

        StatusMessage = "Please login.";
    }
    
    private bool CanLogin() { return !string.IsNullOrWhiteSpace(Username) && !IsNewUser; }
    private bool CanRegister() { return !string.IsNullOrWhiteSpace(Username) && !IsNewUser; }

    private void Login()
    {
        User user = _userService.GetUser(Username);
        
        if (user != null) 
        {
            StatusMessage = $"Logged in as {user.Username}!"; 
            LoginSuccessful?.Invoke(this, user);
        }
        else
        {
            StatusMessage = "User not found.";
        }
    }

    private void Register()
    {
        if (_userService.UserExists(Username))
        {
            StatusMessage = "Username already exists.";
            return;
        }
        
        var newUser = new User { Username = Username };
        bool success = _userService.CreateUser(newUser);

        if (success)
        {
            StatusMessage = "Registration completed! Now login.";
            IsNewUser = false;
        }
        else
        {
            StatusMessage = "Registration failed.";
        }
    }

    private void ToggleRegisterMode()
    {
        IsNewUser = !IsNewUser;
        StatusMessage = IsNewUser ? "Please register a new account." : "Please login with your username.";
        Console.WriteLine($"Toggle Register Mode called, IsNewUser: {IsNewUser}");
        
        OnPropertyChanged(nameof(IsNewUser));
        OnPropertyChanged(nameof(LoginCommand));
        OnPropertyChanged(nameof(RegisterCommand));
    }
}
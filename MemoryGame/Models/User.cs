using MemoryGame.Helpers;
namespace MemoryGame.Models;

public class User : ObservableObject
{
    private string _username;
    private string _avatarPath;
    private UserStatistics _statistics;
    
    public string Username { get=> _username; set => SetProperty(ref _username, value); }
    public string AvatarPath { get => _avatarPath; set => SetProperty(ref _avatarPath, value); }
    public UserStatistics Statistics { get => _statistics; set => SetProperty(ref _statistics, value); }

    public User()
    {
        Statistics = new UserStatistics();
    }
}
using System.IO;
using System.Text.Json;
using MemoryGame.Helpers;
using MemoryGame.Models;

namespace MemoryGame.Services.UserService;

public class UserService : IUserService
{
    private readonly string USER_FILE_PATH;
    private readonly JsonSerializerOptions _options;

    public UserService()
    {
        USER_FILE_PATH = AppDataHelper.GetUserFilePath();
        
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        VerifyFileExists();
    }

    private void VerifyFileExists()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(USER_FILE_PATH)!);
        
        if (!File.Exists(USER_FILE_PATH))
        {
            File.WriteAllText(USER_FILE_PATH, "[]");
        }
    }

    public List<User> GetAllUsers()
    {
        try
        {
            string jsonString = File.ReadAllText(USER_FILE_PATH);
            return JsonSerializer.Deserialize<List<User>>(jsonString, _options) ?? new List<User>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading users: {e.Message}");
            return new List<User>();
        }
    }

    public bool CreateUser(User user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Username)) return false;

        if (UserExists(user.Username)) return false;

        try
        {
            var users = GetAllUsers();

            users.Add(user);
            string jsonString = JsonSerializer.Serialize(users, _options);
            File.WriteAllText(USER_FILE_PATH, jsonString);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error creating user: {e.Message}");
            return false;
        }
    }

    public bool DeleteUser(string username)
    {
        try
        {
            var users = GetAllUsers();
            var userToDelete = users.FirstOrDefault(u => u.Username == username);
            if (userToDelete == null) return false;
            
            users.Remove(userToDelete);
            string jsonString = JsonSerializer.Serialize(users, _options);
            File.WriteAllText(USER_FILE_PATH, jsonString);
            DeleteUserData(username);
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting user: {e.Message}");
            return false;
        }
    }

    public bool UserExists(string username)
    {
        return GetAllUsers().Any(u => u.Username == username);
    }

    public User GetUser(string username)
    {
        return GetAllUsers().FirstOrDefault(u => u.Username == username);
    }

    private void DeleteUserData(string username)
    {
        string savedGamePath = $"{username}_savedgame.json";
        if(File.Exists(savedGamePath)) File.Delete(savedGamePath);
        
        string statsPath = $"{username}_stats.json";
        if (File.Exists(statsPath)) File.Delete(statsPath);
    }
}
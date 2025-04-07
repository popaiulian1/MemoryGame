using MemoryGame.Models;

namespace MemoryGame.Services.UserService;

public interface IUserService
{
    List<User> GetAllUsers();
    bool CreateUser(User user);
    bool DeleteUser(string username);
    bool UserExists(string username);
    User GetUser(string username);
}
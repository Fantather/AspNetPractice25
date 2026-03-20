using AspNetPractice25.Models;

namespace AspNetPractice25
{
    public interface IUser
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(int id);
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);

    }
}

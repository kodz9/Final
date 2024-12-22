using Final.Models;
using Microsoft.AspNetCore.Identity.Data;
namespace Final.Service
{
    public class UserService
    {
        public static UserService Instance { get; private set; } = new UserService();

        private UserService()
        {
        }

        public Dictionary<string, User> Users { get; private set; } = [];

    }
}

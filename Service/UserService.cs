using Final.Models;
using System.Collections.Generic;
using System.Linq;

namespace Final.Service
{
    public class UserService
    {
        public static UserService Instance { get; private set; } = new UserService();

        private UserService()
        {
        }

        // Dictionary to store token and corresponding user
        public Dictionary<string, User> Users { get; private set; } = new Dictionary<string, User>();

        public string GenerateToken(User user)
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Users[token] = user;
            return token;
        }

        public User? GetUserByToken(string token)
        {
            Users.TryGetValue(token, out User? user);
            return user;
        }

        public void RemoveToken(string token)
        {
            Users.Remove(token);
        }

        // 新增方法：检查用户是否有特定权限
        public bool HasPermission(User user, int requiredPermissionId, List<Role> roles)
        {
            var userRoleIds = user.RoleIds.Split(',').Select(int.Parse);
            var userPermissions = roles
                .Where(role => userRoleIds.Contains(role.Id))
                .SelectMany(role => role.PermissionIds.Split(',').Select(int.Parse))
                .Distinct();

            return userPermissions.Contains(requiredPermissionId);
        }
    }
}

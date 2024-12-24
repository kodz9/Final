using Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Final.Service
{
    public class PermissionService(WebContext ctx)
    {
        public bool HasPermission(User user, string requiredPermission)
        {
            // 用户角色ID（字符串，多个用逗号分隔）
            var userRoleIds = user.RoleIds.Split(',').Select(int.Parse).ToList();

            // 获取所有角色
            var roles = ctx.Roles.Where(role => userRoleIds.Contains(role.Id)).ToList();

            // 获取所有权限，找到与 requiredPermission 对应的权限 ID
            var permission = ctx.Permissions.FirstOrDefault(p => p.PermissionName == requiredPermission);

            // 如果权限不存在，返回 false
            if (permission == null)
            {
                return false;
            }

            // 获取角色的所有权限ID
            var rolePermissions = roles
                .SelectMany(role => role.PermissionIds.Split(','))
                .Distinct()
                .ToList(); // 将结果转为 List<string> 方便后续比较

            // 检查角色权限中是否包含对应的权限 ID
            return rolePermissions.Contains(permission.Id.ToString());
        }

    }
}

using Final.Models;
using Final.Service;
using Microsoft.AspNetCore.Mvc;
//yzy///////
namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(WebContext ctx, PermissionService _permissionService) : Controller
    {
        //GET:api/roles/GetRoleSchema
        [HttpGet("{id}/GetRoleById")]
        public IActionResult GetRole(int id)
        {
            var user = HttpContext.Features.Get<User>();
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    msg = "Unauthorized"
                });
            }

            if (!_permissionService.HasPermission(user, "view_role")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to view roles" });
            }
            Role data = ctx.Roles.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"{id} not found"
                });
            }
            return Json(new
            {
                success = true,
                msg = "",
                data
            });
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            var user = HttpContext.Features.Get<User>();
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    msg = "Unauthorized"
                });
            }

            if (!_permissionService.HasPermission(user, "delete_role")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to delete roles" });
            }
            // 查找角色
            var role = ctx.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"Role with ID {id} not found"
                });
            }

            // 删除角色
            ctx.Roles.Remove(role);
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"Role with ID {id} has been deleted"
            });
        }

        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            var user = HttpContext.Features.Get<User>();
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    msg = "Unauthorized"
                });
            }

            if (!_permissionService.HasPermission(user, "add_role")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to add roles" });
            }
            ctx.Roles.Add(role);
            ctx.SaveChanges();
            return Json(new
            {
                success = true,
                msg = "Role added successfully"
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, Role updatedRole)
        {
            var user = HttpContext.Features.Get<User>();
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    msg = "Unauthorized"
                });
            }

            if (!_permissionService.HasPermission(user, "update_role")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to update_role" });
            }
            // 查找角色
            var role = ctx.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"Role with ID {id} not found"
                });
            }
            // 更新角色信息
            role.Description = updatedRole.Description;
            role.RoleName = updatedRole.RoleName;
            // role.permission_ids = updatedRole.permission_ids;
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"Role with ID {id} has been updated"
            });
        }

    }
}

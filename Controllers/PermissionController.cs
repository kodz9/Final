using Final.Models;
using Final.Service;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController(WebContext ctx, PermissionService _permissionService) : Controller
    {
        
        [HttpGet("{id}")]
        public IActionResult GetPermission(int id)
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

            if (!_permissionService.HasPermission(user, "view_permission")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to view permissions" });
            }
            var permission = ctx.Permissions.FirstOrDefault(x => x.Id == id);
            if (permission == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"{id} not found"
                });
            }
            return Ok(permission);
        }

        [HttpPost]
        public IActionResult AddPermission(Permission permission, [FromQuery] string token)
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

            if (!_permissionService.HasPermission(user, "add_permission")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to add permissions" });
            }
            ctx.Permissions.Add(permission);
            ctx.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "Permission added successfully"
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePermission(int id, Permission updatedPermission, [FromQuery] string token)
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

            if (!_permissionService.HasPermission(user, "update_permission")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to update permissions" });
            }
            var permission = ctx.Permissions.FirstOrDefault(x => x.Id == id);
            if (permission == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"Permission with ID {id} not found"
                });
            }

            permission.PermissionName = updatedPermission.PermissionName;
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"Permission with ID {id} has been updated"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePermission(int id, [FromQuery] string token)
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

            if (!_permissionService.HasPermission(user, "delete_permission")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to delete permissions" });
            }
            var permission = ctx.Permissions.FirstOrDefault(x => x.Id == id);
            if (permission == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"Permission with ID {id} not found"
                });
            }

            ctx.Permissions.Remove(permission);
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"Permission with ID {id} has been deleted"
            });
        }
    }
}

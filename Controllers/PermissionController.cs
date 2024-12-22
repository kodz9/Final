using Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController(WebContext ctx) : Controller
    {
        [HttpGet("{id}")]
        public IActionResult GetPermission(int id)
        {
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
        public IActionResult AddPermission(Permission permission)
        {
            ctx.Permissions.Add(permission);
            ctx.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "Permission added successfully"
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePermission(int id, Permission updatedPermission)
        {
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
        public IActionResult DeletePermission(int id)
        {
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

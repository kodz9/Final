using Final.Models;
using Final.Service;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(WebContext ctx,PermissionService _permissionService) : Controller
    {
        [HttpGet("{id}")]
        public IActionResult GetDepartment(int id)
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
            if (!_permissionService.HasPermission(user, "view_department")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to view departments" });
            }
            var department = ctx.Departments.FirstOrDefault(x => x.Id == id);
            if (department == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"{id} not found"
                });
            }
            return Ok(department);
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department, [FromQuery] string token)
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
            if (!_permissionService.HasPermission(user, "add_department")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to add departments" });
            }
            ctx.Departments.Add(department);
            ctx.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "Department added successfully"
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, Department updatedDepartment, [FromQuery] string token)
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
            if (!_permissionService.HasPermission(user, "update_department")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to update departments" });
            }
            var department = ctx.Departments.FirstOrDefault(x => x.Id == id);
            if (department == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"Department with ID {id} not found"
                });
            }

            department.DepartmentName = updatedDepartment.DepartmentName;
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"Department with ID {id} has been updated"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id, [FromQuery] string token)
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
            if (!_permissionService.HasPermission(user, "delete_department")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to delete departments" });
            }
            var department = ctx.Departments.FirstOrDefault(x => x.Id == id);
            if (department == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"Department with ID {id} not found"
                });
            }

            ctx.Departments.Remove(department);
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"Department with ID {id} has been deleted"
            });
        }
    }
}

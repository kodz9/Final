﻿using Final.Models;
using Microsoft.AspNetCore.Mvc;
//yzy///////
namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(WebContext ctx) : Controller
    {
        //GET:api/roles/GetRoleSchema
        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
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
            var role = ctx.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"Role with ID {id} not found"
                });
            }

            role.Description = updatedRole.Description;
            role.RoleName = updatedRole.RoleName;
            //role.permission_ids = updatedRole.permission_ids;
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"Role with ID {id} has been updated"
            });
        }

    }
}

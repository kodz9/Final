using Final.Dto;
using Final.Models;
using Final.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(WebContext ctx, PermissionService _permissionService) : Controller
    {
        private User? user;


        [HttpGet("{id}/GetById")]
        public IActionResult GetUser(int id)
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

            if (!_permissionService.HasPermission(user, "view_user")) // 权限需要自定义
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to view users" });
            }

            var user1 = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (user1 == null||user1.Deleted == 1)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"{id} not found"
                });
            }
            return Ok(user1);
        }

        [HttpPost]
        public IActionResult AddUser(User user1)
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

            if (!_permissionService.HasPermission(user, "add_user")) 
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to add users" });
            }

            try
            {
                ctx.Users.Add(user1);
                ctx.SaveChanges();
                return Ok(new
                {
                    success = true,
                    msg = "User added successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    msg = ex.Message
                });
            }
        }

        [HttpPut("{id}/User")]
        public IActionResult UpdateUser(int id, UpdateUser_user updatedUser)
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

            if (!_permissionService.HasPermission(user, "update_user_user"))
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to update users" });
            }
            var users = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (users == null || users.Deleted == 1)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"User with ID {id} not found"
                });
            }

            users.Mobile = updatedUser.Mobile;
            users.Email = updatedUser.Email;
            users.RealName = updatedUser.RealName;
            users.UserName = updatedUser.UserName;
            users.Gender = updatedUser.Gender;
            users.Address = updatedUser.Address;
            users.Birthday = updatedUser.Birthday;
            users.UserId = updatedUser.UserId;
            users.Password = updatedUser.Password;


            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"User with ID {id} has been updated"
            });
        }

        [HttpPut("{id}/Admin")]
        public IActionResult UpdateUserAdmin(int id, UpdateUser_admin updatedUser)
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

            if (!_permissionService.HasPermission(user, "update_user_admin"))
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to update users" });
            }
            var users = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (users == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"User with ID {id} not found"
                });
            }

            users.Department = updatedUser.Department;


            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"User with ID {id} has been updated"
            });
        }


        [HttpDelete("{id}/test")]
        public IActionResult DeleteUserTest(int id)
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

            if (!_permissionService.HasPermission(user, "delete_user"))
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to delete users" });
            }

            var users = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (users == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"User with ID {id} not found"
                });
            }

            ctx.Users.Remove(users);
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"User with ID {id} has been deleted"
            });
        }

        [HttpPut("{id}/deletUserById")]
        public IActionResult DeleteUserById(int id)
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

            if (!_permissionService.HasPermission(user, "delete_user"))
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to delete users" });
            }

            var users = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (users == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"User with ID {id} not found"
                });
            }

            users.Deleted = 1;
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"User with ID {id} has been deleted"
            });
        }

        [HttpGet("GetByUserName")]
        public IActionResult GetUsersByName(string name)
        {
            // 直接从 HttpContext.Features 获取已验证的 User
            var user = HttpContext.Features.Get<User>();

            // 如果没有找到用户，返回 Unauthorized
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    msg = "Unauthorized"
                });
            }

            if (!_permissionService.HasPermission(user, "view_user"))
            {
                return Unauthorized(new { success = false, msg = "You don't have permission to view users" });
            }
            // 根据用户名查找用户
            var users = ctx.Users
                .Where(x => x.UserName.Contains(name)&&x.Deleted==0)
                .ToList();

            // 如果没有找到相关用户，返回 NotFound
            if (!users.Any())
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"No users found with name containing '{name}'"
                });
            }

            // 返回找到的用户数据
            return Ok(new
            {
                success = true,
                data = users
            });
        }

    


    [HttpPost("login")]
        public IActionResult Login([FromForm] string userName, [FromForm] string password)
        {
            var user = ctx.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    msg = "Invalid username or password"
                });
            }

            string token = UserService.Instance.GenerateToken(user);
            return Ok(new
            {
                success = true,
                token = token
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            var existingUser = ctx.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (existingUser != null)
            {
                return BadRequest(new
                {
                    success = false,
                    msg = "Username already exists"
                });
            }

            // 设置默认值
            user.Department = user.Department ?? "信息学院"; 
            user.RoleIds = user.RoleIds ?? "2"; 
            user.Deleted = 0; // 默认值：未删除

            // 将新用户添加到数据库
            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges(); // 保存到数据库
                return Ok(new
                {
                    success = true,
                    msg = "User registered successfully",
                    data = new { user.UserName, user.RealName } // 返回用户名和真实姓名（可以根据需求扩展）
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    msg = "Error registering user: " + ex.Message
                });
            }
        }
    }
}

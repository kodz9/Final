using Final.Models;
using Final.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(WebContext ctx) : Controller
    {
        private User? user;
       
        [HttpGet("{id}")]
        public IActionResult GetUser(int id, [FromQuery] string token)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"{id} not found"
                });
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser(User user, [FromQuery] string token)
        {
            try
            {
                ctx.Users.Add(user);
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

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser, [FromQuery] string token)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"User with ID {id} not found"
                });
            }

            user.Mobile = updatedUser.Mobile;
            user.Email = updatedUser.Email;
            user.RealName = updatedUser.RealName;
            user.UserName = updatedUser.UserName;
            user.Gender = updatedUser.Gender;
            user.Department = updatedUser.Department;
            user.Address = updatedUser.Address;
            user.Birthday = updatedUser.Birthday;


            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"User with ID {id} has been updated"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id, [FromQuery] string token)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"User with ID {id} not found"
                });
            }

            ctx.Users.Remove(user);
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"User with ID {id} has been deleted"
            });
        }

        [HttpGet("search")]
        public IActionResult SearchUsersByName(string name, [FromQuery] string token)
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

            var users = ctx.Users
                .Where(x => x.UserName.Contains(name))
                .ToList();

            if (!users.Any())
            {
                return NotFound(new
                {
                    success = false,
                    msg = $"No users found with name containing '{name}'"
                });
            }

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

        [HttpPost("logout")]
        public IActionResult Logout([FromForm] string token)
        {
            UserService.Instance.RemoveToken(token);
            return Ok(new
            {
                success = true,
                msg = "User logged out successfully"
            });
        }
    }
}

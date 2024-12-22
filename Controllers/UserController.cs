 using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(WebContext ctx) : Controller
    {
        public User? user;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            user = context.HttpContext.Features.Get<User>();
            if (user == null)
            {
                context.Result = Json(new
                {

                });
            }

            base.OnActionExecuting(context);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    msg = $"{id} not found"
                });
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return Ok(new
            {
                success = true,
                msg = "User added successfully"
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return Json(new
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
            //user.roleIds = updatedUser.roleIds;
            ctx.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = $"User with ID {id} has been updated"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return Json(new
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
        public IActionResult SearchUsersByName(string name)
        {
            var users = ctx.Users
                .Where(x => x.UserName.Contains(name))
                .ToList();

            if (!users.Any())
            {
                return Json(new
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
    }
}

using Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class UserController(WebContext ctx) : Controller
    {
        public IActionResult GetUser(int id)
        {
            //var query = from User in ctx.Users
            //           where User.Id == id
            //           select User;
            //var user = query.FirstOrDefault(); 
            User data = ctx.Users.FirstOrDefault(e => e.Id == id);
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
        public IActionResult AddUser(User user)
        {
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return Json(new
            {
                success = true,
                msg = ""
            });
        }

        public IActionResult DeleteUser(int id)
        {
           var query = from user in ctx.Users 
                       where user.Id == id
                       select user;
            var User = query.FirstOrDefault();
            if (User == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "未找到对应的商品",
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    msg = ""
                });
            }
        }

    }
}

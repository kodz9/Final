using Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(WebContext ctx) : Controller
    {
        [HttpGet("{id}")]
        public IActionResult GetDepartment(int id, [FromQuery] string token)
        {
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

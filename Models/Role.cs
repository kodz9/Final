using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
//yzy
namespace Final.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? RoleName { get; set; }

        public string? PermissionIds { get; set; }
        public int Deleted { get; set; }

    }
}

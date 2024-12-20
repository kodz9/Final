using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;

namespace Final.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? RoleName { get; set; }
        public string? permission_ids { get; set; }

    }
}

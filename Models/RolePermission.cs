﻿namespace Final.Models
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
    }
}

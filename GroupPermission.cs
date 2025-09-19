namespace UserGroupPermissions.Models;

using System;

public class GroupPermission
{
    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
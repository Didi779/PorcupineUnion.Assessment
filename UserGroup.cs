namespace UserGroupPermissions.Models;

using System;

public class UserGroup
{
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = default!;

    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
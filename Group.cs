namespace UserGroupPermissions.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Group
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();

    public ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();
}
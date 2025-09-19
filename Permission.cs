namespace UserGroupPermissions.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Permission
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();
}
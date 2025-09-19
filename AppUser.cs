namespace UserGroupPermissions.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class AppUser
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string UserName { get; set; } = default!;

    [Required]
    [MaxLength(320)]
    public string Email { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Timestamp]
    public byte[]? RowVersion { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
}
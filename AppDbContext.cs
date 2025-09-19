namespace UserGroupPermissions.Data;

using Microsoft.EntityFrameworkCore;
using UserGroupPermissions.Models;
using System;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<GroupPermission> GroupPermissions => Set<GroupPermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<AppUser>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.Email).IsUnique();
            b.Property(u => u.UserName).IsRequired().HasMaxLength(200);
            b.Property(u => u.Email).IsRequired().HasMaxLength(320);
            b.Property(u => u.RowVersion).IsRowVersion();
        });

        // Groups
        modelBuilder.Entity<Group>(b =>
        {
            b.HasKey(g => g.Id);
            b.HasIndex(g => g.Name).IsUnique();
            b.Property(g => g.Name).IsRequired().HasMaxLength(200);
        });

        // Permissions
        modelBuilder.Entity<Permission>(b =>
        {
            b.HasKey(p => p.Id);
            b.HasIndex(p => p.Name).IsUnique();
            b.Property(p => p.Name).IsRequired().HasMaxLength(150);
        });

        // UserGroup (many-to-many: Users <-> Groups)
        modelBuilder.Entity<UserGroup>(b =>
        {
            b.HasKey(ug => new { ug.AppUserId, ug.GroupId });

            b.HasOne(ug => ug.AppUser)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(ug => ug.GroupId);
        });

        // GroupPermission (many-to-many: Groups <-> Permissions)
        modelBuilder.Entity<GroupPermission>(b =>
        {
            b.HasKey(gp => new { gp.GroupId, gp.PermissionId });

            b.HasOne(gp => gp.Group)
                .WithMany(g => g.GroupPermissions)
                .HasForeignKey(gp => gp.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(gp => gp.Permission)
                .WithMany(p => p.GroupPermissions)
                .HasForeignKey(gp => gp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data (note: use constant DateTimes in HasData)
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "Level1", Description = "Basic access" },
            new Permission { Id = 2, Name = "Level2", Description = "Intermediate access" },
            new Permission { Id = 3, Name = "Admin", Description = "Administrative access" }
        );

        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Name = "GroupA", Description = "Alpha group" },
            new Group { Id = 2, Name = "GroupB", Description = "Beta group" }
        );

        modelBuilder.Entity<AppUser>().HasData(
            new AppUser { Id = 1, UserName = "alice", Email = "alice@example.com", CreatedAt = new DateTime(2024, 1, 1) },
            new AppUser { Id = 2, UserName = "bob", Email = "bob@example.com", CreatedAt = new DateTime(2024, 1, 2) }
        );

        modelBuilder.Entity<GroupPermission>().HasData(
            new { GroupId = 1, PermissionId = 1 },
            new { GroupId = 1, PermissionId = 2 },
            new { GroupId = 2, PermissionId = 1 },
            new { GroupId = 2, PermissionId = 3 }
        );

        modelBuilder.Entity<UserGroup>().HasData(
            new { AppUserId = 1, GroupId = 1 },
            new { AppUserId = 2, GroupId = 2 }
        );
    }
}
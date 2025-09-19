using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using UserGroupPermissions.Data;
using UserGroupPermissions.Models;
using UserGroupPermissions.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Configuration: change connection string in appsettings.json before running migrations
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

// Create a user
app.MapPost("/users", async (AppDbContext db, CreateUserDto dto) =>
{
    var user = new AppUser { UserName = dto.UserName, Email = dto.Email };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

// Create a group
app.MapPost("/groups", async (AppDbContext db, CreateGroupDto dto) =>
{
    var g = new Group { Name = dto.Name, Description = dto.Description };
    db.Groups.Add(g);
    await db.SaveChangesAsync();
    return Results.Created($"/groups/{g.Id}", g);
});

// Create a permission
app.MapPost("/permissions", async (AppDbContext db, CreatePermissionDto dto) =>
{
    var p = new Permission { Name = dto.Name, Description = dto.Description };
    db.Permissions.Add(p);
    await db.SaveChangesAsync();
    return Results.Created($"/permissions/{p.Id}", p);
});

// Assign user to group
app.MapPost("/usergroups", async (AppDbContext db, AssignUserToGroupDto dto) =>
{
    var exists = await db.UserGroups.FindAsync(dto.UserId, dto.GroupId);
    if (exists != null) return Results.BadRequest("User already in group");

    var ug = new UserGroup { AppUserId = dto.UserId, GroupId = dto.GroupId };
    db.UserGroups.Add(ug);
    await db.SaveChangesAsync();
    return Results.Ok(ug);
});

// Assign permission to group
app.MapPost("/grouppermissions", async (AppDbContext db, AssignPermissionToGroupDto dto) =>
{
    var exists = await db.GroupPermissions.FindAsync(dto.GroupId, dto.PermissionId);
    if (exists != null) return Results.BadRequest("Permission already assigned to group");

    var gp = new GroupPermission { GroupId = dto.GroupId, PermissionId = dto.PermissionId };
    db.GroupPermissions.Add(gp);
    await db.SaveChangesAsync();
    return Results.Ok(gp);
});

// Get user's groups
app.MapGet("/users/{userId:int}/groups", async (AppDbContext db, int userId) =>
{
    var groups = await db.UserGroups
        .Include(ug => ug.Group)
        .Where(ug => ug.AppUserId == userId)
        .Select(ug => ug.Group)
        .ToListAsync();
    return Results.Ok(groups);
});

// Get user's effective permissions (via groups)
app.MapGet("/users/{userId:int}/permissions", async (AppDbContext db, int userId) =>
{
    var permissions = await db.UserGroups
        .Where(ug => ug.AppUserId == userId)
        .SelectMany(ug => ug.Group.GroupPermissions)
        .Select(gp => gp.Permission)
        .Distinct()
        .ToListAsync();
    return Results.Ok(permissions);
});

app.Run();
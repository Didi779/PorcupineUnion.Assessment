namespace UserGroupPermissions.Dtos;

public record CreateUserDto(string UserName, string Email);
public record CreateGroupDto(string Name, string? Description);
public record CreatePermissionDto(string Name, string? Description);
public record AssignUserToGroupDto(int UserId, int GroupId);
public record AssignPermissionToGroupDto(int GroupId, int PermissionId);
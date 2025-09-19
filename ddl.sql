-- Reference DDL for the code-first model (simplified)

CREATE TABLE [Users] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserName] NVARCHAR(200) NOT NULL,
    [Email] NVARCHAR(320) NOT NULL UNIQUE,
    [CreatedAt] DATETIME2 NOT NULL,
    [RowVersion] ROWVERSION NULL
);

CREATE TABLE [Groups] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL UNIQUE,
    [Description] NVARCHAR(MAX) NULL
);

CREATE TABLE [Permissions] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(150) NOT NULL UNIQUE,
    [Description] NVARCHAR(MAX) NULL
);

CREATE TABLE [UserGroups] (
    [AppUserId] INT NOT NULL,
    [GroupId] INT NOT NULL,
    [AssignedAt] DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    PRIMARY KEY ([AppUserId], [GroupId]),
    FOREIGN KEY ([AppUserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([GroupId]) REFERENCES [Groups]([Id]) ON DELETE CASCADE
);

CREATE TABLE [GroupPermissions] (
    [GroupId] INT NOT NULL,
    [PermissionId] INT NOT NULL,
    [AssignedAt] DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    PRIMARY KEY ([GroupId], [PermissionId]),
    FOREIGN KEY ([GroupId]) REFERENCES [Groups]([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([PermissionId]) REFERENCES [Permissions]([Id]) ON DELETE CASCADE
);
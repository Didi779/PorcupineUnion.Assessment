# UserGroupPermissions (EF Core - Code First)

This sample implements a simple, maintainable, and scalable code-first schema using Entity Framework Core (SQL Server).

**Features**
- AppUser <-> Group (many-to-many) via `UserGroups` join table
- Group <-> Permission (many-to-many) via `GroupPermissions` join table
- Seed data for quick start
- Minimal APIs for CRUD and assignment operations
- Concurrency token (RowVersion) on users

**Run locally**
1. Install .NET SDK (recommended: .NET 8)
2. Edit `appsettings.json` connection string `DefaultConnection` to point to your SQL Server
3. From project root run:

```bash
dotnet restore
dotnet tool install --global dotnet-ef # if you don't already have it
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

Open `https://localhost:5001/swagger` to try endpoints with Swagger UI.

**Push to GitHub**

```bash
git init
git add .
git commit -m "Initial commit - EF Core code-first sample"
gh repo create your-username/UserGroupPermissions --public --source=. --remote=origin
git push -u origin main
```

(If you don't use `gh` CLI, create repo on github.com and set remote origin with `git remote add origin <url>` then `git push -u origin main`.)

**Notes on design choices**
- Explicit join entities (`UserGroup` and `GroupPermission`) give flexibility to add metadata (AssignedAt, role within group, etc.) in the future
- Unique indexes on Emails & Names to ensure fast lookup and data integrity
- RowVersion concurrency token on `AppUser` to safely handle concurrent updates
- Seeded permissions and groups to bootstrap environments

**Next steps / scaling**
- Add caching layer (Redis) to cache user permission sets
- Add CQRS/read-model for heavy-read endpoints (user permissions) and materialize a denormalized table for faster lookup
- Add soft-delete and auditing tables for historical reasons

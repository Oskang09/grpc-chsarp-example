# EF Core Migration

Only create migrations when released, currently just run

```
dotnet ef migrations add new
dotnet ef database update
```

# Dotnet Restoring Issues

```
dotnet nuget locals all -c
```
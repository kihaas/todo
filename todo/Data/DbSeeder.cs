using todo.Models;

namespace todo.Data;

public static class DbSeeder
{
    public static void SeedAdmin(AppDbContext db, IConfiguration config)
    {
        var username = config["SeedAdmin:Username"];
        var email = config["SeedAdmin:Email"];
        var password = config["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
            return;

        if (db.Users.Any(u => u.Role == "Admin"))
            return;

        db.Users.Add(new User
        {
            Username = username,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Admin"
        });
        db.SaveChanges();
    }
}
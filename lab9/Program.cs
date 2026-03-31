using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lab9
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Lab9_Base;Trusted_Connection=True;");
    }

    class Program
    {
        static void Main()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            if (!db.Users.Any())
            {
                db.Users.Add(new User { Name = "Oleg" });
                db.SaveChanges();
                Console.WriteLine("Користувача додано до бази!");
            }

            var user = db.Users.First();
            Console.WriteLine($"Привіт, {user.Name}! База працює.");
            Console.ReadKey();
        }
    }
}
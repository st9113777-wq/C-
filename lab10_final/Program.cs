using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10
{
    
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public List<Project> Projects { get; set; } = new();
    }

    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<TodoItem> Tasks { get; set; } = new();
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }

    
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Lab10_FinalDB;Trusted_Connection=True;");
        }
    }

    
    public class DataRepository
    {
        private readonly AppDbContext _db;
        public DataRepository(AppDbContext db) => _db = db;

        public async Task AddUserAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> GetFullDataAsync()
        {
            return await _db.Users
                .Include(u => u.Projects)
                    .ThenInclude(p => p.Tasks)
                .ToListAsync();
        }
    }

    class Program
    {
        static async Task Main()
        {
            using var db = new AppDbContext();
            await db.Database.EnsureCreatedAsync();

            var repo = new DataRepository(db);

            Console.WriteLine("--- Демонстрація створення даних ---");
            var user = new User { FullName = "Олег" };
            var proj = new Project { Title = "Фінальний проєкт" };
            proj.Tasks.Add(new TodoItem { TaskName = "Завершити лабу 10", IsDone = true });
            user.Projects.Add(proj);

            await repo.AddUserAsync(user);
            Console.WriteLine("Дані успішно додано через репозиторій.");

            var data = await repo.GetFullDataAsync();
            Console.WriteLine($"Користувач: {data[0].FullName}, Проєктів: {data[0].Projects.Count}");
        }
    }
}
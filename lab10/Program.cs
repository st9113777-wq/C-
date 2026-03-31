using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lab10
{
    // Модель даних (Сутність) з новим полем Status (Завдання 10.2)
    public class ProjectItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = "New";
    }

    // Контекст бази даних (Завдання 10.2)
    public class AppDbContext : DbContext
    {
        public DbSet<ProjectItem> Items => Set<ProjectItem>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Налаштування підключення до локальної БД (Завдання 10.1)
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Lab10_Migrations;Trusted_Connection=True;");
        }
    }

    class Program
    {
        static void Main()
        {
            // Використання короткотривалого контексту (Завдання 10.2)
            using (var db = new AppDbContext())
            {
                // Початкове налаштування бази для демонстрації
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine("--- Додавання запису (стан Added) ---");
                var newItem = new ProjectItem { Title = "Виконати лабу 10", Status = "In Progress" };
                db.Items.Add(newItem);

                // Демонстрація Change Tracking (п. 10.2)
                Console.WriteLine($"Поточний стан: {db.Entry(newItem).State}");
                db.SaveChanges();

                Console.WriteLine("\n--- Оновлення запису (стан Modified) ---");
                var item = db.Items.First();
                item.Title = "Лабораторна робота завершена";

                // Перевірка стану перед SaveChanges (Завдання 10.2)
                Console.WriteLine($"Поточний стан: {db.Entry(item).State}");
                db.SaveChanges();

                Console.WriteLine("\n--- Видалення запису (стан Deleted) ---");
                db.Items.Remove(item);
                Console.WriteLine($"Поточний стан: {db.Entry(item).State}");
                db.SaveChanges();

                Console.WriteLine("\nДемонстрацію CRUD та Change Tracking завершено успішно!");
            }
        }
    }
}
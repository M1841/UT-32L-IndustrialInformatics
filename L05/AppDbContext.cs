using Microsoft.EntityFrameworkCore;

using L05.Models;

namespace L05;

public class AppDbContext : DbContext
{
  public DbSet<Models.Thread> Threads { get; set; }
  public DbSet<Reply> Replies { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("DataSource=II_L05.sqlite");

  public static void SeedData()
  {
    using AppDbContext db = new();
    if (!db.Database.EnsureCreated()) { return; }

    Models.Thread thread1 = new()
    {
      Author = "User0",
      Title = "Thread 1",
      Description = "First thread, shows two replies",
      CreatedAt = DateTime.Now
    };
    thread1.Replies.Add(new()
    {
      Author = "User0",
      Content = "Head of first thread",
      CreatedAt = DateTime.Now,
    });
    thread1.Replies.Add(new()
    {
      Author = "User1",
      Content = "Reply to first thread",
      CreatedAt = DateTime.Now
    });

    Models.Thread thread2 = new()
    {
      Author = "User1",
      Title = "Thread 2",
      Description = "Second thread, shows one reply",
      CreatedAt = DateTime.Now
    };
    thread2.Replies.Add(new()
    {
      Author = "User2",
      Content = "Head of second thread",
      CreatedAt = DateTime.Now,
    });

    db.Add(thread1);
    db.Add(thread2);
    db.SaveChanges();
  }
}

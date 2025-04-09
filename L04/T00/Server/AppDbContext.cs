using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace L04.T00.Server;

public class AppDbContext : DbContext
{
  public DbSet<Thread> Threads { get; set; }
  public DbSet<Reply> Replies { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("DataSource=II_L04_T00.sqlite");

  public static void SeedData()
  {
    using AppDbContext db = new();
    if (!db.Database.EnsureCreated()) { return; }

    Thread thread1 = new()
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

    Thread thread2 = new()
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

public class Thread
{
  public Guid Id { get; set; }
  public required string Author { get; set; }
  public required string Title { get; set; }
  public string? Description { get; set; }
  public required DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

  public ICollection<Reply> Replies { get; set; } = [];
}

public record ThreadCreateDto(
  string Author,
  string Title,
  string? Description
)
{ }

public record ThreadUpdateDto(
  string? Title,
  string? Description
)
{ }

public class Reply
{
  public Guid Id { get; set; }
  public Guid ThreadId { get; set; }
  public required string Author { get; set; }
  public required string Content { get; set; }
  public required DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

  [JsonIgnore]
  [ForeignKey(nameof(ThreadId))]
  public Thread? Thread { get; set; }
}

public record ReplyCreateDto(
  Guid ThreadId,
  string Author,
  string Content
)
{ }

public record ReplyUpdateDto(
  string Content
)
{ }

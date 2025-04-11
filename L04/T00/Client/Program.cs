using System.Text.Json;
using System.Text.Json.Serialization;

namespace L04.T00.Client;

static class Program
{
  [STAThread]
  static void Main()
  {
    ApplicationConfiguration.Initialize();
    Application.Run(new UserForm());
  }

  public static string User = "";

  public static Thread[] Threads = [];
  public static Reply[] Replies = [];

  public static Threads? ThreadsWindow;
  public static Replies? RepliesWindow;

  public static readonly HttpClient Http = new()
  {
    BaseAddress = new Uri("http://localhost:5190")
  };
  public static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  public static string RelativeTime(DateTime dateTime)
  {
    TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);
    if (timeSpan.TotalDays > 365)
    {
      return $"{timeSpan.TotalDays / 365:0} years ago";
    }
    if (timeSpan.TotalDays > 30)
    {
      return $"{timeSpan.TotalDays / 30:0} months ago";
    }
    if (timeSpan.TotalDays > 7)
    {
      return $"{timeSpan.TotalDays / 7:0} weeks ago";
    }
    if (timeSpan.TotalDays > 1)
    {
      return $"{timeSpan.TotalDays:0} days ago";
    }
    if (timeSpan.TotalHours > 1)
    {
      return $"{timeSpan.TotalHours:0} hours ago";
    }
    if (timeSpan.TotalMinutes > 1)
    {
      return $"{timeSpan.TotalMinutes:0} minutes ago";
    }
    if (timeSpan.TotalSeconds > 1)
    {
      return $"{timeSpan.TotalSeconds:0} seconds ago";
    }
    return "Now";
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

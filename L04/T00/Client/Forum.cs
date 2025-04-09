using System.Text.Json;
using System.Text.Json.Serialization;

namespace L04.T00.Client;

public partial class Forum : Form
{
  public Forum()
  {
    try
    {
      InitializeComponent();

      Controls.Add(topGroup);
      topGroup.Controls.Add(userLabel);
      topGroup.Controls.Add(userTextbox);
      userTextbox.KeyUp += (sender, args) =>
      {
        newThreadBtn.Enabled = userTextbox.Text != "";
        User = userTextbox.Text;
      };
      topGroup.Controls.Add(searchLabel);
      topGroup.Controls.Add(searchTextbox);
      searchTextbox.KeyUp += (sender, args) =>
      {
        if (searchTextbox.Text != "")
        {
          UpdateThreads(Threads
            .Where(t =>
              t.Title.Contains(
                searchTextbox.Text,
                StringComparison.CurrentCultureIgnoreCase) ||
              t.Author.Contains(
                searchTextbox.Text,
                StringComparison.CurrentCultureIgnoreCase) ||
              (t.Description?.Contains(
                searchTextbox.Text,
                comparisonType: StringComparison.CurrentCultureIgnoreCase) ?? false)
            )
            .ToArray()
          );
        }
        else
        {
          UpdateThreads(Threads);
        }
      };
      topGroup.Controls.Add(newThreadBtn);
      newThreadBtn.Click += (sender, args) =>
      {
        new ThreadForm().Show();
      };

      Controls.Add(ThreadsPanel);

      Threads = Task.Run(FetchThreads).Result;
      UpdateThreads(Threads);
    }
    catch (Exception ex)
    {
      MessageBox.Show(ex.Message);
    }
  }

  public static string User = "";

  public static Thread[] Threads = [];

  public static async Task<Thread[]> FetchThreads()
  {
    using HttpResponseMessage response = await Http.GetAsync("thread");
    response.EnsureSuccessStatusCode();

    string json = await response.Content.ReadAsStringAsync();
    Thread[] threads = JsonSerializer
      .Deserialize<Thread[]>(json, JsonOptions)!;

    return threads;
  }

  public static void UpdateThreads(Thread[] threads)
  {
    ThreadsPanel.Controls.Clear();

    foreach (Thread thread in threads)
    {
      GroupBox threadGroup = new()
      {
        Height = 125,
        Width = 1920 / 2 - 35
      };
      ThreadsPanel.Controls.Add(threadGroup);
      threadGroup.Controls.Add(new Label()
      {
        Text = $"{thread.Replies.Count} repl{(thread.Replies.Count == 1 ? "y" : "ies")}",
        Dock = DockStyle.Top
      });
      threadGroup.Controls.Add(new Label()
      {
        Text = thread.Description,
        Dock = DockStyle.Top
      });
      threadGroup.Controls.Add(new Label()
      {
        Text = $"{thread.Author} - {RelativeTime(thread.CreatedAt)}",
        Dock = DockStyle.Top
      });
      threadGroup.Controls.Add(new Label()
      {
        Text = thread.Title,
        Dock = DockStyle.Top
      });
    }
  }

  public static string RelativeTime(DateTime dateTime)
  {
    TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);
    if (timeSpan.TotalDays >= 365 * 2)
    {
      return $"{timeSpan.TotalDays / 365:0} years ago";
    }
    if (timeSpan.TotalDays >= 30 * 2)
    {
      return $"{timeSpan.TotalDays / 30:0} months ago";
    }
    if (timeSpan.TotalDays >= 7 * 2)
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
    return "Now";
  }

  public static readonly HttpClient Http = new()
  {
    BaseAddress = new Uri("http://localhost:5190")
  };
  public static JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  readonly GroupBox topGroup = new()
  {
    Width = 1920 / 2,
    Height = 80,
    Location = new(0, 0)
  };
  readonly Label userLabel = new()
  {
    Text = "Nickname",
    Width = 120,
    Height = 20,
    Location = new(12, 17)
  };
  readonly TextBox userTextbox = new()
  {
    Width = 100,
    Height = 30,
    Location = new(15, 40)
  };
  readonly Label searchLabel = new()
  {
    Text = "Search",
    Width = 120,
    Height = 20,
    Location = new(277, 17)
  };
  readonly TextBox searchTextbox = new()
  {
    Width = 360,
    Height = 30,
    Location = new(280, 40)
  };
  readonly Button newThreadBtn = new()
  {
    Enabled = false,
    Text = "New Thread",
    Width = 120,
    Height = 30,
    Location = new(1920 / 2 - 120 - 15, 40)
  };
  static readonly FlowLayoutPanel ThreadsPanel = new()
  {
    Width = 1920 / 2,
    Height = 1080 - 110,
    Location = new(15, 90),
    FlowDirection = FlowDirection.TopDown,
  };
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

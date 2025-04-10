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
      FormClosed += (sender, args) =>
      {
        Application.Exit();
      };

      Controls.Add(topGroup);
      topGroup.Controls.Add(new Label()
      {
        Text = User,
        Location = new(20, 17)
      });
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
        Width = 1920 / 2 - 35,
        DataContext = thread
      };
      ThreadsPanel.Controls.Add(threadGroup);
      threadGroup.Controls.Add(new Label()
      {
        Text = thread.Title,
        AutoSize = true,
        Location = new(10, 0)
      });
      threadGroup.Controls.Add(new Label()
      {
        Text = $"{thread.Author} - {RelativeTime(thread.CreatedAt)}",
        AutoSize = true,
        Location = new(750, 0)
      });
      threadGroup.Controls.Add(new Label()
      {
        Text = thread.Description,
        AutoSize = true,
        Location = new(10, 30)
      });
      threadGroup.Controls.Add(new Label()
      {
        Text = $"{thread.Replies.Count} repl{(thread.Replies.Count == 1 ? "y" : "ies")}",
        AutoSize = true,
        Location = new(10, 95)
      });

      Button openButton = new()
      {
        Text = "Open",
        AutoSize = true,
        Location = new(840, 85)
      };
      threadGroup.Controls.Add(openButton);
      if (thread?.Author == User)
      {
        openButton.Location -= new Size(160, 0);
        Button editButton = new()
        {
          Text = "Edit",
          AutoSize = true,
          Location = new(760, 85)
        };
        editButton.Click += (sender, args) =>
        {
          new ThreadForm(thread).Show();
        };
        Button deleteButton = new()
        {
          Text = "Delete",
          AutoSize = true,
          Location = new(840, 85)
        };
        deleteButton.Click += async (sender, args) =>
        {
          DialogResult choice = MessageBox.Show(
            $"Are you sure you want to delete \"{thread.Title}\" and all replies under it?",
            "Confirm deletion",
            MessageBoxButtons.YesNo);

          if (choice == DialogResult.Yes)
          {
            using HttpResponseMessage response = await Http
              .DeleteAsync($"thread/{thread.Id}");

            response.EnsureSuccessStatusCode();

            Threads = await FetchThreads();
            UpdateThreads(Threads);
          }
        };
        threadGroup.Controls.Add(editButton);
        threadGroup.Controls.Add(deleteButton);
      }
    }
  }

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

  public static readonly HttpClient Http = new()
  {
    BaseAddress = new Uri("http://localhost:5190")
  };
  public static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  readonly GroupBox topGroup = new()
  {
    Width = 1920 / 2,
    Height = 80,
    Location = new(0, 0)
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
    Text = "New Thread",
    Width = 120,
    Height = 30,
    Location = new(20, 40)
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

using System.Text.Json;

namespace L04.T00.Client;

public partial class Threads : Form
{
  public Threads()
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
        Text = Program.User,
        Location = new(20, 17)
      });
      topGroup.Controls.Add(searchLabel);
      topGroup.Controls.Add(searchTextbox);
      searchTextbox.KeyUp += (sender, args) =>
      {
        if (searchTextbox.Text != "")
        {
          Update(Program.Threads
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
          Update(Program.Threads);
        }
      };
      topGroup.Controls.Add(newThreadBtn);
      newThreadBtn.Click += (sender, args) =>
      {
        new ThreadForm().Show();
      };

      Controls.Add(ThreadsPanel);

      Program.Threads = Task.Run(Fetch).Result;
      Update(Program.Threads);
    }
    catch (Exception ex)
    {
      MessageBox.Show(ex.Message);
    }
  }

  public static async Task<Thread[]> Fetch()
  {
    using HttpResponseMessage response = await Program.Http.GetAsync("thread");
    response.EnsureSuccessStatusCode();

    string json = await response.Content.ReadAsStringAsync();
    Thread[] threads = JsonSerializer
      .Deserialize<Thread[]>(json, Program.JsonOptions)!;

    return threads;
  }

  public static void Update(Thread[] threads)
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
        Text = $"{thread.Author} - {Program.RelativeTime(thread.CreatedAt)}",
        AutoSize = true,
        Location = new(770, 0)
      });
      threadGroup.Controls.Add(new Label()
      {
        Text = thread.Description,
        AutoSize = true,
        Location = new(10, 30)
      });

      Button viewRepliesBtn = new()
      {
        Text = $"View {thread.Replies.Count} repl{(thread.Replies.Count == 1 ? "y" : "ies")}",
        AutoSize = true,
        Location = new(10, 85)
      };
      threadGroup.Controls.Add(viewRepliesBtn);
      if (thread?.Author == Program.User)
      {
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
            using HttpResponseMessage response = await Program.Http
              .DeleteAsync($"thread/{thread.Id}");

            response.EnsureSuccessStatusCode();

            Program.Threads = await Fetch();
            Update(Program.Threads);
          }
        };
        threadGroup.Controls.Add(editButton);
        threadGroup.Controls.Add(deleteButton);
      }
    }
  }

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

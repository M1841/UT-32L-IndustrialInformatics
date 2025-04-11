using System.Text.Json;

namespace L04.T00.Client;

public partial class Replies : Form
{
  public Replies(Guid threadId)
  {
    ThreadId = threadId;
    try
    {
      InitializeComponent();

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
          Update(Program.Replies
            .Where(t =>
              t.Content.Contains(
                searchTextbox.Text,
                StringComparison.CurrentCultureIgnoreCase) ||
              t.Author.Contains(
                searchTextbox.Text,
                StringComparison.CurrentCultureIgnoreCase)
            )
            .ToArray()
          );
        }
        else
        {
          Update(Program.Replies);
        }
      };
      topGroup.Controls.Add(newReplyBtn);
      newReplyBtn.Click += (sender, args) =>
      {
        new ReplyForm(ThreadId).Show();
      };

      Controls.Add(RepliesPanel);

      Program.Replies = Task.Run(Fetch).Result;
      Update(Program.Replies);
    }
    catch (Exception ex)
    {
      MessageBox.Show(ex.Message);
    }
  }

  public Guid ThreadId;

  public async Task<Reply[]> Fetch()
  {
    using HttpResponseMessage response = await Program.Http.GetAsync($"reply/{ThreadId}");
    response.EnsureSuccessStatusCode();

    string json = await response.Content.ReadAsStringAsync();
    Reply[] replies = JsonSerializer
      .Deserialize<Reply[]>(json, Program.JsonOptions)!;

    return replies;
  }

  public void Update(Reply[] replies)
  {
    RepliesPanel.Controls.Clear();

    foreach (Reply reply in replies)
    {
      GroupBox replyGroup = new()
      {
        Height = 125,
        Width = 1920 / 2 - 35,
        DataContext = reply
      };
      RepliesPanel.Controls.Add(replyGroup);
      replyGroup.Controls.Add(new Label()
      {
        Text = $"{reply.Author} - {Program.RelativeTime(reply.CreatedAt)}",
        AutoSize = true,
        Location = new(10, 0)
      });
      if (reply.UpdatedAt != null)
      {
        replyGroup.Controls.Add(new Label()
        {
          Text = $"Updated {Program.RelativeTime(reply.UpdatedAt ?? reply.CreatedAt)}",
          AutoSize = true,
          Location = new(770, 0)
        });
      }
      replyGroup.Controls.Add(new Label()
      {
        Text = reply.Content,
        AutoSize = true,
        Location = new(10, 30)
      });

      if (reply?.Author == Program.User)
      {
        Button editButton = new()
        {
          Text = "Edit",
          AutoSize = true,
          Location = new(760, 85)
        };
        editButton.Click += (sender, args) =>
        {
          new ReplyForm(reply).Show();
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
            $"Are you sure you want to delete this reply?",
            "Confirm deletion",
            MessageBoxButtons.YesNo);

          if (choice == DialogResult.Yes)
          {
            using HttpResponseMessage response = await Program.Http
              .DeleteAsync($"reply/{reply.Id}");

            response.EnsureSuccessStatusCode();

            replies = await Fetch();
            Update(replies);

            Program.Threads = await Threads.Fetch();
            Program.ThreadsWindow!.Update(Program.Threads);
          }
        };
        replyGroup.Controls.Add(editButton);
        replyGroup.Controls.Add(deleteButton);
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
  readonly Button newReplyBtn = new()
  {
    Text = "New Reply",
    Width = 120,
    Height = 30,
    Location = new(20, 40)
  };
  readonly FlowLayoutPanel RepliesPanel = new()
  {
    Width = 1920 / 2,
    Height = 1080 - 110,
    Location = new(15, 90),
    FlowDirection = FlowDirection.TopDown,
  };

  private System.ComponentModel.Container? components = null;
  protected override void Dispose(bool disposing)
  {
    if (disposing && (components != null))
    {
      components.Dispose();
    }
    base.Dispose(disposing);
  }
  private void InitializeComponent()
  {
    components = new System.ComponentModel.Container();
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new System.Drawing.Size(960, 1080);
    Text = "Replies";
  }
}

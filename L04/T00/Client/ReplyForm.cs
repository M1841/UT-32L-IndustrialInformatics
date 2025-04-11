using System.Text;
using System.Text.Json;

namespace L04.T00.Client;

public partial class ReplyForm : Form
{
  public ReplyForm(Guid threadId)
  {
    InitializeComponent();
    AddControls();
    submitBtn.Text = "Add";
    submitBtn.Click += async (sender, args) =>
    {
      if (contentTextbox.Text != "")
      {
        using StringContent json = new(
          JsonSerializer.Serialize(new ReplyCreateDto(
            threadId,
            Program.User,
            contentTextbox.Text)
          ),
          Encoding.UTF8,
          "application/json");

        using HttpResponseMessage response = await Program.Http
          .PostAsync("reply", json);

        response.EnsureSuccessStatusCode();

        Program.Replies = await Program.RepliesWindow!.Fetch();
        Program.RepliesWindow.Update(Program.Replies);

        Program.Threads = await Threads.Fetch();
        Program.ThreadsWindow!.Update(Program.Threads);

        Close();
      }
    };
  }
  public ReplyForm(Reply reply)
  {
    InitializeComponent();
    AddControls();
    submitBtn.Text = "Edit";
    submitBtn.Enabled = true;
    contentTextbox.Text = reply.Content;
    submitBtn.Click += async (sender, args) =>
    {
      if (contentTextbox.Text != "")
      {
        using StringContent json = new(
          JsonSerializer.Serialize(new ReplyUpdateDto(
            contentTextbox.Text)
          ),
          Encoding.UTF8,
          "application/json");

        using HttpResponseMessage response = await Program.Http
          .PutAsync($"reply/{reply.Id}", json);

        response.EnsureSuccessStatusCode();

        Program.Replies = await Program.RepliesWindow!.Fetch();
        Program.RepliesWindow.Update(Program.Replies);

        Close();
      }
    };
  }

  void AddControls()
  {
    Controls.Add(contentLabel);
    Controls.Add(contentTextbox);
    contentTextbox.KeyUp += (sender, args) =>
    {
      submitBtn.Enabled = contentTextbox.Text != "";
    };
    Controls.Add(submitBtn);
  }

  readonly Label contentLabel = new()
  {
    Text = "Content",
    Location = new(10, 10)
  };
  readonly TextBox contentTextbox = new()
  {
    Height = 70,
    Width = 600 - 30,
    Location = new(15, 35),
    Multiline = true
  };
  readonly Button submitBtn = new()
  {
    Enabled = false,
    Height = 30,
    Width = 600 - 30,
    Location = new(15, 130),
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
    ClientSize = new Size(600, 180);
    Text = "Reply Form";
  }
}
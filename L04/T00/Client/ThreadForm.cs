using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace L04.T00.Client;

public partial class ThreadForm : Form
{
  public ThreadForm()
  {
    InitializeComponent();
    AddControls();
    submitBtn.Text = "Add";
    submitBtn.Click += async (sender, args) =>
    {
      if (titleTextbox.Text != "")
      {
        using StringContent json = new(
          JsonSerializer.Serialize(new ThreadCreateDto(
            Forum.User,
            titleTextbox.Text,
            descriptionTextbox.Text)
          ),
          Encoding.UTF8,
          "application/json");

        using HttpResponseMessage response = await Forum.Http
          .PostAsync("thread", json);

        response.EnsureSuccessStatusCode();

        Forum.Threads = await Forum.FetchThreads();
        Forum.UpdateThreads(Forum.Threads);

        Close();
      }
    };
  }
  public ThreadForm(Thread thread)
  {
    InitializeComponent();
    submitBtn.Text = "Edit";
    AddControls();
  }

  void AddControls()
  {
    Controls.Add(titleLabel);
    Controls.Add(titleTextbox);
    titleTextbox.KeyUp += (sender, args) =>
    {
      submitBtn.Enabled = titleTextbox.Text != "";
    };
    Controls.Add(descriptionLabel);
    Controls.Add(descriptionTextbox);
    Controls.Add(submitBtn);
  }

  readonly Label titleLabel = new()
  {
    Text = "Title",
    Location = new(10, 10)
  };
  readonly TextBox titleTextbox = new()
  {
    Width = 600 - 30,
    Location = new(15, 35)
  };
  readonly Label descriptionLabel = new()
  {
    Text = "Description (Optional)",
    Location = new(10, 80)
  };
  readonly TextBox descriptionTextbox = new()
  {
    Height = 100,
    Width = 600 - 30,
    Location = new(15, 105),
    Multiline = true
  };
  readonly Button submitBtn = new()
  {
    Enabled = false,
    Height = 30,
    Width = 600 - 30,
    Location = new(15, 230),
  };
}
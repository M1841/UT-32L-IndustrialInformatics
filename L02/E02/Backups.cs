using System.Text.Json;

namespace L02.E02
{
  public partial class Backups : Form
  {
    public Backups()
    {
      exitButton.Click += HandleExit;
      copyButton.Click += HandleCopy;
      deleteButton.Click += HandleDelete;

      Controls.Add(listBox1);
      Controls.Add(listBox2);
      Controls.Add(copyButton);
      Controls.Add(deleteButton);
      Controls.Add(exitButton);

      Controls.Add(new Label()
      {
        Text = "Local",
        Location = new Point(50, 20),
      });
      Controls.Add(new Label()
      {
        Text = "Cloud",
        Location = new Point(390, 20),
      });

      string content = File.ReadAllText("items.json");
      string[] items = JsonSerializer.Deserialize<string[]>(content) ?? [];
      listBox1.BeginUpdate();
      foreach (string item in items)
      {
        listBox1.Items.Add(item);
      }
      listBox1.EndUpdate();

      InitializeComponent();
    }

    private readonly ListBox listBox1 = new()
    {
      Location = new Point(50, y: 50),
      Width = 160,
      Height = 140,
      SelectionMode = SelectionMode.MultiExtended
    };
    private readonly ListBox listBox2 = new()
    {
      Location = new Point(390, y: 50),
      Width = 160,
      Height = 140,
      SelectionMode = SelectionMode.MultiExtended
    };
    private readonly Button copyButton = new()
    {
      Text = "Copy",
      Location = new Point(260, 50),
      AutoSize = true,
      FlatStyle = FlatStyle.Popup,
    };
    private readonly Button deleteButton = new()
    {
      Text = "Delete",
      Location = new Point(260, 100),
      AutoSize = true,
      FlatStyle = FlatStyle.Popup,
    };
    private readonly Button exitButton = new()
    {
      Text = "Exit",
      Location = new Point(260, 150),
      AutoSize = true,
      FlatStyle = FlatStyle.Popup,
    };

    public static void HandleExit(object? sender, EventArgs e)
    {
      Application.Exit();
    }

    public void HandleCopy(object? sender, EventArgs e)
    {
      listBox2.BeginUpdate();
      foreach (string item in listBox1.SelectedItems)
      {
        listBox2.Items.Add(item);
      }
      listBox2.EndUpdate();
    }

    public void HandleDelete(object? sender, EventArgs e)
    {
      listBox2.BeginUpdate();
      listBox2.Items.Clear();
      listBox2.EndUpdate();
    }
  }
}

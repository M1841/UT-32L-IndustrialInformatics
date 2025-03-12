using System.Text.Json;

namespace Ex02
{
  public partial class Form1 : Form
  {
    public Form1()
    {
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

    public static void HandleExit(object sender, EventArgs e)
    {
      Application.Exit();
    }

    public void HandleCopy(object sender, EventArgs e)
    {
      listBox2.BeginUpdate();
      foreach (string item in listBox1.SelectedItems)
      {
        listBox2.Items.Add(item);
      }
      listBox2.EndUpdate();
    }

    public void HandleDelete(object sender, EventArgs e)
    {
      listBox2.BeginUpdate();
      listBox2.Items.Clear();
      listBox2.EndUpdate();
    }
  }
}

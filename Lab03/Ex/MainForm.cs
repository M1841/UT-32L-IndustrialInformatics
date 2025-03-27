using Microsoft.EntityFrameworkCore;

namespace Ex
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      // AppDbContext.SeedData();

      using AppDbContext db = new();
      InitializeComponent();

      Controls.Add(leftGroup);
      foreach (var university in db.Universities.Include(u => u.Faculties).OrderBy(u => u.Name))
      {
        universitiesList.Items.Add(university);
      }
      universitiesList.Click += (sender, args) =>
      {
        if (universitiesList.SelectedItem != null)
        {
          University university = (University)universitiesList.SelectedItem!;
          facultiesList.Items.Clear();
          foreach (var faculty in university.Faculties.OrderBy(f => f.Name))
          {
            facultiesList.Items.Add(faculty);
          }
          codeTextbox.Text = university.Code.ToString();
          cityTextbox.Text = university.City;
        }
      };
      leftGroup.Controls.Add(universitiesList);
      leftGroup.Controls.Add(universitiesLabel);
      leftGroup.Controls.Add(buttonGroup);
      buttonGroup.Controls.Add(deleteButton);
      buttonGroup.Controls.Add(editButton);
      buttonGroup.Controls.Add(addButton);

      Controls.Add(rightGroup);
      rightGroup.Controls.Add(cityTextbox);
      rightGroup.Controls.Add(cityLabel);
      rightGroup.Controls.Add(codeTextbox);
      rightGroup.Controls.Add(codeLabel);
      rightGroup.Controls.Add(facultiesList);
      rightGroup.Controls.Add(facultiesLabel);
      rightGroup.Controls.Add(facultiesTableButton);
    }

    readonly GroupBox leftGroup = new()
    {
      Dock = DockStyle.Left,
      Width = 400
    };
    readonly ListBox universitiesList = new()
    {
      Dock = DockStyle.Top,
      Height = 350
    };
    readonly Label universitiesLabel = new()
    {
      Text = "Universities",
      Dock = DockStyle.Top
    };
    readonly GroupBox buttonGroup = new()
    {
      Dock = DockStyle.Bottom,
      Height = 70
    };
    readonly Button addButton = new()
    {
      Text = "Add",
      Dock = DockStyle.Left,
      Width = 129
    };
    readonly Button editButton = new()
    {
      Text = "Edit",
      Dock = DockStyle.Left,
      Width = 129
    };
    readonly Button deleteButton = new()
    {
      Text = "Delete",
      Dock = DockStyle.Left,
      Width = 129
    };
    readonly GroupBox rightGroup = new()
    {
      Dock = DockStyle.Right,
      Width = 400
    };
    readonly TextBox cityTextbox = new()
    {
      ReadOnly = true,
      Dock = DockStyle.Top
    };
    readonly Label cityLabel = new()
    {
      Text = "City",
      Dock = DockStyle.Top
    };
    readonly TextBox codeTextbox = new()
    {
      ReadOnly = true,
      Dock = DockStyle.Top
    };
    readonly Label codeLabel = new()
    {
      Text = "Code",
      Dock = DockStyle.Top
    };
    readonly ListBox facultiesList = new()
    {
      Dock = DockStyle.Top,
      Height = 250
    };
    readonly Label facultiesLabel = new()
    {
      Text = "Faculties",
      Dock = DockStyle.Top
    };
    readonly Button facultiesTableButton = new()
    {
      Text = "Faculties Table",
      Dock = DockStyle.Bottom,
      Height = 45
    };
  }
}

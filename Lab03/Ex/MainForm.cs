using Microsoft.EntityFrameworkCore;

namespace Ex
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      AppDbContext.SeedData();
      InitializeComponent();

      Controls.Add(leftGroup);
      leftGroup.Controls.Add(cityTextbox);
      leftGroup.Controls.Add(cityLabel);
      leftGroup.Controls.Add(codeTextbox);
      leftGroup.Controls.Add(codeLabel);
      using (AppDbContext db = new())
      {
        foreach (var university in db.Universities
          .Include(u => u.Faculties)
          .OrderBy(u => u.Name))
        {
          universitiesList.Items.Add(university);
        }
      }
      universitiesList.Click += (sender, args) =>
      {
        if (universitiesList.SelectedItem is University university)
        {
          facultiesList.Items.Clear();
          foreach (var faculty in university.Faculties
            .OrderBy(f => f.Name))
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
      deleteButton.Click += (sender, args) =>
      {
        if (universitiesList.SelectedItem is University university)
        {
          DialogResult messageBoxResult = MessageBox.Show(
            $"Are you sure you want to delete \"{university.Name}\"?",
            "Confirm deletion",
            MessageBoxButtons.YesNo);

          if (messageBoxResult == DialogResult.Yes)
          {
            using AppDbContext db = new();
            db.Remove(university);
            db.SaveChanges();
            universitiesList.Items.Remove(university);
          }
        }
      };
      editButton.Click += (sender, args) =>
      {
        if (universitiesList.SelectedItem is University university)
        {
          UniversityForm editForm = new(university);
          editForm.Show();
        }
      };
      addButton.Click += (sender, args) =>
      {
        UniversityForm addForm = new();
        addForm.Show();
      };
      buttonGroup.Controls.Add(deleteButton);
      buttonGroup.Controls.Add(editButton);
      buttonGroup.Controls.Add(addButton);

      Controls.Add(rightGroup);
      rightGroup.Controls.Add(facultiesList);
      rightGroup.Controls.Add(facultiesLabel);
      facultiesTableButton.Click += (sender, args) =>
      {
        FacultiesForm facultiesForm = new();
        facultiesForm.Show();
      };
      rightGroup.Controls.Add(facultiesTableButton);
    }

    readonly GroupBox leftGroup = new()
    {
      Dock = DockStyle.Left,
      Width = 400
    };
    public static readonly ListBox universitiesList = new()
    {
      Dock = DockStyle.Top,
      Height = 250
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
    public static readonly ListBox facultiesList = new()
    {
      Dock = DockStyle.Top,
      Height = 350
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

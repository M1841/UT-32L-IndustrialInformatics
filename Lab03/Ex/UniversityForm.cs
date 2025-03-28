using Microsoft.EntityFrameworkCore;

namespace Ex
{
  public partial class UniversityForm : Form
  {
    public UniversityForm()
    {
      addButton.Text = "Add";
      addButton.Click += (sender, args) =>
      {
        if (nameTextbox.Text != "" && cityTextbox.Text != "")
        {
          University university = new()
          {
            Name = nameTextbox.Text,
            City = cityTextbox.Text
          };

          using AppDbContext db = new();
          db.Add(university);
          db.SaveChanges();

          FacultiesForm.facultiesSource.DataSource = db.Faculties.Include(f => f.University);
          FacultiesForm.facultiesGridView.DataSource = FacultiesForm.facultiesSource;
          MainForm.universitiesList.Items.Add(university);
          Close();
        }
      };
      InitializeComponent();
      AddControls();
    }
    public UniversityForm(University university)
    {
      InitializeComponent();
      nameTextbox.Text = university.Name;
      cityTextbox.Text = university.City;
      addButton.Text = "Edit";
      addButton.Click += (sender, args) =>
      {
        if (nameTextbox.Text != "" && cityTextbox.Text != "")
        {
          int index = MainForm.universitiesList.Items.IndexOf(university);
          MainForm.universitiesList.Items.Remove(university);

          university.Name = nameTextbox.Text;
          university.City = cityTextbox.Text;

          using AppDbContext db = new();
          db.Update(university);
          db.SaveChanges();

          FacultiesForm.facultiesSource.DataSource = db.Faculties.Include(f => f.University);
          FacultiesForm.facultiesGridView.DataSource = FacultiesForm.facultiesSource;
          MainForm.universitiesList.Items.Insert(index, university);
          Close();
        }
      };
      AddControls();
    }

    void AddControls()
    {
      Padding = new Padding(10, 10, 10, 10);
      nameTextbox.KeyUp += HandleEdit;
      cityTextbox.KeyUp += HandleEdit;

      Controls.Add(nameLabel);
      Controls.Add(nameTextbox);
      Controls.Add(cityLabel);
      Controls.Add(cityTextbox);
      Controls.Add(addButton);
    }

    void HandleEdit(object? sender, KeyEventArgs args)
    {
      addButton.Enabled = nameTextbox.Text != "" && cityTextbox.Text != "";
    }

    readonly Label nameLabel = new()
    {
      Text = "Name",
      Dock = DockStyle.Bottom
    };
    readonly TextBox nameTextbox = new()
    {
      Dock = DockStyle.Bottom
    };
    readonly Label cityLabel = new()
    {
      Text = "City",
      Dock = DockStyle.Bottom
    };
    readonly TextBox cityTextbox = new()
    {
      Dock = DockStyle.Bottom
    };
    readonly Button addButton = new()
    {
      Dock = DockStyle.Bottom,
      Height = 50,
      Enabled = false
    };
  }
}

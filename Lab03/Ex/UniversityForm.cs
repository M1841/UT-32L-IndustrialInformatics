namespace Ex
{
  public partial class UniversityForm : Form
  {
    public UniversityForm()
    {
      button.Text = "Add";
      button.Click += (sender, args) =>
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
      button.Text = "Edit";
      button.Click += (sender, args) =>
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

          MainForm.universitiesList.Items.Insert(index, university);
          Close();
        }
      };
      AddControls();
    }

    void AddControls()
    {
      Controls.Add(nameLabel);
      Controls.Add(nameTextbox);
      Controls.Add(cityLabel);
      Controls.Add(cityTextbox);
      Controls.Add(button);
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
    readonly Button button = new()
    {
      Dock = DockStyle.Bottom,
      Height = 50
    };
  }
}

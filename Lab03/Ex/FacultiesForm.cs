using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Ex
{
  public partial class FacultiesForm : Form
  {
    public FacultiesForm()
    {
      InitializeComponent();

      using (AppDbContext db = new())
      {
        facultiesSource.DataSource = db.Faculties.Include(f => f.University);
        facultiesGridView.DataSource = facultiesSource;
      }

      facultiesGridView.DataBindingComplete += (sender, args) =>
      {
        facultiesGridView.Columns[nameof(Faculty.Id)]!.ReadOnly = true;
        facultiesGridView.Columns[nameof(Faculty.Code)]!.ReadOnly = true;
        facultiesGridView.Columns[nameof(Faculty.University)]!.ReadOnly = true;
      };

      facultiesGridView.UserDeletingRow += (sender, args) =>
      {
        int? id = (int?)args.Row!.Cells[nameof(Faculty.Id)].Value;
        if (id != null)
        {
          using AppDbContext db = new();
          Faculty faculty = db.Faculties
            .Where(f => f.Id == id)
            .First();

          DialogResult messageBoxResult = MessageBox.Show(
            $"Are you sure you want to delete \"{faculty.Name}\"?",
            "Confirm deletion",
            MessageBoxButtons.YesNo);

          if (messageBoxResult == DialogResult.Yes)
          {
            db.Remove(faculty);
            db.SaveChanges();

            foreach (University university in MainForm.universitiesList.Items)
            {
              if (
                university.Code == faculty.Code &&
                university.Faculties
                  .Where(f => f.Id == faculty.Id)
                  .FirstOrDefault() is Faculty oldFaculty)
              {
                university.Faculties.Remove(oldFaculty);
                break;
              }
            }
            foreach (Faculty oldFaculty in MainForm.facultiesList.Items)
            {
              if (oldFaculty.Id == faculty.Id)
              {
                MainForm.facultiesList.Items.Remove(oldFaculty);
                break;
              }
            }
          }
          else
          {
            args.Cancel = true;
          }
        }
      };
      facultiesGridView.CellValueChanged += (sender, args) =>
      {
        if (args.ColumnIndex == 1 && facultiesSource[args.RowIndex] is Faculty faculty)
        {
          using AppDbContext db = new();
          db.Update(faculty);
          db.SaveChanges();

          foreach (University university in MainForm.universitiesList.Items)
          {
            if (
              university.Code == faculty.Code &&
              university.Faculties
                .Where(f => f.Id == faculty.Id)
                .FirstOrDefault() is Faculty oldFaculty)
            {
              university.Faculties.Clear();
              foreach (Faculty f in db.Faculties
                .Where(f => f.Code == university.Code)
                .OrderBy(f => f.Name))
              {
                university.Faculties.Add(f);
              }
            }
          }
          foreach (Faculty oldFaculty in MainForm.facultiesList.Items)
          {
            if (oldFaculty.Id == faculty.Id)
            {
              int index = MainForm.facultiesList.Items.IndexOf(oldFaculty);
              MainForm.facultiesList.Items.Remove(oldFaculty);
              MainForm.facultiesList.Items.Insert(index, faculty);
              break;
            }
          }
        }
      };
      Controls.Add(facultiesGridView);
      Controls.Add(inputGroup);

      nameTextbox.KeyUp += (sender, args) =>
      {
        isNameValid = nameTextbox.Text != "";
        addButton.Enabled = isNameValid && isCodeValid;
      };
      codeTextbox.KeyUp += (sender, args) =>
      {
        if (int.TryParse(codeTextbox.Text, out int code))
        {
          using AppDbContext db = new();
          if (db.Universities.Where(u => u.Code == code).FirstOrDefault()
              is University university)
          {
            universityTextbox.Text = university.Name;
            isCodeValid = true;
            this.university = university;
          }
          else
          {
            universityTextbox.Text = "???";
            isCodeValid = false;
            this.university = null;
          }
        }
        else
        {
          universityTextbox.Text = "???";
          isCodeValid = false;
          university = null;
        }
        addButton.Enabled = isNameValid && isCodeValid;
      };
      addButton.Click += (sender, args) =>
      {
        if (university != null)
        {
          Faculty faculty = new() { Name = nameTextbox.Text };
          university.Faculties.Add(faculty);

          using AppDbContext db = new();
          db.Update(university);
          db.SaveChanges();

          facultiesSource.DataSource = db.Faculties.Include(f => f.University);
          facultiesGridView.DataSource = facultiesSource;

          foreach (University university in MainForm.universitiesList.Items)
          {
            if (university.Code == this.university.Code)
            {
              university.Faculties.Add(faculty);
              break;
            }
          }
          if (
            MainForm.universitiesList.SelectedItem is University selectedUniversity &&
            selectedUniversity.Code == university.Code)
          {
            MainForm.facultiesList.Items.Add(faculty);
          }
          nameTextbox.Text = "";
          codeTextbox.Text = "";
          universityTextbox.Text = "";
          addButton.Enabled = false;
        }
      };

      inputGroup.Controls.Add(nameTextbox);
      inputGroup.Controls.Add(codeTextbox);
      inputGroup.Controls.Add(universityTextbox);
      inputGroup.Controls.Add(addButton);
    }

    bool isNameValid = false;
    bool isCodeValid = false;
    University? university = null;

    public static readonly DataGridView facultiesGridView = new()
    {
      Dock = DockStyle.Fill,
      AutoGenerateColumns = true,
      AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
      AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
      EditMode = DataGridViewEditMode.EditOnF2,
    };
    public static readonly BindingSource facultiesSource = new();
    readonly GroupBox inputGroup = new()
    {
      Dock = DockStyle.Bottom,
      Height = 60
    };
    readonly TextBox nameTextbox = new()
    {
      Location = new Point(102, 20),
      Width = 398,
      PlaceholderText = "Name"
    };
    readonly TextBox codeTextbox = new()
    {
      Location = new Point(500, 20),
      Width = 72,
      PlaceholderText = "Code"
    };
    readonly TextBox universityTextbox = new()
    {
      Location = new Point(572, 20),
      Width = 264,
      PlaceholderText = "University",
      ReadOnly = true,
      Enabled = false
    };
    readonly Button addButton = new()
    {
      Text = "Add",
      Location = new Point(20, 20),
      Height = 30,
      Enabled = false
    };
  }
}

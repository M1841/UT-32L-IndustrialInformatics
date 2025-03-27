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

            MainForm.universitiesList.Items.Clear();
            MainForm.facultiesList.Items.Clear();
            foreach (University university in db.Universities
              .Include(u => u.Faculties)
              .OrderBy(u => u.Name))
            {
              MainForm.universitiesList.Items.Add(university);
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

          MainForm.universitiesList.Items.Clear();
          MainForm.facultiesList.Items.Clear();
          foreach (University university in db.Universities
            .Include(u => u.Faculties)
            .OrderBy(u => u.Name))
          {
            MainForm.universitiesList.Items.Add(university);
          }
        }
      };
      Controls.Add(facultiesGridView);
    }

    readonly DataGridView facultiesGridView = new()
    {
      Dock = DockStyle.Fill,
      AutoGenerateColumns = true,
      AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
      AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
      EditMode = DataGridViewEditMode.EditOnF2,
    };
    readonly BindingSource facultiesSource = new();
  }
}

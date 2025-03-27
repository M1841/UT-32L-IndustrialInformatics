using Microsoft.EntityFrameworkCore;

namespace Ex
{
  public partial class FacultiesForm : Form
  {
    public FacultiesForm()
    {
      InitializeComponent();
      facultiesGridView.ColumnCount = 3;
      facultiesGridView.Columns[0].Name = "ID";
      facultiesGridView.Columns[0].Width = 30;
      facultiesGridView.Columns[1].Name = "Name";
      facultiesGridView.Columns[1].Width = 400;
      facultiesGridView.Columns[2].Name = "University";
      facultiesGridView.Columns[2].Width = 290;
      using (AppDbContext db = new())
      {
        foreach (Faculty faculty in db.Faculties.Include(f => f.University))
        {
          facultiesGridView.Rows.Add([
            faculty.Id,
            faculty.Name,
            faculty.University?.Name ?? "-"
          ]);
        }
      }
      Controls.Add(facultiesGridView);
    }

    readonly DataGridView facultiesGridView = new()
    {
      Dock = DockStyle.Fill
    };
  }
}

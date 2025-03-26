using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Ex
{
  public class AppDbContext : DbContext
  {
    public DbSet<University> Universities { get; set; }
    public DbSet<Faculty> Faculties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
      => options.UseSqlite("DataSource=Lab03Ex.db;cache=shared");

    public static void SeedData()
    {
      using AppDbContext db = new();
      if (!db.Database.EnsureCreated()) { return; }

      University UTCN = new()
      {
        Name = "Universitatea Tehnică din Cluj-Napoca",
        City = "Cluj-Napoca"
      };
      UTCN.Faculties.Add(new() { Name = "Automatică și Calculatoare" });
      UTCN.Faculties.Add(new() { Name = "Electronică, Telecomunicații și Tehnologia Informației" });

      University UPB = new()
      {
        Name = "Universitatea Politehnică București",
        City = "București"
      };
      UPB.Faculties.Add(new() { Name = "Ingierie Electrică" });
      UPB.Faculties.Add(new() { Name = "Inginerie Mecanică și Mecatronică" });

      db.Add(UTCN);
      db.Add(UPB);
      db.SaveChanges();
    }
  }

  public class University
  {
    public int Id { get; set; }
    public required string Name { get; set; } = "";
    public required string City { get; set; } = "";

    [Key]
    public int Code { get; set; }

    public ICollection<Faculty> Faculties { get; set; } = [];

    public override string ToString()
    {
      return Name;
    }
  }

  public class Faculty
  {
    public int Id { get; set; }
    public required string Name { get; set; }

    public int Code { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(Code))]
    public University? University { get; set; }

    public override string ToString()
    {
      return Name;
    }
  }
}

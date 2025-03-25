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
      using var db = new AppDbContext();
      if (!db.Database.EnsureCreated()) { return; }

      University UTCN = new()
      {
        Name = "UTCN",
        City = "Cluj-Napoca"
      };
      UTCN.Faculties.Add(new() { Name = "AC" });
      UTCN.Faculties.Add(new() { Name = "ETTI" });

      University UPB = new()
      {
        Name = "UPB",
        City = "București"
      };
      UPB.Faculties.Add(new() { Name = "IE" });
      UPB.Faculties.Add(new() { Name = "IMM" });

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
  }

  public class Faculty
  {
    public int Id { get; set; }
    public required string Name { get; set; }

    public int Code { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(Code))]
    public University? University { get; set; }
  }
}

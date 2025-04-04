using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace L03
{
  public class AppDbContext : DbContext
  {
    public DbSet<University> Universities { get; set; }
    public DbSet<Faculty> Faculties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
      // => options.UseSqlite("DataSource=II_L03.sqlite;cache=shared");
      => options.UseSqlServer("Server=localhost;Database=II_L03;Trusted_Connection=True;Encrypt=False");

    public static void SeedData()
    {
      using AppDbContext db = new();
      if (!db.Database.EnsureCreated()) { return; }

      University UTCN = new()
      {
        Name = "Universitatea Tehnică din Cluj-Napoca",
        City = "Cluj-Napoca"
      };
      UTCN.Faculties.Add(new() { Name = "Arhitectură și Urbanism" });
      UTCN.Faculties.Add(new() { Name = "Automatică și Calculatoare" });
      UTCN.Faculties.Add(new() { Name = "Autovehicule Rutiere, Mecatronică și Mecanică" });
      UTCN.Faculties.Add(new() { Name = "Construcții" });
      UTCN.Faculties.Add(new() { Name = "Electronică, Telecomunicații și Tehnologia Informației" });
      UTCN.Faculties.Add(new() { Name = "Ingineria Materialelor și a Mediului" });
      UTCN.Faculties.Add(new() { Name = "Ingineria Instalațiilor" });
      UTCN.Faculties.Add(new() { Name = "Inginerie Electrică" });
      UTCN.Faculties.Add(new() { Name = "Inginerie Industrială, Robotică și Managementul Producției" });

      University UPB = new()
      {
        Name = "Universitatea Politehnică București",
        City = "București"
      };
      UPB.Faculties.Add(new() { Name = "Ingierie Electrică" });
      UPB.Faculties.Add(new() { Name = "Inginerie Industrială și Robotică" });
      UPB.Faculties.Add(new() { Name = "Inginerie Chimică și Biotehnologii" });
      UPB.Faculties.Add(new() { Name = "Energetică" });
      UPB.Faculties.Add(new() { Name = "Ingineria Sistemelor Biotehnice" });
      UPB.Faculties.Add(new() { Name = "Inginerie în Limbi Străine" });
      UPB.Faculties.Add(new() { Name = "Automatică și Calculatoare" });
      UPB.Faculties.Add(new() { Name = "Transporturi" });
      UPB.Faculties.Add(new() { Name = "Științe Aplicate" });
      UPB.Faculties.Add(new() { Name = "Electronică, Telecomunicații și Tehnologia Informației" });
      UPB.Faculties.Add(new() { Name = "Inginerie Aerospațială" });
      UPB.Faculties.Add(new() { Name = "Inginerie Medicală" });
      UPB.Faculties.Add(new() { Name = "Inginerie Mecanică și Mecatronică" });
      UPB.Faculties.Add(new() { Name = "Știința și Ingineria Materialelor" });
      UPB.Faculties.Add(new() { Name = "Antreprenoriat, Ingineria și Managementul Afacerilor" });

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

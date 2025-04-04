using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseHttpsRedirection();

var rnd = new Random();

app.MapGet(
  "FtoC",
  ([FromQuery] double degrees) => $"{(degrees - 32) / 1.8:0.##}"
);
app.MapGet(
  "CtoF",
  ([FromQuery] double degrees) => $"{degrees * 1.8 + 32:0.##}"
);

app.MapGet(
  "now",
  () => $"{DateTime.Now:dddd, d MMMM yyyy, h:mm:ss tt}"
);

app.MapGet(
  "numbers",
  () => new int[5].Select(
    _ => rnd.Next(100_000, 999_999))
);

app.MapGet(
  "RonToEur",
  ([FromQuery] double amount) => $"{amount / 4.97:0.##}"
);
app.MapGet(
  "EurToRon",
  ([FromQuery] double amount) => $"{amount * 4.97:0.##}"
);

app.Run();

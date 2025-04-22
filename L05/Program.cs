namespace L05;

public static class Program
{
  public static void Main(string[] args)
  {

    AppDbContext.SeedData();

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
      options.IdleTimeout = TimeSpan.MaxValue);

    builder.Services.AddControllersWithViews();

    builder.Services.AddScoped<AppDbContext>();

    var app = builder.Build();


    if (!app.Environment.IsDevelopment())
    {
      app.UseExceptionHandler("/Home/Error");
      app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseSession();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.Run();
  }

  public static string RelativeTime(DateTime dateTime)
  {
    TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);
    if (timeSpan.TotalDays > 365)
    {
      return $"{timeSpan.TotalDays / 365:0} years ago";
    }
    if (timeSpan.TotalDays > 30 * 2)
    {
      return $"{timeSpan.TotalDays / 30:0} months ago";
    }
    if (timeSpan.TotalDays > 7 * 2)
    {
      return $"{timeSpan.TotalDays / 7:0} weeks ago";
    }
    if (timeSpan.TotalDays > 2)
    {
      return $"{timeSpan.TotalDays:0} days ago";
    }
    if (timeSpan.TotalHours > 2)
    {
      return $"{timeSpan.TotalHours:0} hours ago";
    }
    if (timeSpan.TotalMinutes > 2)
    {
      return $"{timeSpan.TotalMinutes:0} minutes ago";
    }
    if (timeSpan.TotalSeconds > 2)
    {
      return $"{timeSpan.TotalSeconds:0} seconds ago";
    }
    return "Now";
  }
}

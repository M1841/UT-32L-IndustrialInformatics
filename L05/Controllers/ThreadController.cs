using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using L05.Models;

namespace L05.Controllers;

public class ThreadController(AppDbContext db) : Controller
{
  [HttpGet]
  public IActionResult Index([FromQuery] string? search)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["Title"] = "Threads";
    return View(new ThreadsPageViewModel(
      db.Threads
        .Include(t => t.Replies)
        .ToList()
        .Where(t =>
          search == null || search == "" ||
          t.Title.Contains(
            search,
            StringComparison.CurrentCultureIgnoreCase) ||
          t.Author.Contains(
            search,
            StringComparison.CurrentCultureIgnoreCase) ||
          (t.Description != null && t.Description!.Contains(
            search,
            StringComparison.CurrentCultureIgnoreCase))
        )
        .OrderByDescending(t => t.CreatedAt)
        .ToArray(),
      nickname,
      search ?? ""));
  }

  [HttpGet]
  public IActionResult Add()
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["Title"] = "Add Thread";
    return View(new ThreadCreateDto(
      "",
      "")
    );
  }

  [HttpGet]
  public IActionResult Edit([FromRoute] Guid id)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    Models.Thread? thread = db.Threads
      .FirstOrDefault(t => t.Id == id);

    if (thread == null) { return NotFound(); }
    if (nickname != thread.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["Title"] = "Edit Thread";
    return View(new ThreadUpdateDto(
      thread.Id,
      thread.Title,
      thread.Description)
    );
  }

  [HttpGet]
  public IActionResult Delete([FromRoute] Guid id)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }
    Models.Thread? thread = db.Threads
      .FirstOrDefault(t => t.Id == id);

    if (thread == null) { return NotFound(); }
    if (nickname != thread.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["Title"] = "Delete Thread";
    return View(new ThreadUpdateDto(
      thread.Id,
      thread.Title,
      thread.Description)
    );
  }

  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromForm] ThreadCreateDto dto)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }
    if (dto.Title == null)
    {
      return BadRequest("Title cannot be empty");
    }

    Models.Thread thread = new()
    {
      Author = nickname,
      Title = dto.Title,
      Description = dto.Description,
      CreatedAt = DateTime.Now
    };

    await db.Threads.AddAsync(thread);
    await db.SaveChangesAsync();

    return RedirectToAction("Index", "Thread");
  }

  [HttpPost]
  public async Task<IActionResult> UpdateAsync(
    [FromRoute] Guid id, [FromForm] ThreadUpdateDto dto)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    Models.Thread? thread = await db.Threads
      .FirstOrDefaultAsync(t => t.Id == id);

    if (thread == null) { return NotFound(); }
    if (nickname != thread.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    thread.Title = dto.Title ?? thread.Title;
    thread.Description = dto.Description ?? thread.Description;
    thread.UpdatedAt = DateTime.Now;

    await db.SaveChangesAsync();

    return RedirectToAction("Index", "Thread");
  }

  [HttpPost]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    Models.Thread? thread = await db.Threads
      .Include(t => t.Replies)
      .FirstOrDefaultAsync(t => t.Id == id);

    if (thread == null) { return NotFound(); }
    if (nickname != thread.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    db.Threads.Remove(thread);
    await db.SaveChangesAsync();

    return RedirectToAction("Index", "Thread");
  }
}

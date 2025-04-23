using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using L05.Models;

namespace L05.Controllers;

public class ReplyController(AppDbContext db) : Controller
{
  [HttpGet]
  public IActionResult Index([FromRoute] Guid id, [FromQuery] string? search)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    Models.Thread? thread = db.Threads
      .Include(t => t.Replies)
      .FirstOrDefault(t => t.Id == id);

    if (thread == null) { return NotFound(); }

    return View(new RepliesPageViewModel(
      thread.Replies.ToList()
        .Where(r =>
          search == null || search == "" ||
          r.Content.Contains(
            search,
            StringComparison.CurrentCultureIgnoreCase) ||
          r.Author.Contains(
            search,
            StringComparison.CurrentCultureIgnoreCase))
        .ToArray(),
      id,
      nickname,
      search ?? ""));
  }

  [HttpGet]
  public IActionResult Add([FromRoute] Guid id)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["Title"] = "Add Reply";
    return View(new ReplyCreateDto(
      id,
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

    Reply? reply = db.Replies
      .FirstOrDefault(r => r.Id == id);

    if (reply == null) { return NotFound(); }
    if (nickname != reply.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["Title"] = "Edit Reply";
    return View(new ReplyUpdateDto(
      reply.Id,
      reply.Content)
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
    Reply? reply = db.Replies
      .FirstOrDefault(t => t.Id == id);

    if (reply == null) { return NotFound(); }
    if (nickname != reply.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewData["Title"] = "Delete Thread";
    return View(new ReplyUpdateDto(
      reply.Id,
      reply.Content)
    );
  }

  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromForm] ReplyCreateDto dto)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }
    if (dto.Content == null)
    {
      return BadRequest("Content cannot be empty");
    }

    Models.Thread? thread = await db.Threads.FirstOrDefaultAsync(t => t.Id == dto.ThreadId);
    if (thread == null)
    {
      return NotFound("Cannot reply to non-existing thread");
    }

    Reply reply = new()
    {
      Author = nickname,
      Content = dto.Content,
      CreatedAt = DateTime.Now
    };
    thread.Replies.Add(reply);

    await db.SaveChangesAsync();

    return RedirectToAction(
      "Index", "Reply",
      new { id = dto.ThreadId });
  }

  [HttpPost]
  public async Task<IActionResult> UpdateAsync(
    [FromRoute] Guid id, [FromForm] ReplyUpdateDto dto)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    Reply? reply = await db.Replies
      .FirstOrDefaultAsync(r => r.Id == id);

    if (reply == null) { return NotFound(); }
    if (nickname != reply.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    reply.Content = dto.Content;
    reply.UpdatedAt = DateTime.Now;

    await db.SaveChangesAsync();

    return RedirectToAction(
      "Index", "Reply",
      new { id = reply.ThreadId });
  }

  [HttpPost]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
  {
    string? nickname = HttpContext.Session.GetString("Nickname");
    if (nickname == null)
    {
      return RedirectToAction("Index", "Home");
    }

    Reply? reply = await db.Replies
      .Include(r => r.Thread)
      .FirstOrDefaultAsync(r => r.Id == id);

    if (reply == null) { return NotFound(); }
    if (nickname != reply.Author)
    {
      return RedirectToAction("Index", "Home");
    }

    db.Replies.Remove(reply);
    await db.SaveChangesAsync();

    return RedirectToAction(
      "Index", "Reply",
      new { id = reply.ThreadId });
  }
}

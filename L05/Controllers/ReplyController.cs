using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using L05.Models;

namespace L05.Controllers;

public class ReplyController(AppDbContext db) : Controller
{
  public async Task<IActionResult> CreateAsync([FromBody] ReplyCreateDto dto)
  {
    if (dto.Author == null || dto.Content == null)
    {
      return BadRequest("Author and Content cannot be empty");
    }

    using AppDbContext db = new();

    Models.Thread? thread = await db.Threads.FirstOrDefaultAsync(t => t.Id == dto.ThreadId);
    if (thread == null)
    {
      return NotFound("Cannot reply to non-existing thread");
    }

    Reply reply = new()
    {
      Author = dto.Author,
      Content = dto.Content,
      CreatedAt = DateTime.Now
    };
    thread.Replies.Add(reply);

    await db.SaveChangesAsync();

    return Ok(reply);
  }

  public async Task<IActionResult> GetAllByThreadIdAsync([FromRoute] Guid id)
  {
    Models.Thread? thread = await db.Threads
      .Include(t => t.Replies)
      .FirstOrDefaultAsync(t => t.Id == id);

    if (thread == null) { return NotFound(); }

    return Ok(thread.Replies.ToArray());
  }

  public async Task<IActionResult> UpdateAsync(
    [FromRoute] Guid id, [FromBody] ReplyUpdateDto dto)
  {
    Reply? reply = await db.Replies
      .FirstOrDefaultAsync(r => r.Id == id);

    if (reply == null) { return NotFound(); }

    reply.Content = dto.Content;
    reply.UpdatedAt = DateTime.Now;

    await db.SaveChangesAsync();

    return Ok(reply);
  }

  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
  {
    Reply? reply = await db.Replies
      .Include(r => r.Thread)
      .FirstOrDefaultAsync(r => r.Id == id);

    if (reply == null) { return NotFound(); }

    db.Replies.Remove(reply);
    await db.SaveChangesAsync();

    return NoContent();
  }
}

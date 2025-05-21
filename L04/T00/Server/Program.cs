using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L04.T00.Server;

public static class Program
{
  public static void Main(string[] args)
  {
    AppDbContext.SeedData();

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCors(options =>
      {
        options.AddDefaultPolicy(policy =>
        {
          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
      });

    WebApplication app = builder.Build();

    app.UseCors();

    // Thread - CREATE
    app.MapPost(
      "thread",
      async ([FromBody] ThreadCreateDto dto) =>
      {
        if (dto.Author == null || dto.Title == null)
        {
          return Results.BadRequest("Author and Title cannot be empty");
        }

        Thread thread = new()
        {
          Author = dto.Author,
          Title = dto.Title,
          Description = dto.Description,
          CreatedAt = DateTime.Now
        };

        using AppDbContext db = new();
        await db.Threads.AddAsync(thread);
        await db.SaveChangesAsync();

        return Results.Ok(thread);
      }
    );

    // Thread - READ One
    app.MapGet(
      "thread/{id}",
      async ([FromRoute] Guid id) =>
      {
        using AppDbContext db = new();
        Thread? thread = await db.Threads
          .Include(t => t.Replies)
          .FirstOrDefaultAsync(t => t.Id == id);

        if (thread == null) { return Results.NotFound(); }

        return Results.Ok(thread);
      }
    );

    // Thread - READ All
    app.MapGet(
      "thread",
      () =>
      {
        using AppDbContext db = new();
        return Results.Ok(db.Threads
          .Include(t => t.Replies)
          .OrderByDescending(t => t.CreatedAt)
          .ToArray());
      }
    );

    // Thread - UPDATE
    app.MapPut(
      "thread/{id}",
      async ([FromRoute] Guid id, [FromBody] ThreadUpdateDto dto) =>
      {
        using AppDbContext db = new();

        Thread? thread = await db.Threads
          .FirstOrDefaultAsync(t => t.Id == id);

        if (thread == null) { return Results.NotFound(); }

        thread.Title = dto.Title ?? thread.Title;
        thread.Description = dto.Description ?? thread.Description;
        thread.UpdatedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return Results.Ok(thread);
      }
    );

    // Thread - DELETE
    app.MapDelete(
      "thread/{id}",
      async ([FromRoute] Guid id) =>
      {
        using AppDbContext db = new();

        Thread? thread = await db.Threads
          .Include(t => t.Replies)
          .FirstOrDefaultAsync(t => t.Id == id);

        if (thread == null) { return Results.NotFound(); }

        db.Threads.Remove(thread);
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );

    // Reply - CREATE
    app.MapPost(
      "reply",
      async ([FromBody] ReplyCreateDto dto) =>
      {
        if (dto.Author == null || dto.Content == null)
        {
          return Results.BadRequest("Author and Content cannot be empty");
        }

        using AppDbContext db = new();
        Thread? thread = await db.Threads.FirstOrDefaultAsync(t => t.Id == dto.ThreadId);
        if (thread == null)
        {
          return Results.NotFound("Cannot reply to non-existing thread");
        }

        Reply reply = new()
        {
          Author = dto.Author,
          Content = dto.Content,
          CreatedAt = DateTime.Now
        };
        thread.Replies.Add(reply);

        await db.SaveChangesAsync();

        return Results.Ok(reply);
      }
    );

    // Reply - READ One
    app.MapGet(
      "reply/{id}",
      async ([FromRoute] Guid id) =>
      {
        using AppDbContext db = new();
        Reply? reply = await db.Replies
          .FirstOrDefaultAsync(r => r.Id == id);

        if (reply == null) { return Results.NotFound(); }

        return Results.Ok(reply);
      }
    );

    // Reply - READ All (under same thread)
    app.MapGet(
      "thread/{id}/replies",
      async ([FromRoute] Guid id) =>
      {
        using AppDbContext db = new();
        Thread? thread = await db.Threads
          .Include(t => t.Replies)
          .FirstOrDefaultAsync(t => t.Id == id);

        if (thread == null) { return Results.NotFound(); }

        return Results.Ok(thread.Replies.ToArray());
      }
    );

    // Reply - UPDATE
    app.MapPut(
      "reply/{id}",
      async ([FromRoute] Guid id, [FromBody] ReplyUpdateDto dto) =>
      {
        using AppDbContext db = new();

        Reply? reply = await db.Replies
          .FirstOrDefaultAsync(r => r.Id == id);

        if (reply == null) { return Results.NotFound(); }

        reply.Content = dto.Content;
        reply.UpdatedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return Results.Ok(reply);
      }
    );

    // Reply - DELETE
    app.MapDelete(
      "reply/{id}",
      async ([FromRoute] Guid id) =>
      {
        using AppDbContext db = new();

        Reply? reply = await db.Replies
          .Include(r => r.Thread)
          .FirstOrDefaultAsync(r => r.Id == id);

        if (reply == null) { return Results.NotFound(); }

        db.Replies.Remove(reply);
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );

    app.Run();
  }
}

namespace L05.Models;

public class Thread
{
  public Guid Id { get; set; }
  public required string Author { get; set; }
  public required string Title { get; set; }
  public string? Description { get; set; }
  public required DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

  public ICollection<Reply> Replies { get; set; } = [];
}

public record ThreadsPageViewModel(
  Thread[] Threads,
  string Nickname,
  string Search
)
{ }

public record ThreadCreateDto(
  string Author,
  string Title,
  string? Description
)
{ }

public record ThreadUpdateDto(
  Guid Id,
  string? Title,
  string? Description
)
{ }

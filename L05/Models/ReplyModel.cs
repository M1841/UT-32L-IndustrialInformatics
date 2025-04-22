using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace L05.Models;

public class Reply
{
  public Guid Id { get; set; }
  public Guid ThreadId { get; set; }
  public required string Author { get; set; }
  public required string Content { get; set; }
  public required DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

  [JsonIgnore]
  [ForeignKey(nameof(ThreadId))]
  public Thread? Thread { get; set; }
}

public record ReplyCreateDto(
  Guid ThreadId,
  string Author,
  string Content
)
{ }

public record ReplyUpdateDto(
  string Content
)
{ }

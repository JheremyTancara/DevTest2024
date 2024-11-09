using System.Text.Json.Serialization;
using Project.Core.Entities.Options.Models;

namespace Project.Core.Entities.Polls.Models
{
  public class Poll
  {
    public Guid PollID { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid[]? PollOptionID { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
  }
}

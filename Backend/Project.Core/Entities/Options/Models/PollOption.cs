using System.Text.Json.Serialization;

namespace Project.Core.Entities.Options.Models
{
  public class PollOption
  {
    public Guid PollOptionID { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Votes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
  }
}

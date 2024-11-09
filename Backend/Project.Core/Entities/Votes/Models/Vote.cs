using System.Text.Json.Serialization;

namespace Project.Core.Entities.Options.Models
{
  public class Vote
  {
    public Guid VoteID { get; set; }
    public Guid PollID { get; set; }
    public Guid PollOptionID { get; set; }
    public string VoterEmail { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
  }
}

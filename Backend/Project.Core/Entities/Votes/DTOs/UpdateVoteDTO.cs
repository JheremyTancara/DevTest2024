using Project.Core.Entities.Options.Models;

namespace Project.Core.Entities.Options.DTOs
{
  public class UpdateVoteDTO
  {
    public Guid? PollID { get; set; }
    public Guid? PollOptionID { get; set; }
    public string? VoterEmail { get; set; } = string.Empty;
  }
}

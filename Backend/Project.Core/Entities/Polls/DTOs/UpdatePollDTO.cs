using Project.Core.Entities.Options.Models;

namespace Project.Core.Entities.Polls.DTOs
{
  public class UpdatePollDTO
  {
    public string? Name { get; set; } = string.Empty;
    public Guid[]? PollOptionID { get; set; }
  }
}

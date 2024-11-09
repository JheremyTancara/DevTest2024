using Project.Core.Entities.Options.Models;

namespace Project.Core.Entities.Options.DTOs
{
  public class UpdatePollOptionsDTO
  {
    public string? Name { get; set; } = string.Empty;
    public int? Votes { get; set; }
  }
}

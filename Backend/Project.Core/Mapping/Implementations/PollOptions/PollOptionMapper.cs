using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Mapping.Interface;

namespace Project.Core.Mapping.Implementations.Products
{
  public class PollOptionMapper : IMapper<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO>
  {
    public PollOption MapCreate(RegisterPollOptionsDTO productDTO)
    {
      return new PollOption
      {
        Name = productDTO.Name,
        Votes = productDTO.Votes,
        CreatedAt = DateTimeOffset.UtcNow
      };
    }

    public void MapUpdate(PollOption existingPollOption, UpdatePollOptionsDTO pollOptionDTO)
    {
      if (!string.IsNullOrEmpty(pollOptionDTO.Name) && existingPollOption.Name != pollOptionDTO.Name)
      {
        existingPollOption.Name = pollOptionDTO.Name;
      }

      if (pollOptionDTO.Votes.HasValue && existingPollOption.Votes != pollOptionDTO.Votes.Value)
      {
        existingPollOption.Votes = pollOptionDTO.Votes.Value;
      }
    }
  }
}

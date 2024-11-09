using Project.Core.Entities.Polls.DTOs;
using Project.Core.Entities.Polls.Models;
using Project.Core.Mapping.Interface;

namespace Project.Core.Mapping.Implementations.Products
{
  public class PollMapper : IMapper<Poll, RegisterPollDTO, UpdatePollDTO>
  {
    public Poll MapCreate(RegisterPollDTO productDTO)
    {
      return new Poll
      {
        Name = productDTO.Name,
        PollOptionID = productDTO.PollOptionID,
        CreatedAt = DateTimeOffset.UtcNow
      };
    }

    public void MapUpdate(Poll existingPollOption, UpdatePollDTO pollOptionDTO)
    {
      if (!string.IsNullOrEmpty(pollOptionDTO.Name) && existingPollOption.Name != pollOptionDTO.Name)
      {
        existingPollOption.Name = pollOptionDTO.Name;
      }
    }
  }
}

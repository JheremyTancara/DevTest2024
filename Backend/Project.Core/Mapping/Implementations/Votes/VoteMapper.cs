using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Mapping.Interface;

namespace Project.Core.Mapping.Implementations.Products
{
  public class VoteMapper : IMapper<Vote, RegisterVoteDTO, UpdateVoteDTO>
  {
    public Vote MapCreate(RegisterVoteDTO productDTO)
    {
      return new Vote
      {
        PollID = productDTO.PollID,
        PollOptionID = productDTO.PollOptionID,
        VoterEmail = productDTO.VoterEmail,
        CreatedAt = DateTimeOffset.UtcNow
      };
    }

    public void MapUpdate(Vote existingVote, UpdateVoteDTO VoteDTO)
    {
      if (!string.IsNullOrEmpty(VoteDTO.VoterEmail) && existingVote.VoterEmail != VoteDTO.VoterEmail)
      {
        existingVote.VoterEmail = VoteDTO.VoterEmail;
      }
    }
  }
}

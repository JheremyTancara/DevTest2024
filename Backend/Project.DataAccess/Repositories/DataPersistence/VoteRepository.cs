using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Entities.Polls.DTOs;
using Project.Core.Entities.Polls.Models;
using Project.Core.Mapping.Interface;
using Project.DataAccess.Base;
using Project.DataAccess.Interface;

namespace Project.DataAccess.Repositories.DataPersistence
{
  public class VoteRepository : RepositoryBase<Vote, RegisterVoteDTO, UpdateVoteDTO>
  {
    private readonly AppDataContext _context;
    private readonly IMapper<Vote, RegisterVoteDTO, UpdateVoteDTO> _VoteMapper;
    private readonly IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> _pollOptionRepository;
    private readonly IRepository<Poll, RegisterPollDTO, UpdatePollDTO> _pollRepository;


    public VoteRepository(AppDataContext context, IMapper<Vote, RegisterVoteDTO, UpdateVoteDTO> VoteMapper, IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> pollOptionRepository, IRepository<Poll, RegisterPollDTO, UpdatePollDTO> pollRepository)
    {
      _context = context;
      _VoteMapper = VoteMapper;
      _pollOptionRepository = pollOptionRepository;
      _pollRepository = pollRepository;
    }

    public override async Task<IEnumerable<Vote>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      IQueryable<Vote> query;

      if (includeDeleted) {
        query = _context.Votes;
      } 
      else {
        query = _context.Votes.Where(p => !p.IsDeleted);
      }

      var sortBy = sort.TrimStart('+', '-'); 
      if (sort.StartsWith("-"))
      {
        query = query.OrderByDescending(p => EF.Property<object>(p, sortBy));
      }
      else
      {
        query = query.OrderBy(p => EF.Property<object>(p, sortBy));
      }
      return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public override async Task<Vote?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (includeDeleted)
      {
        return await _context.Votes.FindAsync(id);
      }

      var product = await _context.Votes.FindAsync(id);
      return product != null && !product.IsDeleted ? product : null;
    }

    public override async Task<Vote> CreateAsync(RegisterVoteDTO voteDTO)
    {
      var pollExists = await _pollRepository.GetByIdAsync(voteDTO.PollID);

      if (pollExists == null)
      {
          throw new ArgumentException("The specified PollID does not exist or is deleted.");
      }

      var pollOptionExists = await _pollOptionRepository.GetByIdAsync(voteDTO.PollOptionID);

      if (pollOptionExists == null)
      {
          throw new ArgumentException("The specified PollOptionID does not exist or is deleted.");
      }

      var vote = _VoteMapper.MapCreate(voteDTO);
      vote.VoteID = Guid.NewGuid();

      _context.Votes.Add(vote);
      await _context.SaveChangesAsync();

      return vote;
    }

    public override async Task UpdateAsync(Guid id, UpdateVoteDTO voteDTO)
    {
      var existingVote = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Vote with ID {id} was not found.");

      _VoteMapper.MapUpdate(existingVote, voteDTO);
      await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(Guid id)
    {
      var voteToDelete = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Vote with ID {id} was not found.");

      voteToDelete.IsDeleted = true;
      await _context.SaveChangesAsync();
    }

    public override async Task UndeleteAsync(Guid id)
    {
      var voteToRestore = await _context.Votes.FirstOrDefaultAsync(p => p.VoteID == id && p.IsDeleted) ??                
      throw new KeyNotFoundException($"Vote with ID {id} was not found or is already active.");

      voteToRestore.IsDeleted = false;
      await _context.SaveChangesAsync();
    }

    public override async Task<bool> ExistsByNameAsync(string VoterEmail)
    {
      return await _context.Votes
        .AnyAsync(p => p.VoterEmail.ToLower() == VoterEmail.ToLower() && !p.IsDeleted);
    }
  }
}

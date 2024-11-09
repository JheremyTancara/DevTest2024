using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Entities.Polls.DTOs;
using Project.Core.Entities.Polls.Models;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.Core.Mapping.Interface;
using Project.Core.Utils;
using Project.DataAccess.Base;
using Project.DataAccess.Interface;

namespace Project.DataAccess.Repositories.DataPersistence
{
  public class PollRepository : RepositoryBase<Poll, RegisterPollDTO, UpdatePollDTO>
  {
    private readonly AppDataContext _context;
    private readonly IMapper<Poll, RegisterPollDTO, UpdatePollDTO> _pollMapper;
    private readonly IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> _pollOptionRepository;

    public PollRepository(AppDataContext context, IMapper<Poll, RegisterPollDTO, UpdatePollDTO> pollMapper, IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> pollOptionRepository)
    {
      _context = context;
      _pollMapper = pollMapper;
      _pollOptionRepository = pollOptionRepository;
    }

    public override async Task<IEnumerable<Poll>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      IQueryable<Poll> query;

      if (includeDeleted) {
        query = _context.Polls;
      } 
      else {
        query = _context.Polls.Where(p => !p.IsDeleted);
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

    public override async Task<Poll?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (includeDeleted)
      {
        return await _context.Polls.FindAsync(id);
      }

      var product = await _context.Polls.FindAsync(id);
      return product != null && !product.IsDeleted ? product : null;
    }

    public override async Task<Poll> CreateAsync(RegisterPollDTO pollDTO)
    {
      for (int i=0; i<pollDTO.PollOptionID.Length; i++) {
          var productExists = await _pollOptionRepository.GetByIdAsync(pollDTO.PollOptionID[i]);

          if (productExists == null)
          {
              throw new ArgumentException("The specified PollOptionID does not exist or is deleted.");
          }
      }
      
      if (pollDTO.PollOptionID.Length < 2)
      {
          throw new ArgumentException("Insufficient PollOptionsId. At least 2 PollOptionsID are required to create 1 poll");
      }

      var poll = _pollMapper.MapCreate(pollDTO);
      poll.PollID = Guid.NewGuid();

      _context.Polls.Add(poll);
      await _context.SaveChangesAsync();

      return poll;
    }

    public override async Task UpdateAsync(Guid id, UpdatePollDTO pollDTO)
    {
      var existingPoll = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Poll with ID {id} was not found.");

      _pollMapper.MapUpdate(existingPoll, pollDTO);
      await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(Guid id)
    {
      var productToDelete = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Poll with ID {id} was not found.");

      productToDelete.IsDeleted = true;
      await _context.SaveChangesAsync();
    }

    public override async Task UndeleteAsync(Guid id)
    {
      var productToRestore = await _context.Polls.FirstOrDefaultAsync(p => p.PollID == id && p.IsDeleted) ??                
      throw new KeyNotFoundException($"Product with ID {id} was not found or is already active.");

      productToRestore.IsDeleted = false;
      await _context.SaveChangesAsync();
    }

    public override async Task<bool> ExistsByNameAsync(string name)
    {
      return await _context.Polls
        .AnyAsync(p => p.Name.ToLower() == name.ToLower() && !p.IsDeleted);
    }
  }
}

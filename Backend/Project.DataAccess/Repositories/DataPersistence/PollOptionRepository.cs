using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Mapping.Interface;
using Project.DataAccess.Base;

namespace Project.DataAccess.Repositories.DataPersistence
{
  public class PollOptionRepository : RepositoryBase<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO>
  {
    private readonly AppDataContext _context;
    private readonly IMapper<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> _pollOptionMapper;

    public PollOptionRepository(AppDataContext context, IMapper<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> pollOptionMapper)
    {
      _context = context;
      _pollOptionMapper = pollOptionMapper;
    }

    public override async Task<IEnumerable<PollOption>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      IQueryable<PollOption> query;

      if (includeDeleted) {
        query = _context.PollOptions;
      } 
      else {
        query = _context.PollOptions.Where(p => !p.IsDeleted);
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

    public override async Task<PollOption?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (includeDeleted)
      {
        return await _context.PollOptions.FindAsync(id);
      }

      var product = await _context.PollOptions.FindAsync(id);
      return product != null && !product.IsDeleted ? product : null;
    }

    public override async Task<PollOption> CreateAsync(RegisterPollOptionsDTO pullOptionDTO)
    {
      var pullOption = _pollOptionMapper.MapCreate(pullOptionDTO);
      pullOption.PollOptionID = Guid.NewGuid();

      _context.PollOptions.Add(pullOption);
      await _context.SaveChangesAsync();

      return pullOption;
    }

    public override async Task UpdateAsync(Guid id, UpdatePollOptionsDTO pullOptionDTO)
    {
      var existingProduct = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Product with ID {id} was not found.");

      _pollOptionMapper.MapUpdate(existingProduct, pullOptionDTO);
      await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(Guid id)
    {
      var pullOptionToDelete = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Product with ID {id} was not found.");

      pullOptionToDelete.IsDeleted = true;
      await _context.SaveChangesAsync();
    }

    public override async Task UndeleteAsync(Guid id)
    {
      var pullOptionToRestore = await _context.PollOptions.FirstOrDefaultAsync(p => p.PollOptionID == id && p.IsDeleted) ??                
      throw new KeyNotFoundException($"Product with ID {id} was not found or is already active.");

      pullOptionToRestore.IsDeleted = false;
      await _context.SaveChangesAsync();
    }

    public override async Task<bool> ExistsByNameAsync(string name)
    {
      return await _context.PollOptions
        .AnyAsync(p => p.Name.ToLower() == name.ToLower() && !p.IsDeleted);
    }
  }
}

using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.Core.Mapping.Interface;
using Project.DataAccess.Base;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Project.DataAccess.Repositories.InMemory
{
  public class PollOptionRepository : RepositoryBase<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO>
  {
    private readonly IMapper<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> _pollOptionMapper;
    private readonly ConcurrentDictionary<Guid, PollOption> _pollOptions = new();
    private const string FilePath = "Resources/polloptions.json";

    public PollOptionRepository(IMapper<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> pollOptionMapper)
    {
      _pollOptionMapper = pollOptionMapper;
      LoadProductsFromFile();
    }

    private void LoadProductsFromFile()
    {
      if (File.Exists(FilePath))
      {
        var json = File.ReadAllText(FilePath);
        var pollOptions = JsonSerializer.Deserialize<ConcurrentDictionary<Guid, PollOption>>(json);
        if (pollOptions != null)
        {
          foreach (var pollOption in pollOptions)
          {
            _pollOptions.TryAdd(pollOption.Key, pollOption.Value);
          }
        }
      }
    }

    private void SaveProductsToFile()
    {
      var json = JsonSerializer.Serialize(_pollOptions);
      File.WriteAllText(FilePath, json);
    }

    public override Task<IEnumerable<PollOption>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      var query = _pollOptions.Values.AsQueryable();

      if (!includeDeleted)
      {
        query = query.Where(p => !p.IsDeleted);
      }

      var sortedQuery = sort.StartsWith("-")
        ? query.OrderByDescending(p => p.CreatedAt)
        : query.OrderBy(p => p.CreatedAt);

      var pagedResult = sortedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsEnumerable();
      return Task.FromResult(pagedResult);
    }

    public override Task<PollOption?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (_pollOptions.TryGetValue(id, out var product) && (includeDeleted || !product.IsDeleted))
      {
        return Task.FromResult<PollOption?>(product);
      }
      return Task.FromResult<PollOption?>(null);
    }

    public override Task<PollOption> CreateAsync(RegisterPollOptionsDTO pollOptionDTO)
    {
      var pollOption = _pollOptionMapper.MapCreate(pollOptionDTO);
      pollOption.PollOptionID = Guid.NewGuid();

      _pollOptions[pollOption.PollOptionID] = pollOption;
      SaveProductsToFile();
      return Task.FromResult(pollOption);
    }

    public override Task UpdateAsync(Guid id, UpdatePollOptionsDTO pollOptionDTO)
    {
      if (_pollOptions.TryGetValue(id, out var existingPollOption))
      {
        _pollOptionMapper.MapUpdate(existingPollOption, pollOptionDTO);
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task DeleteAsync(Guid id)
    {
      if (_pollOptions.TryGetValue(id, out var pollOption))
      {
        pollOption.IsDeleted = true;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task UndeleteAsync(Guid id)
    {
      if (_pollOptions.TryGetValue(id, out var pollOption) && pollOption.IsDeleted)
      {
        pollOption.IsDeleted = false;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task<bool> ExistsByNameAsync(string name)
    {
      var exists = _pollOptions.Values.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !p.IsDeleted);
      return Task.FromResult(exists);
    }
  }
}

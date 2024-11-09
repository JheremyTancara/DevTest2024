using Project.Core.Entities.Polls.DTOs;
using Project.Core.Entities.Polls.Models;
using Project.Core.Mapping.Interface;
using Project.DataAccess.Base;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Project.DataAccess.Repositories.InMemory
{
  public class PollRepository : RepositoryBase<Poll, RegisterPollDTO, UpdatePollDTO>
  {
    private readonly IMapper<Poll, RegisterPollDTO, UpdatePollDTO> _pollMapper;
    private readonly ConcurrentDictionary<Guid, Poll> _poll = new();
    private const string FilePath = "Resources/poll.json";

    public PollRepository(IMapper<Poll, RegisterPollDTO, UpdatePollDTO> pollMapper)
    {
      _pollMapper = pollMapper;
      LoadProductsFromFile();
    }

    private void LoadProductsFromFile()
    {
      if (File.Exists(FilePath))
      {
        var json = File.ReadAllText(FilePath);
        var poll = JsonSerializer.Deserialize<ConcurrentDictionary<Guid, Poll>>(json);
        if (poll != null)
        {
          foreach (var polls in poll)
          {
            _poll.TryAdd(polls.Key, polls.Value);
          }
        }
      }
    }

    private void SaveProductsToFile()
    {
      var json = JsonSerializer.Serialize(_poll);
      File.WriteAllText(FilePath, json);
    }

    public override Task<IEnumerable<Poll>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      var query = _poll.Values.AsQueryable();

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

    public override Task<Poll?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (_poll.TryGetValue(id, out var poll) && (includeDeleted || !poll.IsDeleted))
      {
        return Task.FromResult<Poll?>(poll);
      }
      return Task.FromResult<Poll?>(null);
    }

    public override Task<Poll> CreateAsync(RegisterPollDTO pollOptionDTO)
    {
      var pollOption = _pollMapper.MapCreate(pollOptionDTO);
      pollOption.PollID = Guid.NewGuid();

      _poll[pollOption.PollID] = pollOption;
      SaveProductsToFile();
      return Task.FromResult(pollOption);
    }

    public override Task UpdateAsync(Guid id, UpdatePollDTO pollDTO)
    {
      if (_poll.TryGetValue(id, out var existingPoll))
      {
        _pollMapper.MapUpdate(existingPoll, pollDTO);
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task DeleteAsync(Guid id)
    {
      if (_poll.TryGetValue(id, out var poll))
      {
        poll.IsDeleted = true;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task UndeleteAsync(Guid id)
    {
      if (_poll.TryGetValue(id, out var poll) && poll.IsDeleted)
      {
        poll.IsDeleted = false;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task<bool> ExistsByNameAsync(string name)
    {
      var exists = _poll.Values.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !p.IsDeleted);
      return Task.FromResult(exists);
    }
  }
}

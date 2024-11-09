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
  public class VoteRepository : RepositoryBase<Vote, RegisterVoteDTO, UpdateVoteDTO>
  {
    private readonly IMapper<Vote, RegisterVoteDTO, UpdateVoteDTO> _VoteMapper;
    private readonly ConcurrentDictionary<Guid, Vote> _Votes = new();
    private const string FilePath = "Resources/vote.json";

    public VoteRepository(IMapper<Vote, RegisterVoteDTO, UpdateVoteDTO> VoteMapper)
    {
      _VoteMapper = VoteMapper;
      LoadProductsFromFile();
    }

    private void LoadProductsFromFile()
    {
      if (File.Exists(FilePath))
      {
        var json = File.ReadAllText(FilePath);
        var Votes = JsonSerializer.Deserialize<ConcurrentDictionary<Guid, Vote>>(json);
        if (Votes != null)
        {
          foreach (var Vote in Votes)
          {
            _Votes.TryAdd(Vote.Key, Vote.Value);
          }
        }
      }
    }

    private void SaveProductsToFile()
    {
      var json = JsonSerializer.Serialize(_Votes);
      File.WriteAllText(FilePath, json);
    }

    public override Task<IEnumerable<Vote>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      var query = _Votes.Values.AsQueryable();

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

    public override Task<Vote?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (_Votes.TryGetValue(id, out var product) && (includeDeleted || !product.IsDeleted))
      {
        return Task.FromResult<Vote?>(product);
      }
      return Task.FromResult<Vote?>(null);
    }

    public override Task<Vote> CreateAsync(RegisterVoteDTO VoteDTO)
    {
      var Vote = _VoteMapper.MapCreate(VoteDTO);
      Vote.VoteID = Guid.NewGuid();

      _Votes[Vote.VoteID] = Vote;
      SaveProductsToFile();
      return Task.FromResult(Vote);
    }

    public override Task UpdateAsync(Guid id, UpdateVoteDTO VoteDTO)
    {
      if (_Votes.TryGetValue(id, out var existingVote))
      {
        _VoteMapper.MapUpdate(existingVote, VoteDTO);
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task DeleteAsync(Guid id)
    {
      if (_Votes.TryGetValue(id, out var Vote))
      {
        Vote.IsDeleted = true;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task UndeleteAsync(Guid id)
    {
      if (_Votes.TryGetValue(id, out var Vote) && Vote.IsDeleted)
      {
        Vote.IsDeleted = false;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task<bool> ExistsByNameAsync(string VoterEmail)
    {
      var exists = _Votes.Values.Any(p => p.VoterEmail.Equals(VoterEmail, StringComparison.OrdinalIgnoreCase) && !p.IsDeleted);
      return Task.FromResult(exists);
    }
  }
}

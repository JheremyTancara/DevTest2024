using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Utils;
using Project.DataAccess.Interface;

namespace Project.Api.Controllers.V1
{
  [ApiExplorerSettings(GroupName = "v1")]
  [Route("api/v1/[controller]")]
  [ApiController]
  public class VoteController : ControllerBase
  {
    private readonly IRepository<Vote, RegisterVoteDTO, UpdateVoteDTO> VoteRepository;

    public VoteController(IRepository<Vote, RegisterVoteDTO, UpdateVoteDTO> _VoteRepository)
    {
      VoteRepository = _VoteRepository;
    }

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    [HttpGet(Name = "GetVotes")]
    public async Task<ActionResult<IEnumerable<Vote>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "+CreatedAt", [FromQuery] bool includeDeleted = false)
    {
      var sortValidationError = ProductErrorMessages.InvalidSortParameter(sort);
      if (sortValidationError != null)
      {
        return sortValidationError;
      }
      var pagedVote = await VoteRepository.GetAllAsync(pageNumber, pageSize, sort, includeDeleted);
      return Ok(pagedVote);
    }

    /// <summary>
    /// Retrieves a specific product by its unique identifier.
    /// </summary>
    [HttpGet("{id}", Name = "GetVote")]
    public async Task<ActionResult<Vote>> GetById(Guid id)
    {
      var Vote = await VoteRepository.GetByIdAsync(id);

      if (Vote == null)
      {
        return ProductErrorMessages.ProductNotFound(id);
      }

      return Ok(Vote);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    [HttpPost(Name = "AddVote")]
    public async Task<IActionResult> Create([FromBody] RegisterVoteDTO VoteDTO)
    {
      if (await VoteRepository.ExistsByNameAsync(VoteDTO.VoterEmail))
      {
        return ProductErrorMessages.UniqueVoterEmailConflict(VoteDTO.VoterEmail);
      }

      var newVote = await VoteRepository.CreateAsync(VoteDTO);

      return CreatedAtAction(nameof(GetById), new { id = newVote.VoteID }, newVote);
    }

    /// <summary>
    /// Updates an existing product by its unique identifier.
    /// </summary>
    [HttpPatch("{id}", Name = "UpdateVote")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVoteDTO VoteDTO)
    {
      var VoteToUpdate = await VoteRepository.GetByIdAsync(id);

      if (VoteToUpdate != null)
      {
        await VoteRepository.UpdateAsync(id, VoteDTO);
        return NoContent();
      }
      else
      {
        return ProductErrorMessages.ProductNotFound(id);
      }
    }

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    [HttpDelete("{id}", Name = "DeleteVote")]
    public async Task<IActionResult> Delete(Guid id)
    {
      var VoteToDelete = await VoteRepository.GetByIdAsync(id);

      if (VoteToDelete == null)
      {
        return ProductErrorMessages.ProductNotFound(id);
      }

      await VoteRepository.DeleteAsync(id);
      return NoContent();
    }

    /// <summary>
    /// Restores a deleted product by its unique identifier.
    /// </summary>
    [HttpPost("{id}/undeleted", Name = "UndeleteVote")]
    public async Task<IActionResult> UnDelete(Guid id)
    {
      var VoteToDelete = await VoteRepository.GetByIdAsync(id, true);

      if (VoteToDelete == null){
        return ProductErrorMessages.ProductNotFound(id);
      }
      if (!VoteToDelete.IsDeleted) {
        return ProductErrorMessages.ObjectAlreadyActivated(id);
      }

      await VoteRepository.UndeleteAsync(id);
      return NoContent();
    }
  }
}
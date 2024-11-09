using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Entities.Polls.DTOs;
using Project.Core.Entities.Polls.Models;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.Core.Utils;
using Project.DataAccess.Interface;

namespace Project.Api.Controllers.V1
{
  [ApiExplorerSettings(GroupName = "v1")]
  [Route("api/v1/[controller]")]
  [ApiController]
  public class PollController : ControllerBase
  {
    private readonly IRepository<Poll, RegisterPollDTO, UpdatePollDTO> pollRepository;

    public PollController(IRepository<Poll, RegisterPollDTO, UpdatePollDTO> _pollRepository)
    {
      pollRepository = _pollRepository;
    }

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    [HttpGet(Name = "GetPolls")]
    public async Task<ActionResult<IEnumerable<Poll>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "+CreatedAt", [FromQuery] bool includeDeleted = false)
    {
      var sortValidationError = ProductErrorMessages.InvalidSortParameter(sort);
      if (sortValidationError != null)
      {
        return sortValidationError;
      }
      var pagedPoll = await pollRepository.GetAllAsync(pageNumber, pageSize, sort, includeDeleted);
      return Ok(pagedPoll);
    }

    /// <summary>
    /// Retrieves a specific product by its unique identifier.
    /// </summary>
    [HttpGet("{id}", Name = "GetPoll")]
    public async Task<ActionResult<Poll>> GetById(Guid id)
    {
      var poll = await pollRepository.GetByIdAsync(id);

      if (poll == null)
      {
        return ProductErrorMessages.ProductNotFound(id);
      }

      return Ok(poll);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    [HttpPost(Name = "AddPoll")]
    public async Task<IActionResult> Create([FromBody] RegisterPollDTO pollDTO)
    {
      if (await pollRepository.ExistsByNameAsync(pollDTO.Name))
      {
        return ProductErrorMessages.UniqueNameConflict(pollDTO.Name);
      }

      var newPoll = await pollRepository.CreateAsync(pollDTO);

      return CreatedAtAction(nameof(GetById), new { id = newPoll.PollID }, newPoll);
    }

    /// <summary>
    /// Updates an existing product by its unique identifier.
    /// </summary>
    [HttpPatch("{id}", Name = "UpdatePoll")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePollDTO pollDTO)
    {
      var pollToUpdate = await pollRepository.GetByIdAsync(id);

      if (pollToUpdate != null)
      {
        await pollRepository.UpdateAsync(id, pollDTO);
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
    [HttpDelete("{id}", Name = "DeletePoll")]
    public async Task<IActionResult> Delete(Guid id)
    {
      var pollToDelete = await pollRepository.GetByIdAsync(id);

      if (pollToDelete == null)
      {
        return ProductErrorMessages.ProductNotFound(id);
      }

      await pollRepository.DeleteAsync(id);
      return NoContent();
    }

    /// <summary>
    /// Restores a deleted product by its unique identifier.
    /// </summary>
    [HttpPost("{id}/undeleted", Name = "UndeletePoll")]
    public async Task<IActionResult> UnDelete(Guid id)
    {
      var pollToDelete = await pollRepository.GetByIdAsync(id, true);

      if (pollToDelete == null){
        return ProductErrorMessages.ProductNotFound(id);
      }
      if (!pollToDelete.IsDeleted) {
        return ProductErrorMessages.ObjectAlreadyActivated(id);
      }

      await pollRepository.UndeleteAsync(id);
      return NoContent();
    }
  }
}
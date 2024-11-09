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
  public class PollOptionsController : ControllerBase
  {
    private readonly IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> pollOptionRepository;

    public PollOptionsController(IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO> _pollOptionRepository)
    {
      pollOptionRepository = _pollOptionRepository;
    }

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    [HttpGet(Name = "GetPollOptions")]
    public async Task<ActionResult<IEnumerable<PollOption>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "+CreatedAt", [FromQuery] bool includeDeleted = false)
    {
      var sortValidationError = ProductErrorMessages.InvalidSortParameter(sort);
      if (sortValidationError != null)
      {
        return sortValidationError;
      }
      var pagedPollOptions = await pollOptionRepository.GetAllAsync(pageNumber, pageSize, sort, includeDeleted);
      return Ok(pagedPollOptions);
    }

    /// <summary>
    /// Retrieves a specific product by its unique identifier.
    /// </summary>
    [HttpGet("{id}", Name = "GetPollOption")]
    public async Task<ActionResult<PollOption>> GetById(Guid id)
    {
      var pollOption = await pollOptionRepository.GetByIdAsync(id);

      if (pollOption == null)
      {
        return ProductErrorMessages.ProductNotFound(id);
      }

      return Ok(pollOption);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    [HttpPost(Name = "AddPollOption")]
    public async Task<IActionResult> Create([FromBody] RegisterPollOptionsDTO pollOptionDTO)
    {
      if (await pollOptionRepository.ExistsByNameAsync(pollOptionDTO.Name))
      {
        return ProductErrorMessages.UniqueNameConflict(pollOptionDTO.Name);
      }

      var newPollOption = await pollOptionRepository.CreateAsync(pollOptionDTO);

      return CreatedAtAction(nameof(GetById), new { id = newPollOption.PollOptionID }, newPollOption);
    }

    /// <summary>
    /// Updates an existing product by its unique identifier.
    /// </summary>
    [HttpPatch("{id}", Name = "UpdatePollOption")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePollOptionsDTO productDTO)
    {
      var pollOptionToUpdate = await pollOptionRepository.GetByIdAsync(id);

      if (pollOptionToUpdate != null)
      {
        await pollOptionRepository.UpdateAsync(id, productDTO);
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
    [HttpDelete("{id}", Name = "DeletePollOption")]
    public async Task<IActionResult> Delete(Guid id)
    {
      var pollOptionToDelete = await pollOptionRepository.GetByIdAsync(id);

      if (pollOptionToDelete == null)
      {
        return ProductErrorMessages.ProductNotFound(id);
      }

      await pollOptionRepository.DeleteAsync(id);
      return NoContent();
    }

    /// <summary>
    /// Restores a deleted product by its unique identifier.
    /// </summary>
    [HttpPost("{id}/undeleted", Name = "UndeletePollOption")]
    public async Task<IActionResult> UnDelete(Guid id)
    {
      var pollOptionToDelete = await pollOptionRepository.GetByIdAsync(id, true);

      if (pollOptionToDelete == null){
        return ProductErrorMessages.ProductNotFound(id);
      }
      if (!pollOptionToDelete.IsDeleted) {
        return ProductErrorMessages.ObjectAlreadyActivated(id);
      }

      await pollOptionRepository.UndeleteAsync(id);
      return NoContent();
    }
  }
}
using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.DataAccess.Interface;

namespace Project.Api.Controllers.V2
{
  [ApiExplorerSettings(GroupName = "v2")]
  [Route("api/v2/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IRepository<Product, RegisterProductDTO, UpdateProductDTO> productRepository;

    public ProductsController(IRepository<Product, RegisterProductDTO, UpdateProductDTO> _productRepository)
    {
      productRepository = _productRepository;
    }

    [HttpGet(Name = "GetProductsV2")]
    public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "+name")
    {
      var pagedProducts = await productRepository.GetAllAsync(pageNumber, pageSize, sort);
      return Ok(pagedProducts);
    }
  }
}
using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.Core.Mapping.Interface;

namespace Project.Core.Mapping.Implementations.Products
{
  public class ProductMapper : IMapper<Product, RegisterProductDTO, UpdateProductDTO>
  {
    public Product MapCreate(RegisterProductDTO productDTO)
    {
      return new Product
      {
        Name = productDTO.Name,
        Price = productDTO.Price,
        ProductStatus = productDTO.ProductStatus,
        CreatedAt = DateTimeOffset.UtcNow
      };
    }

    public void MapUpdate(Product existingProduct, UpdateProductDTO productDTO)
    {
      if (!string.IsNullOrEmpty(productDTO.Name) && existingProduct.Name != productDTO.Name)
      {
        existingProduct.Name = productDTO.Name;
      }

      if (productDTO.Price.HasValue && existingProduct.Price != productDTO.Price.Value)
      {
        existingProduct.Price = productDTO.Price.Value;
      }

      if (productDTO.ProductStatus.HasValue && existingProduct.ProductStatus != productDTO.ProductStatus.Value)
      {
        existingProduct.ProductStatus = productDTO.ProductStatus.Value;
      }
    }
  }
}

using Project.Core.Entities.Products.Models;

namespace Project.Core.Entities.Products.DTOs
{
  public class RegisterProductDTO
  {
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public ProductStatusEnum ProductStatus { get; set; }
  }
}

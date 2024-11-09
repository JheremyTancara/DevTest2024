using System.Text.Json.Serialization;

namespace Project.Core.Entities.Products.Models
{
  public class Product
  {
    public Guid ProductID { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public ProductStatusEnum ProductStatus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
  }
}

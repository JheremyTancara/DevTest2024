using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.Core.Mapping.Interface;
using Project.DataAccess.Base;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Project.DataAccess.Repositories.InMemory
{
  public class ProductRepository : RepositoryBase<Product, RegisterProductDTO, UpdateProductDTO>
  {
    private readonly IMapper<Product, RegisterProductDTO, UpdateProductDTO> _productMapper;
    private readonly ConcurrentDictionary<Guid, Product> _products = new();
    private const string FilePath = "Resources/products.json";

    public ProductRepository(IMapper<Product, RegisterProductDTO, UpdateProductDTO> productMapper)
    {
      _productMapper = productMapper;
      LoadProductsFromFile();
    }

    private void LoadProductsFromFile()
    {
      if (File.Exists(FilePath))
      {
        var json = File.ReadAllText(FilePath);
        var products = JsonSerializer.Deserialize<ConcurrentDictionary<Guid, Product>>(json);
        if (products != null)
        {
          foreach (var product in products)
          {
            _products.TryAdd(product.Key, product.Value);
          }
        }
      }
    }

    private void SaveProductsToFile()
    {
      var json = JsonSerializer.Serialize(_products);
      File.WriteAllText(FilePath, json);
    }

    public override Task<IEnumerable<Product>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      var query = _products.Values.AsQueryable();

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

    public override Task<Product?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (_products.TryGetValue(id, out var product) && (includeDeleted || !product.IsDeleted))
      {
        return Task.FromResult<Product?>(product);
      }
      return Task.FromResult<Product?>(null);
    }

    public override Task<Product> CreateAsync(RegisterProductDTO productDTO)
    {
      var product = _productMapper.MapCreate(productDTO);
      product.ProductID = Guid.NewGuid();
      _products[product.ProductID] = product;
      SaveProductsToFile();
      return Task.FromResult(product);
    }

    public override Task UpdateAsync(Guid id, UpdateProductDTO productDTO)
    {
      if (_products.TryGetValue(id, out var existingProduct))
      {
        _productMapper.MapUpdate(existingProduct, productDTO);
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task DeleteAsync(Guid id)
    {
      if (_products.TryGetValue(id, out var product))
      {
        product.IsDeleted = true;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task UndeleteAsync(Guid id)
    {
      if (_products.TryGetValue(id, out var product) && product.IsDeleted)
      {
        product.IsDeleted = false;
        SaveProductsToFile();
      }
      return Task.CompletedTask;
    }

    public override Task<bool> ExistsByNameAsync(string name)
    {
      var exists = _products.Values.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !p.IsDeleted);
      return Task.FromResult(exists);
    }
  }
}

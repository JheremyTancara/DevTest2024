using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.Core.Mapping.Interface;
using Project.DataAccess.Base;

namespace Project.DataAccess.Repositories.DataPersistence
{
  public class ProductRepository : RepositoryBase<Product, RegisterProductDTO, UpdateProductDTO>
  {
    private readonly AppDataContext _context;
    private readonly IMapper<Product, RegisterProductDTO, UpdateProductDTO> _productMapper;

    public ProductRepository(AppDataContext context, IMapper<Product, RegisterProductDTO, UpdateProductDTO> productMapper)
    {
      _context = context;
      _productMapper = productMapper;
    }

    public override async Task<IEnumerable<Product>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string sort = "+CreatedAt", bool includeDeleted = false)
    {
      IQueryable<Product> query;

      if (includeDeleted) {
        query = _context.Products;
      } 
      else {
        query = _context.Products.Where(p => !p.IsDeleted);
      }

      var sortBy = sort.TrimStart('+', '-'); 
      if (sort.StartsWith("-"))
      {
        query = query.OrderByDescending(p => EF.Property<object>(p, sortBy));
      }
      else
      {
        query = query.OrderBy(p => EF.Property<object>(p, sortBy));
      }
      return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public override async Task<Product?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
      if (includeDeleted)
      {
        return await _context.Products.FindAsync(id);
      }

      var product = await _context.Products.FindAsync(id);
      return product != null && !product.IsDeleted ? product : null;
    }

    public override async Task<Product> CreateAsync(RegisterProductDTO productDTO)
    {
      var product = _productMapper.MapCreate(productDTO);
      product.ProductID = Guid.NewGuid();

      _context.Products.Add(product);
      await _context.SaveChangesAsync();

      return product;
    }

    public override async Task UpdateAsync(Guid id, UpdateProductDTO productDTO)
    {
      var existingProduct = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Product with ID {id} was not found.");

      _productMapper.MapUpdate(existingProduct, productDTO);
      await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(Guid id)
    {
      var productToDelete = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Product with ID {id} was not found.");

      productToDelete.IsDeleted = true;
      await _context.SaveChangesAsync();
    }

    public override async Task UndeleteAsync(Guid id)
    {
      var productToRestore = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id && p.IsDeleted) ??                
      throw new KeyNotFoundException($"Product with ID {id} was not found or is already active.");

      productToRestore.IsDeleted = false;
      await _context.SaveChangesAsync();
    }

    public override async Task<bool> ExistsByNameAsync(string name)
    {
      return await _context.Products
        .AnyAsync(p => p.Name.ToLower() == name.ToLower() && !p.IsDeleted);
    }
  }
}

using Project.DataAccess.Interface;

namespace Project.DataAccess.Base
{
  public abstract class RepositoryBase<TEntity, TCreateDto, TUpdateDto> : IRepository<TEntity, TCreateDto, TUpdateDto>
  {
    public abstract Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int pageSize, string sort, bool includeDeleted = false);
    public abstract Task<TEntity?> GetByIdAsync(Guid id, bool includeDeleted = false);
    public abstract Task<TEntity> CreateAsync(TCreateDto entity);
    public abstract Task UpdateAsync(Guid id, TUpdateDto entity);
    public abstract Task DeleteAsync(Guid id);
    public abstract Task UndeleteAsync(Guid id);
    public abstract Task<bool> ExistsByNameAsync(string name);
  }
}

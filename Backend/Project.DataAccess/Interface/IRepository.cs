namespace Project.DataAccess.Interface
{
    public interface IRepository<TEntity, TCreateDto, TUpdateDto> : IEntityRepository
    {
      Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int pageSize, string sort, bool includeDeleted = false);
      Task<TEntity?> GetByIdAsync(Guid id, bool includeDeleted = false);
      Task<TEntity> CreateAsync(TCreateDto createDto);
      Task UpdateAsync(Guid id, TUpdateDto updateDto0);
      Task DeleteAsync(Guid id);
      Task UndeleteAsync(Guid id);     
    }
}
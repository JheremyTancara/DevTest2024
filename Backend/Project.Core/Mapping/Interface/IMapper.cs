namespace Project.Core.Mapping.Interface
{
  public interface IMapper<TEntity, TCreateDto, TUpdateDto>
  {
    TEntity MapCreate(TCreateDto source);
    void MapUpdate(TEntity existingEntity, TUpdateDto updateDto);
  }
}

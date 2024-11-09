namespace Project.DataAccess.Interface
{
    public interface IEntityRepository
    {
      Task<bool> ExistsByNameAsync(string name);
    }
}
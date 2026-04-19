using System.Linq.Expressions;


namespace WebAppPassport.DataBase.Repositories;

public interface IRepository<T>
{

    public Task<ICollection<T>> GetAllAsync();
    
    public Task AddAsync(T entity);
    
    public Task<ICollection<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
}
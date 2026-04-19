using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Models;


namespace WebAppPassport.DataBase.Repositories;



public class UserRepository(AppContext context): BaseRepository(context), IRepository<User>
{
    public async Task<ICollection<User>> GetAllAsync()
    {
        return await Context.Users.ToListAsync();
    }

    public async Task AddAsync(User entity)
    {
        await Context.Users.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<ICollection<User>> GetByConditionAsync(Expression<Func<User, bool>> predicate)
    {
        return await Context.Users.Where(predicate).ToListAsync();
    }
}
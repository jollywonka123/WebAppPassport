using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Models;


namespace WebAppPassport.DataBase.Repositories;

public class CountryRepository(AppContext context): BaseRepository(context), IRepository<Country>
{
    public async Task<ICollection<Country>> GetAllAsync()
    {
        return await Context.Countries.ToListAsync();
    }

    public async Task AddAsync(Country entity)
    {
        await Context.Countries.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<ICollection<Country>> GetByConditionAsync(Expression<Func<Country, bool>> predicate)
    {
        return await Context.Countries.Where(predicate).ToListAsync();
    }
}
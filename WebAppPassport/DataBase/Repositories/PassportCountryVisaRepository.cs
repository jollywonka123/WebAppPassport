using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Models;
using AppContext = WebAppPassport.DataBase.AppContext;

namespace WebAppPassport.DataBase.Repositories;

public class PassportCountryVisaRepository(AppContext context): BaseRepository(context), IRepository<PassportCountryVisa>
{
    public async Task<ICollection<PassportCountryVisa>> GetAllAsync()
    {
        return await Context.Destinations.ToListAsync();
    }

    public async Task AddAsync(PassportCountryVisa entity)
    {
        await Context.Destinations.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<ICollection<PassportCountryVisa>> GetByConditionAsync(Expression<Func<PassportCountryVisa, bool>> predicate)
    {
        return await Context.Destinations.Where(predicate).ToListAsync();
    }
}
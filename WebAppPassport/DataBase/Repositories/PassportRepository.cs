using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Models;

namespace WebAppPassport.DataBase.Repositories;
using AppContext = WebAppPassport.DataBase.AppContext;

public class PassportRepository(AppContext context): BaseRepository(context), IRepository<Passport>
{
    public async Task<ICollection<Passport>> GetAllAsync()
    {
        return await Context.Passports.ToListAsync();
    }

    public async Task AddAsync(Passport entity)
    {
        await Context.Passports.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<ICollection<Passport>> GetByConditionAsync(Expression<Func<Passport, bool>> predicate)
    {
        return await Context.Passports.Where(predicate).ToListAsync();
    }
}
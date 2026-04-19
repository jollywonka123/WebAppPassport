using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Models;

namespace WebAppPassport.DataBase.Repositories;

public class PassportUserRepository(AppContext context): BaseRepository(context), IRepository<(Passport, User)>
{
    public async Task<ICollection<(Passport, User)>> GetAllAsync()
    {
        var users = await Context.Users.Include(x => x.Passports).ToListAsync();

        return users
            .SelectMany(
                user => user.Passports,
                (user, passport) => (passport, user)
            )
            .ToList();
    }

    public async Task AddAsync((Passport, User) entity)
    {
        entity.Item1.Users.Add(entity.Item2);
        entity.Item2.Passports.Add(entity.Item1);
        await Context.SaveChangesAsync();
    }

    public async Task<ICollection<(Passport, User)>> GetByConditionAsync(Expression<Func<(Passport, User), bool>> predicate)
    {
        var users = await Context.Users.Include(x => x.Passports).ToListAsync();
        var func = predicate.Compile();
        return users
            .SelectMany(
                user => user.Passports,
                (user, passport) => (passport, user)
            )
            .Where(func)
            .ToList();
    }
}
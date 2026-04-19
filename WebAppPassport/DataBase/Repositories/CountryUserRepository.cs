using System.Linq.Expressions;
using WebAppPassport.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAppPassport.DataBase.Repositories;

public class CountryUserRepository(AppContext context): BaseRepository(context), IRepository<(Country, User)>
{
    public async Task<ICollection<(Country, User)>> GetAllAsync()
    {
        var users = await Context.Users.Include(x => x.Countries).ToListAsync();
        if (users.Any())
        {
            return users
                .SelectMany(
                    user => user.Countries!,
                    (user, country) => (country, user)
                )
                .ToList();
        }
        return new List<(Country, User)>();
    }

    public async Task AddAsync((Country, User) entity)
    {
        if (entity.Item1.Users != null && entity.Item2.Countries != null)
        {
            entity.Item1.Users.Add(entity.Item2);
            entity.Item2.Countries.Add(entity.Item1);
            await Context.SaveChangesAsync();
        }
    }

    public async Task<ICollection<(Country, User)>> GetByConditionAsync(Expression<Func<(Country, User), bool>> predicate)
    {
        var users = await Context.Users.Include(x => x.Countries).ToListAsync();
        if (users.Any())
        {
            var func = predicate.Compile();
            return users
                .SelectMany(
                    user => user.Countries!,
                    (user, country) => (country, user)
                )
                .Where(func)
                .ToList();
        }
        return new List<(Country, User)>();
    }
}
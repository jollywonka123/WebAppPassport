
namespace WebAppPassport.DataBase.Repositories;

public class BaseRepository(AppContext context)
{
    protected AppContext Context { get; set; } = context;
}
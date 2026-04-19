using WebAppPassport.DataBase.StaticData.ModelsForStatic;

namespace WebAppPassport.DataBase.Repositories;

public interface IStaticRepository
{
    public ICollection<LossModelByDualCitizenship> GetAllLossModels();
    
    public ICollection<PopulationModel> GetAllPopulations();
}
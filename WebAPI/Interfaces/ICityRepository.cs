using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        //not async beacuseas this method just add entity in memory
        void AddCity(City city);

        void DeleteCity(int CityId);
        Task<City> FindCity(int id);

        ////it may take longer time due to this async
        //Task<bool> SaveAsync();

    }
}

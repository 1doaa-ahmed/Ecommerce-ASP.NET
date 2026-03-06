using Project_E_commerse.Models;
using Project_E_commerse.Repositories.Interfaces;

namespace Project_E_commerse.Services.Address
{
    public interface IAddressService : IRepository<Project_E_commerse.Models.Address>
    {
        Task<IEnumerable<Project_E_commerse.Models.Address>> GetByUserAsync(string userId);
        Task<Project_E_commerse.Models.Address?> GetDefaultAsync(string userId);
    }
}
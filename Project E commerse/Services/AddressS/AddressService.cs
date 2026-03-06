using Microsoft.EntityFrameworkCore;
using Project_E_commerse.Data;

using Project_E_commerse.Repositories;

namespace Project_E_commerse.Services.Address
{
    public class AddressService : Repository<Project_E_commerse.Models.Address>, IAddressService
    {
        public AddressService(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Project_E_commerse.Models.Address>> GetByUserAsync(string userId)
            => await _dbSet
                .Where(a => a.UserId == userId)
                .ToListAsync();

        public async Task<Project_E_commerse.Models.Address?> GetDefaultAsync(string userId)
            => await _dbSet
                .Where(a => a.UserId == userId && a.IsDefault)
                .FirstOrDefaultAsync();
    }
}
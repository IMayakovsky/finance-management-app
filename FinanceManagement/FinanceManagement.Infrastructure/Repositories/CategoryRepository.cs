using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface ICategoryRepository : IRepository
    {
        Task<Category> GetByIdAndUserId(int categoryId, int userId);
        Task<List<Category>> GetDefaultCategories();
        Task<List<Category>> GetByUserId(int userId);
    }

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public async Task<Category> GetByIdAndUserId(int categoryId, int userId)
        {
            return await Entities.Where(e => e.Id == categoryId && e.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<Category>> GetDefaultCategories()
        {
            return await Entities.Where(e => !e.UserId.HasValue).ToListAsync();
        }

        public async Task<List<Category>> GetByUserId(int userId)
        {
            return await Entities.Where(e => e.UserId == userId).ToListAsync();
        }
    }
}

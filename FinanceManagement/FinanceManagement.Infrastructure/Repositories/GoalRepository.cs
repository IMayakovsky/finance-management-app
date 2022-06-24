using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IGoalRepository : IRepository
    {
        Task<Goal> GetByIdAndUserId(int goalId, int userId);
        Task<List<Goal>> GetByUserId(int userId);
        Task<List<Goal>> GetExpiredGoals(DateTime maxDateTime);
    }

    public class GoalRepository : BaseRepository<Goal>, IGoalRepository
    {
        public async Task<Goal> GetByIdAndUserId(int goalId, int userId)
        {
            return await Entities.Where(e => e.Id == goalId && e.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<Goal>> GetByUserId(int userId)
        {
            return await Entities.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<List<Goal>> GetExpiredGoals(DateTime maxDateTime)
        {
            return await Entities.Where(e => e.DateTo < maxDateTime).ToListAsync();
        }
    }
}

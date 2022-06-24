using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public interface IDebtRepository : IRepository
    {
        Task<Debt> GetByIdAndUserId(int debtId, int userId);
        Task<List<Debt>> GetByUserId(int userId);
        Task<List<Debt>> GetExpiredDebts(DateTime maxDateTime);
    }

    public class DebtRepository : BaseRepository<Debt>, IDebtRepository
    {
        public async Task<Debt> GetByIdAndUserId(int subscriptionId, int userId)
        {
            return await Entities.Where(e => e.Id == subscriptionId && e.UserId == userId && !e.IsClosed).FirstOrDefaultAsync();
        }

        public async Task<List<Debt>> GetByUserId(int userId)
        {
            return await Entities.Where(e => e.UserId == userId && !e.IsClosed).ToListAsync();
        }

        public async Task<List<Debt>> GetExpiredDebts(DateTime maxDateTime)
        {
            return await Entities.Where(e => e.DueTo.HasValue && e.DueTo < maxDateTime && !e.IsClosed).ToListAsync();
        }
    }
}

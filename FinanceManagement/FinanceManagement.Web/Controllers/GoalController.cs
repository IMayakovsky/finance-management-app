using FinanceManagement.Infrastructure.Attributes;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManagement.Web.Controllers
{
    /// <summary>
    /// Controlller for Goals Operations
    /// </summary>
    [BaseAuthorize]
    [Route("api/goals")]
    public class GoalController : BaseController
    {
        private readonly Lazy<IGoalOperation> goalOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="goalOperation"></param>
        public GoalController(Lazy<IGoalOperation> goalOperation)
        {
            this.goalOperation = goalOperation;
        }

        /// <summary>
        /// Returns User's Goals
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<GoalDto>> GetUserGoals()
        {
            return await goalOperation.Value.GetUserGoalsCached(userId);
        }

        /// <summary>
        /// Deletes Goal
        /// </summary>
        /// <param name="goalId"></param>
        /// <returns></returns>
        [HttpDelete("{goalId}")]
        public async Task DeleteGoal(int goalId)
        {
            await goalOperation.Value.DeleteGoal(goalId, this.userId);
        }

        /// <summary>
        /// Creates Goal
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GoalDto> InsertGoal(GoalDto goal)
        {
            return await goalOperation.Value.InsertGoal(goal, this.userId);
        }

        /// <summary>
        /// Updates Goal
        /// </summary>
        /// <param name="goalId"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        [HttpPut("{goalId}")]
        public async Task UpdateGoal(int goalId, GoalDto goal)
        {
            goal.Id = goalId;

            await goalOperation.Value.UpdateGoal(goal, this.userId);
        }

        /// <summary>
        /// Change Goal amount
        /// </summary>
        /// <param name="goalId"></param>
        /// <returns></returns>
        [AccountAccess]
        [HttpPatch("{goalId}/amount")]
        public async Task ChangeGoalAmount(int goalId, int accountId, double amount)
        {
            await goalOperation.Value.ChangeGoalAmount(goalId, accountId, amount, this.userId);
        }

        /// <summary>
        /// Closes Goal
        /// </summary>
        /// <param name="goalId"></param>
        /// <returns></returns>
        [HttpPatch("{goalId}/close")]
        public async Task CloseGoal(int goalId)
        {
            await goalOperation.Value.CloseGoal(goalId, this.userId);
        }
    }
}

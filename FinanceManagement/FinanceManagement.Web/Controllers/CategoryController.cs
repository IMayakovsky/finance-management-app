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
    /// Controller for Categories Operations
    /// </summary>
    [BaseAuthorize]
    [Route("api/categories")]
    public class CategoryController : BaseController
    {
        private readonly Lazy<ITransactionOperation> transactionOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionOperation"></param>
        public CategoryController(Lazy<ITransactionOperation> transactionOperation)
        {
            this.transactionOperation = transactionOperation;
        }

        /// <summary>
        /// Returns User's Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("userCategories")]
        public async Task<List<CategoryDto>> GetUserCategories()
        {
            return await transactionOperation.Value.GetUserCategoriesCached(userId);
        }

        /// <summary>
        /// Returns default/system categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("defaultCategories")]
        public async Task<List<CategoryDto>> GetDefaultCategories()
        {
            return await transactionOperation.Value.GetDefaultCategoriesCached();
        }

        /// <summary>
        /// Deletes User Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("{categoryId}")]
        public async Task DeleteUserCategory(int categoryId)
        {
            await transactionOperation.Value.DeleteUserCategory(userId, categoryId);
        }

        /// <summary>
        /// Add User Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task InsertUserCategory(CategoryDto category)
        {
            await transactionOperation.Value.InsertUserCategory(userId, category);
        }

        /// <summary>
        /// Updates User Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{categoryId}")]
        public async Task UpdateUserCategoryName(int categoryId, string name)
        {
            await transactionOperation.Value.UpdateUserCategoryName(userId, categoryId, name);
        }
    }
}

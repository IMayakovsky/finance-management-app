using System.Threading.Tasks;

namespace FinanceManagement.ValidityInformer.Processors
{
    internal abstract class BaseProccessor
    {
        /// <summary>
        /// Validates Data in database, creates notification if validation was faild. 
        /// </summary>
        /// <returns></returns>
        public abstract Task Process();
    }
}
